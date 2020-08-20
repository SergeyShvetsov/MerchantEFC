using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Data.Model;
using Data.Model.Extensions;
using Data.Model.Interfaces;
using Data.Model.Models;
using Data.Tools.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using WebUI.Extensions;
using WebUI.Models.Account;

namespace WebUI.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppConfig _config;
        private readonly ApplicationContext _cntx;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IStringLocalizer _resources;
        private ISession _session => _httpContextAccessor.HttpContext.Session;

        public AccountController(IOptions<AppConfig> config, ApplicationContext context, IHttpContextAccessor httpContextAccessor, IStringLocalizerFactory localizer)
        {
            _config = config.Value;
            _cntx = context;
            _httpContextAccessor = httpContextAccessor;
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
        public IActionResult Index()
        {
            return RedirectToAction("Login");
        }

        [HttpGet]
        //[ActionName("create-account")]
        public IActionResult CreateAccount()
        {
            return View("CreateAccount");
        }
        [HttpPost]
        //[ActionName("create-account")]
        public IActionResult CreateAccount(UserVM model)
        {
            // Проверяем модель на валидность
            if (!ModelState.IsValid) return View(model);

            //  Проверяем соответствие пароля
            if (!model.Password.Equals(model.ConfirmPassword))
            {
                ModelState.AddModelError("", _resources["PasswordDoNotMatch"]);
                return View(model);
            }
            // Проверяем имя на уникальность
            if (_cntx.AppUsers.Any(x => x.UserName == model.UserName))
            {
                ModelState.AddModelError("", string.Format(_resources["LoginIsTaken"], model.UserName));
                model.UserName = string.Empty;
                return View(model);
            }
            // Создать экземпляр UserDTO
            var userDTO = new AppUser()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                EmailAddress = model.EmailAddress,
                UserName = model.UserName,
                Password = model.Password,
                UserStatus = Status.Active,
                UserRole = RoleType.User
            };
            _cntx.AppUsers.Add(userDTO);
            _cntx.SaveChanges();

            // Записать сообщение
            TempData["SM"] = _resources["YouAreRegistered"].Value;
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Login()
        {

            // Проверить авторизован ли пользователь
            var userName = User.Identity.Name;
            if (!string.IsNullOrEmpty(userName))
            {
                return RedirectToAction("UserProfile");
            }

            // Проверяем пользователя на валидность
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginUserVM model)
        {
            // Проверяем модель на валидность
            if (!ModelState.IsValid) return View(model);
            var user = _cntx.AppUsers.FirstOrDefault(x => x.UserName == model.UserName);
            bool hasError = false;
            if (user == null)
            {
                ModelState.AddModelError("", _resources["InvalidLoginOrPassword"]);
                return View(model);
            }
            switch (user.UserStatus)
            {
                case Status.Pending:
                    hasError = true;
                    ModelState.AddModelError("", _resources["AccountIsInPending"]);
                    break;
                case Status.InActive:
                    hasError = true;
                    ModelState.AddModelError("", _resources["AccountIsInactive"]);
                    break;
                case Status.Blocked:
                    var date = user.LastVisit.AddDays(1);
                    if (DateTime.Now > date) break;

                    hasError = true;
                    ModelState.AddModelError("", string.Format(_resources["AccountIsBlocked"], date.ToShortDateString(), date.ToShortTimeString()));
                    break;
            }

            if (user == null || user.IsArchived || user.Password != model.Password)
            {
                hasError = true;
                ModelState.AddModelError("", _resources["InvalidLoginOrPassword"]);
                user.AttemptsCount++;
                if (user.AttemptsCount > _config.Admin_RowsPerPage)
                {
                    user.UserStatus = Status.Blocked;
                    user.AttemptsCount = 0;
                }
            }

            if (hasError)
            {
                user.LastVisit = DateTime.Now;
                _cntx.AppUsers.Update(user);
                _cntx.SaveChanges();
                return View(model);
            }

            user.LastVisit = DateTime.Now;
            user.AttemptsCount = 0;
            user.UserStatus = Status.Active;
            _cntx.AppUsers.Update(user);
            _cntx.SaveChanges();

            await Authenticate(model.UserName); // аутентификация
            return Redirect("~/");
        }
        private async Task Authenticate(string userName)
        {
            var user = _cntx.AppUsers.FirstOrDefault(x => x.UserName == userName);
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userName),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.UserRole.ToString())
            };

            _session.Set("CurrentUser", new CurrentUser
            {
                UserId = user.Id,
                Role = user.UserRole,
                StoreId = user.StoreId,
                CityId = user.CityId,
                CompanyId = user.CompanyId
            });

            // создаем объект ClaimsIdentity
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

            // установка аутентификационных куки
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        [HttpGet]
        [Authorize]
        public IActionResult UserProfile()
        {
            // получаем имя пользователя
            var userName = User.Identity.Name;
            // Получаем пользователя
            var user = _cntx.AppUsers.First(x => x.UserName == userName);

            return View(new UserProfileVM(user));
        }
        [HttpPost]
        [Authorize]
        public IActionResult UserProfile(UserProfileVM model)
        {
            // Проверяем модель на валидность
            if (!ModelState.IsValid) return View(model);
            //  Проверяем соответствие пароля
            if (!string.IsNullOrWhiteSpace(model.Password))
            {
                if (!model.Password.Equals(model.ConfirmPassword))
                {
                    ModelState.AddModelError("", _resources["PasswordDoNotMatch"]);
                    return View(model);
                }
            }

            // Получить экземпляр UserDTO
            var dto = _cntx.AppUsers.Find(model.Id);
            if (!dto.UserName.Equals(model.UserName))
            {
                // Проверяем имя на уникальность
                if (_cntx.AppUsers.Any(x => x.UserName == model.UserName))
                {
                    ModelState.AddModelError("", string.Format(_resources["LoginIsTaken"], model.UserName));
                    model.UserName = string.Empty;
                    return View(model);
                }

            }

            dto.FirstName = model.FirstName;
            dto.LastName = model.LastName;
            dto.EmailAddress = model.EmailAddress;
            dto.UserName = model.UserName;
            dto.Password = model.Password;

            // Сохранить данные
            _cntx.AppUsers.Update(dto);
            _cntx.SaveChanges();

            // Записать сообщение
            TempData["SM"] = _resources["YourProfileEdited"].Value;

            if (User.Identity.Name != model.UserName)
            {
                return RedirectToAction("Logout");
            }
            return View(model);
        }
    }
}
