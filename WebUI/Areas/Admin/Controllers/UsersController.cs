using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Model;
using Data.Model.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using WebUI.Areas.Admin.Models;
using X.PagedList;

namespace WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UsersController : Controller
    {
        private ApplicationContext _db;
        private readonly AppConfig _config;

        public UsersController(ApplicationContext cntx, IOptions<AppConfig> config)
        {
            _db = cntx;
            _config = config.Value;
        }
        // GET: UsersController
        public ActionResult List(int? page, Guid? roleId, IFormCollection collection)
        {
            var aaa = collection;

            // Устанавливаем номер страницы
            var pageNumber = page ?? 1;
            int pageSize = _config.Admin_Users_List_UsersPerPage;

            // Заполняем категории данными
            ViewBag.Roles = new SelectList(_db.Roles.OrderBy(o => (int)o.RoleType).ToList(), dataValueField: "RoleId", dataTextField: "DisplayName");
            // Устанавливаем выбранную категорию
            ViewBag.SelectedRole = roleId.ToString();

            // Инициализируем List и заполняем данными
            var listOfUsers = from u in _db.Users
                              join ur in _db.UserRoles on u.UserId equals ur.UserId
                              join r in _db.Roles on ur.RoleId equals r.RoleId
                              where roleId == null || roleId == Guid.Empty || r.RoleId == (Guid)roleId
                              select u;

            var listOfUsersVM = (from u in _db.Users
                                 join ur in _db.UserRoles on u.UserId equals ur.UserId
                                 join r in _db.Roles on ur.RoleId equals r.RoleId
                                 where roleId == null || roleId == Guid.Empty || r.RoleId == (Guid)roleId
                                 select new UserListVM(u))
                                             .ToList()
                                             .GroupBy(gr => gr.UserId)
                                             .Select(user => user.First())
                                             .OrderBy(o=>o.UserName);
            // Устанавливаем постраничную навигацию
            var onePageOfUsers = listOfUsersVM.ToPagedList(pageNumber, pageSize: pageSize);
            // Возвращаем в преставление
            return View(onePageOfUsers);
        }

        [HttpGet]
        public IActionResult CreateUser(Guid? roleId)
        {
            ViewBag.NewPassword = Data.Tools.Pasword.Generate(_config.PasswordLenght);
            ViewBag.Roles = _db.Roles.OrderByDescending(x => (int)x.RoleType).Select(s => new RoleVM { RoleId = s.RoleId, RoleName = s.DisplayName, isCheked = s.RoleId == roleId }).ToList();
            return View("CreateUser");
        }
        [HttpPost]
        //[ActionName("create-account")]
        public IActionResult CreateUser(UserEditVM model, IFormCollection collection)
        {
            var rolesCol = collection.Where(w => w.Key.StartsWith("chk_")).Select(s => new Guid(s.Value)).ToList();

            //Проверяем модель на валидность
            if (!ModelState.IsValid)
            {
                ViewBag.Roles = _db.Roles.OrderByDescending(x => (int)x.RoleType).Select(s => new RoleVM { RoleId = s.RoleId, RoleName = s.DisplayName, isCheked = rolesCol.Contains(s.RoleId) }).ToList();
                return View(model);
            }

            if (!rolesCol.Any())
            {
                ViewBag.Roles = _db.Roles.OrderByDescending(x => (int)x.RoleType).Select(s => new RoleVM { RoleId = s.RoleId, RoleName = s.DisplayName, isCheked = rolesCol.Contains(s.RoleId) }).ToList();
                ModelState.AddModelError("", "Assign a user role!");
                return View(model);
            }

            if (string.IsNullOrEmpty(model.Password) || model.Password.Length < _config.PasswordLenght)
            {
                ViewBag.Roles = _db.Roles.OrderByDescending(x => (int)x.RoleType).Select(s => new RoleVM { RoleId = s.RoleId, RoleName = s.DisplayName, isCheked = rolesCol.Contains(s.RoleId) }).ToList();
                ModelState.AddModelError("", $"Password must be at least {_config.PasswordLenght} characters!");
                return View(model);
            }

            // Проверяем имя на уникальность
            if (_db.Users.Any(a => a.UserName.Equals(model.UserName)))
            {
                ViewBag.Roles = _db.Roles.OrderByDescending(x => (int)x.RoleType).Select(s => new RoleVM { RoleId = s.RoleId, RoleName = s.DisplayName, isCheked = rolesCol.Contains(s.RoleId) }).ToList();
                ModelState.AddModelError("", $"Login {model.UserName} is taken!");
                model.UserName = string.Empty;
                return View(model);
            }

            // Создать экземпляр UserDTO
            var userDTO = new AppUser()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                EmailAddress = model.Email,
                UserName = model.UserName,
                Password = model.Password
            };
            _db.Users.Add(userDTO);

            // Добавить роли пользователю
            var userRoles = new List<UserRole>();
            foreach (var role in rolesCol)
            {
                _db.UserRoles.Add(new UserRole()
                {
                    UserId = userDTO.UserId,
                    RoleId = role
                });
            }

            // Сохранить данные
            _db.SaveChanges();
            Emailer.SendUserRegistrationMail(_config.AdministrationEmail, userDTO);
            // Записать сообщение
            TempData["SM"] = "You are added a new user.";
            return RedirectToAction("List");
        }
        [HttpGet]
        public IActionResult DeleteUser(Guid id)
        {
            // Удаляем связи с ролями
            var roles = _db.UserRoles.Where(x => x.UserId == id);
            foreach (var role in roles)
            {
                _db.UserRoles.Remove(role);
            }
            // Удаляем пользователя из БД
            var dto = _db.Users.Find(id);
            _db.Users.Remove(dto);
            _db.SaveChanges();

            TempData["SM"] = "The user was deleted!";
            return RedirectToAction("List");
        }
    }
}

