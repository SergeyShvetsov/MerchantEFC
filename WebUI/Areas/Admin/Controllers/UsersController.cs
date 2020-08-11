using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Model;
using Data.Model.Extensions;
using Data.Model.Interfaces;
using Data.Model.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using WebUI.Areas.Admin.Models;
using WebUI.Extensions;
using X.PagedList;

namespace WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,SuperUser")]
    public class UsersController : Controller
    {
        //private ApplicationContext _cntx;
        private readonly AppConfig _config;
        private readonly IEntityContext _cntx;
        private readonly IStringLocalizer _resources;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;

        private IEnumerable<RoleType> _availableRoles => _cntx.GetAvailableRoles(User);
        private IEnumerable<Status> _availableStatuses => _cntx.GetAvailableStatuses(User);
        private readonly IEnumerable<Store> _availableStores;

        public UsersController(IOptions<AppConfig> config, IEntityContext context, IStringLocalizerFactory localizer, IHttpContextAccessor httpContextAccessor)
        {
            // _cntx = cntx;
            _config = config.Value;
            _cntx = context;
            _resources = localizer.GetLocalResources();
            _httpContextAccessor = httpContextAccessor;
            _availableStores = _cntx.GetAvailableStores(_session);
        }
        // GET: UsersController
        public ActionResult List(int? page, int roleId)
        {
            ViewBag.TabItem = "Users";

            // Устанавливаем номер страницы
            var pageNumber = page ?? 1;
            int pageSize = _config.Admin_RowsPerPage;

            // Заполняем список ролей данными
            ViewBag.AvailableRoles = _availableRoles;
            ViewBag.SelectedRole = roleId;

            // Инициализируем List и заполняем данными
            var listOfUsers = _cntx.Users.GetAllByRole((RoleType)roleId)
                .ApplyArchivedFilter();

            if (!User.IsInRole("Admin"))
            {
                listOfUsers = listOfUsers.ApplyAvailableFilter(_availableStores);
            }

            var listOfUsersVM = listOfUsers
                .Select(s => new UserListVM(s))
                .ToList()
                .GroupBy(gr => gr.UserId)
                .Select(user => user.First())
                .OrderBy(o => o.UserName);

            // Устанавливаем постраничную навигацию
            var onePageOfUsers = listOfUsersVM.ToPagedList(pageNumber, pageSize: pageSize);

            // Возвращаем в преставление

            return View(onePageOfUsers);
        }

        [HttpGet]
        public IActionResult DeleteUser(Guid id, int roleId)
        {
            var user = _cntx.Users.GetById(id);
            var model = new UserDeleteVM
            {
                UserId = id,
                RoleId = roleId,
                UserName = user.UserName
            };

            return PartialView("_DeleteUserModal", model);
        }
        [HttpPost]
        public IActionResult DeleteUser(UserDeleteVM model)
        {
            var user = _cntx.Users.GetById(model.UserId);
            // Удаляем связи с ролями
            // Удаляем пользователя из БД
            _cntx.Users.Delete(user);
            _cntx.Save();

            TempData["SM"] = _resources["UserWasDeleted"].Value;
            return RedirectToAction("List");
        }

        [HttpGet]
        public IActionResult CreateUser(Guid? roleId, int? storeId)
        {
            ViewBag.TabItem = "Users";
            ViewBag.AvailableRoles = _availableRoles;
            ViewBag.AvailableStores = _availableStores;
            ViewBag.AvailableStatuses = _availableStatuses;

            var model = new UserEditVM()
            {
                Password = Data.Tools.Pasword.Generate(_config.PasswordLenght),
                UserStatus = Status.Active
            };

            return View("CreateUser", model);
        }

        [HttpPost]
        //[ActionName("create-account")]
        public IActionResult CreateUser(UserEditVM model, IFormCollection collection)
        {
            ViewBag.TabItem = "Users";
            var rolesIdsCol = collection["UserRoles"].Select(s => new Guid(s));

            //Проверяем модель на валидность
            if (!ModelState.IsValid)
            {
                ViewBag.AvailableRoles = _availableRoles;
                ViewBag.AvailableStores = _availableStores;
                ViewBag.AvailableStatuses = _availableStatuses;
                return View(model);
            }

            if (string.IsNullOrEmpty(model.Password) || model.Password.Length < _config.PasswordLenght)
            {
                ViewBag.AvailableRoles = _availableRoles;
                ViewBag.AvailableStores = _availableStores;
                ViewBag.AvailableStatuses = _availableStatuses;
                ModelState.AddModelError("", string.Format(_resources["InvalidPasswordLenght"], _config.PasswordLenght));
                return View(model);
            }

            // Проверяем имя на уникальность
            if (!_cntx.Users.IsUniqName(model.UserName))
            {
                ViewBag.AvailableRoles = _availableRoles;
                ViewBag.AvailableStores = _availableStores;
                ViewBag.AvailableStatuses = _availableStatuses;
                ModelState.AddModelError("", string.Format(_resources["LoginIsTaken"], model.UserName));
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
                Password = model.Password,
                UserRole = model.UserRole
            };
            if (model.StoreId != null)
            {
                userDTO.Store = _cntx.Stores.GetById((int)model.StoreId);
            }
            else
            {
                userDTO.Store = null;
            }

            _cntx.Users.Insert(userDTO);

            // Сохранить данные
            _cntx.Save();

            Emailer.SendUserRegistrationMail(_config.AdministrationEmail, userDTO);
            // Записать сообщение
            TempData["SM"] = _resources["NewUserAdded"].Value;
            return RedirectToAction("List");
        }

        [HttpGet]
        public IActionResult EditUser(Guid id)
        {
            ViewBag.TabItem = "Users";
            ViewBag.AvailableRoles = _availableRoles;
            ViewBag.AvailableStores = _availableStores;
            ViewBag.AvailableStatuses = _availableStatuses;
            var user = _cntx.Users.GetById(id);

            return View("EditUser", new UserEditVM(user));
        }
        [HttpPost]
        public IActionResult EditUser(UserEditVM model, IFormCollection collection)
        {
            ViewBag.TabItem = "Users";
            var rolesIdsCol = collection["UserRoles"].Select(s => new Guid(s));

            //Проверяем модель на валидность
            if (!ModelState.IsValid)
            {
                ViewBag.AvailableRoles = _availableRoles;
                ViewBag.AvailableStores = _availableStores;
                ViewBag.AvailableStatuses = _availableStatuses;
                return View(model);
            }

            if (string.IsNullOrEmpty(model.Password) || model.Password.Length < _config.PasswordLenght)
            {
                ViewBag.AvailableRoles = _availableRoles;
                ViewBag.AvailableStores = _availableStores;
                ViewBag.AvailableStatuses = _availableStatuses;
                ModelState.AddModelError("", string.Format(_resources["InvalidPasswordLenght"], _config.PasswordLenght));
                return View(model);
            }

            // Получить экземпляр UserDTO
            var userDTO = _cntx.Users.GetById(model.UserId);

            userDTO.FirstName = model.FirstName;
            userDTO.LastName = model.LastName;
            userDTO.EmailAddress = model.Email;
            userDTO.UserName = model.UserName;
            userDTO.Password = model.Password;
            userDTO.UserRole = model.UserRole;

            // Проверяем имя на уникальность
            if (!_cntx.Users.IsUniqName(model.UserName))
            {
                ViewBag.AvailableRoles = _availableRoles;
                ViewBag.AvailableStores = _availableStores;
                ViewBag.AvailableStatuses = _availableStatuses;
                ModelState.AddModelError("", string.Format(_resources["LoginIsTaken"], model.UserName));
                model.UserName = userDTO.UserName;
                return View(model);
            }

            if (model.StoreId != null)
            {
                userDTO.Store = _cntx.Stores.GetById((int)model.StoreId);
            }
            else
            {
                userDTO.Store = null;
            }

            // Добавить роли пользователю
            //_cntx.Context.RemoveAllRolesFromUser(userDTO);
            //_cntx.Context.AddRolesToUser(userDTO, rolesIdsCol);

            _cntx.Users.Update(userDTO);

            // Сохранить данные
            _cntx.Save();

            // Записать сообщение
            TempData["SM"] = _resources["UserWasEdited"].Value;
            return RedirectToAction("List");
        }
    }
}

