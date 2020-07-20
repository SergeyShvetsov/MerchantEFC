using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Model;
using Data.Model.Extensions;
using Data.Model.Interfaces;
using Data.Model.Models;
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
    public class UsersController : Controller
    {
        //private ApplicationContext _cntx;
        private readonly AppConfig _config;
        private readonly IEntityContext _cntx;
        private readonly IStringLocalizer _resources;

        public UsersController(IOptions<AppConfig> config, IEntityContext context, IStringLocalizerFactory localizer)
        {
            // _cntx = cntx;
            _config = config.Value;
            _cntx = context;
            _resources = localizer.GetLocalResources();
        }
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );
            return LocalRedirect(returnUrl);
        }
        // GET: UsersController
        public ActionResult List(int? page, Guid? roleId)
        {
            // Устанавливаем номер страницы
            var pageNumber = page ?? 1;
            int pageSize = _config.Admin_Users_List_UsersPerPage;

            // Заполняем список ролей данными
            ViewBag.Roles = new SelectList(_cntx.Roles.GetAll().OrderBy(o => (int)o.RoleType).ToList(), dataValueField: "RoleId", dataTextField: "DisplayName");
            //// Заполняем список stores данными
            //ViewBag.Stores = new SelectList(_cntx.Stores.GetAll().OrderBy(o => o.StoreName).ToList(), dataValueField: "Id", dataTextField: "StoreName");

            // Устанавливаем выбранную роль
            ViewBag.SelectedRole = roleId.ToString();

            // Инициализируем List и заполняем данными
            var listOfUsers = from ur in _cntx.UserRoles.GetAll().ApplyArchivedFilter()
                              where roleId == null || roleId == Guid.Empty || ur.RoleId == (Guid)roleId
                              select ur.AppUser;

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
        public IActionResult DeleteUser(Guid id)
        {
            var user = _cntx.Users.GetById(id);
            // Удаляем связи с ролями
            _cntx.Context.RemoveAllRolesFromUser(user);
            // Удаляем пользователя из БД
            _cntx.Users.Delete(user);
            _cntx.Save();

            TempData["SM"] = _resources["UserWasDeleted"];
            return RedirectToAction("List");
        }

        [HttpGet]
        public IActionResult CreateUser(Guid? roleId, int? storeId)
        {
            ViewBag.AvailableRoles = GetAvailableRoles(new List<Guid>());
            ViewBag.AvailableStores = GetAvailableStores();
            ViewBag.AvailableStatuses = AvailableStatuses();

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
            var rolesIdsCol = collection["UserRoles"].Select(s => new Guid(s));

            //Проверяем модель на валидность
            if (!ModelState.IsValid)
            {
                ViewBag.AvailableRoles = GetAvailableRoles(rolesIdsCol);
                ViewBag.AvailableStores = GetAvailableStores();
                ViewBag.AvailableStatuses = AvailableStatuses();
                return View(model);
            }

            if (string.IsNullOrEmpty(model.Password) || model.Password.Length < _config.PasswordLenght)
            {
                ViewBag.AvailableRoles = GetAvailableRoles(rolesIdsCol);
                ViewBag.AvailableStores = GetAvailableStores();
                ViewBag.AvailableStatuses = AvailableStatuses();
                ModelState.AddModelError("", string.Format(_resources["InvalidPasswordLenght"], _config.PasswordLenght));
                return View(model);
            }

            // Проверяем имя на уникальность
            if (!_cntx.Users.IsUniqName(model.UserName))
            {
                ViewBag.AvailableRoles = GetAvailableRoles(rolesIdsCol);
                ViewBag.AvailableStores = GetAvailableStores();
                ViewBag.AvailableStatuses = AvailableStatuses();
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
                Password = model.Password
            };
            if (model.AssignedStoreId != null)
            {
                userDTO.AssignedStore = _cntx.Stores.GetById((int)model.AssignedStoreId);
            }
            else
            {
                userDTO.AssignedStore = null;
            }

            _cntx.Users.Insert(userDTO);
            _cntx.Save();

            // Добавить роли пользователю
            _cntx.Context.AddRolesToUser(userDTO, rolesIdsCol);

            // Сохранить данные
            _cntx.Save();

            Emailer.SendUserRegistrationMail(_config.AdministrationEmail, userDTO);
            // Записать сообщение
            TempData["SM"] = _resources["NewUserAdded"];
            return RedirectToAction("List");
        }

        [HttpGet]
        public IActionResult EditUser(Guid id)
        {
            ViewBag.AvailableRoles = GetAvailableRoles(new List<Guid>());
            ViewBag.AvailableStores = GetAvailableStores();
            ViewBag.AvailableStatuses = AvailableStatuses();
            var user = _cntx.Users.GetById(id);

            return View("EditUser", new UserEditVM(user));
        }
        [HttpPost]
        public IActionResult EditUser(UserEditVM model, IFormCollection collection)
        {
            var rolesIdsCol = collection["UserRoles"].Select(s => new Guid(s));

            //Проверяем модель на валидность
            if (!ModelState.IsValid)
            {
                ViewBag.AvailableRoles = GetAvailableRoles(rolesIdsCol);
                ViewBag.AvailableStores = GetAvailableStores();
                ViewBag.AvailableStatuses = AvailableStatuses();
                return View(model);
            }

            if (string.IsNullOrEmpty(model.Password) || model.Password.Length < _config.PasswordLenght)
            {
                ViewBag.AvailableRoles = GetAvailableRoles(rolesIdsCol);
                ViewBag.AvailableStores = GetAvailableStores();
                ViewBag.AvailableStatuses = AvailableStatuses();
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

            if (model.AssignedStoreId != null)
            {
                userDTO.AssignedStore = _cntx.Stores.GetById((int)model.AssignedStoreId);
            }
            else
            {
                userDTO.AssignedStore = null;
            }

            // Добавить роли пользователю
            _cntx.Context.RemoveAllRolesFromUser(userDTO);
            _cntx.Context.AddRolesToUser(userDTO, rolesIdsCol);

            _cntx.Users.Update(userDTO);

            // Сохранить данные
            _cntx.Save();

            // Записать сообщение
            TempData["SM"] = _resources["UserWasEdited"];
            return RedirectToAction("List");
        }
        private List<RoleVM> GetAvailableRoles(IEnumerable<Guid> rolesIds)
        {
            return _cntx.Roles.GetAll().OrderByDescending(x => (int)x.RoleType).Select(s => new RoleVM { RoleId = s.RoleId, RoleName = s.DisplayName, isCheked = rolesIds.Contains(s.RoleId) }).ToList();
        }
        private List<StoreVM> GetAvailableStores()
        {
            return _cntx.Stores.GetAll().OrderBy(x => x.StoreName).Select(s => new StoreVM { StoreId = s.Id, StoreCode = s.StoreCode, StoreName = s.StoreName }).ToList();
        }
        private List<Status> AvailableStatuses()
        {
            return Enum.GetValues(typeof(Status)).Cast<Status>().Select(v => v).ToList();
        }


    }
}

