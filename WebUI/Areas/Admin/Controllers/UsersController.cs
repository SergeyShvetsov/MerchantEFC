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

namespace WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,SuperUser")]
    public class UsersController : Controller
    {
        //private ApplicationContext _cntx;
        private readonly AppConfig _config;
        private readonly ApplicationContext _cntx;
        private readonly IStringLocalizer _resources;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;

        private IEnumerable<RoleType> _availableRoles => _cntx.GetAvailableRoles(User);
        private IEnumerable<Status> _availableStatuses => _cntx.GetAvailableStatuses(User);
        private readonly IQueryable<City> _availableCities;
        private readonly IQueryable<Store> _availableStores;
        private readonly IQueryable<Company> _availableCompanies;
        private readonly IQueryable<AppUser> _availableUsers;

        public UsersController(IOptions<AppConfig> config, ApplicationContext context, IStringLocalizerFactory localizer, IHttpContextAccessor httpContextAccessor)
        {
            // _cntx = cntx;
            _config = config.Value;
            _cntx = context;
            _resources = localizer.GetLocalResources();
            _httpContextAccessor = httpContextAccessor;
            _availableCities = _cntx.Cities.ApplySecurityFilter(_session);
            _availableStores = _cntx.Stores.ApplySecurityFilter(_session);
            _availableCompanies = _cntx.Companies.ApplySecurityFilter(_session);
            _availableUsers = _cntx.AppUsers.ApplySecurityFilter(_session);
        }

        private void SetViewBag()
        {
            ViewBag.TabItem = "Users";
            ViewBag.AvailableRoles = _availableRoles;
            ViewBag.AvailableCities = _availableCities.ToList();
            ViewBag.AvailableStores = _availableStores.ToList();
            ViewBag.AvailableCompanies = _availableCompanies.ToList();
            ViewBag.AvailableStatuses = _availableStatuses;
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

            var listOfUsers = _availableUsers;

            if (roleId != 0)
            {
                listOfUsers = _availableUsers.Where(x => x.UserRole == (RoleType)roleId);

            }

            var tmp = listOfUsers.OrderBy(o => o.UserName);
            return View(tmp.ToPagedList(pageNumber, pageSize));
        }

        [HttpGet]
        public IActionResult DeleteUser(Guid id, int roleId)
        {
            var user = _cntx.AppUsers.Find(id);
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
            var user = _cntx.AppUsers.Find(model.UserId);
            user.Archive(_cntx);
            _cntx.SaveChanges();

            TempData["SM"] = _resources["UserWasDeleted"].Value;
            return RedirectToAction("List");
        }

        [HttpGet]
        public IActionResult CreateUser(Guid? roleId, int? storeId)
        {
            SetViewBag();
            var model = new UserEditVM()
            {
                Password = Data.Tools.Pasword.Generate(_config.PasswordLenght),
                UserStatus = Status.Active
            };

            return View("CreateUser", model);
        }

        [HttpPost]
        //[ActionName("create-account")]
        public IActionResult CreateUser(UserEditVM model)
        {
            ViewBag.TabItem = "Users";

            //Проверяем модель на валидность
            if (!ModelState.IsValid)
            {
                SetViewBag();
                return View(model);
            }

            if (string.IsNullOrEmpty(model.Password) || model.Password.Length < _config.PasswordLenght)
            {
                SetViewBag();
                ModelState.AddModelError("", string.Format(_resources["InvalidPasswordLenght"], _config.PasswordLenght));
                return View(model);
            }

            // Проверяем имя на уникальность
            if (_cntx.AppUsers.Any(x => x.UserName == model.UserName && !x.IsArchived))
            {
                SetViewBag();
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
                UserRole = model.UserRole,
                CityId = model.CityId == 0 ? null : model.CityId,
                StoreId = model.StoreId == 0 ? null : model.StoreId,
                CompanyId = model.CompanyId == 0 ? null : model.CompanyId
            };

            _cntx.AppUsers.Add(userDTO);
            _cntx.SaveChanges();

            Emailer.SendUserRegistrationMail(_config.AdministrationEmail, userDTO);
            // Записать сообщение
            TempData["SM"] = _resources["NewUserAdded"].Value;
            return RedirectToAction("List");
        }

        [HttpGet]
        public IActionResult EditUser(Guid id)
        {
            ViewBag.TabItem = "Users";
            SetViewBag();
            var user = _cntx.AppUsers.Find(id);

            return View("EditUser", new UserEditVM(user));
        }
        [HttpPost]
        public IActionResult EditUser(UserEditVM model)
        {
            ViewBag.TabItem = "Users";

            //Проверяем модель на валидность
            if (!ModelState.IsValid)
            {
                SetViewBag();
                return View(model);
            }

            if (string.IsNullOrEmpty(model.Password) || model.Password.Length < _config.PasswordLenght)
            {
                SetViewBag();
                ModelState.AddModelError("", string.Format(_resources["InvalidPasswordLenght"], _config.PasswordLenght));
                return View(model);
            }

            // Получить экземпляр UserDTO
            var userDTO = _cntx.AppUsers.Find(model.UserId);

            userDTO.FirstName = model.FirstName;
            userDTO.LastName = model.LastName;
            userDTO.EmailAddress = model.Email;
            userDTO.UserName = model.UserName;
            userDTO.Password = model.Password;
            userDTO.UserRole = model.UserRole;
            userDTO.CityId = model.CityId == 0 ? null : model.CityId;
            userDTO.StoreId = model.StoreId == 0 ? null : model.StoreId;
            userDTO.CompanyId = model.CompanyId == 0 ? null : model.CompanyId;

            if (userDTO.UserName != model.UserName && _cntx.AppUsers.Any(x => x.UserName == model.UserName && !x.IsArchived))
            {
                SetViewBag();
                ModelState.AddModelError("", string.Format(_resources["LoginIsTaken"], model.UserName));
                model.UserName = userDTO.UserName;
                return View(model);
            }

            _cntx.AppUsers.Update(userDTO);

            // Сохранить данные
            _cntx.SaveChanges();

            // Записать сообщение
            TempData["SM"] = _resources["UserWasEdited"].Value;
            return RedirectToAction("List");
        }
    }
}

