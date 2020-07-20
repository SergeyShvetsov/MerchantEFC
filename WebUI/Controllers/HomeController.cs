using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Data.Model;
using Data.Model.Extensions;
using Data.Model.Interfaces;
using Data.Model.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using WebUI.Extensions;
using WebUI.Resources;

namespace WebUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly IEntityContext _cntx;
        private readonly IStringLocalizer _resources;
        public HomeController(IEntityContext context, IStringLocalizerFactory localizer)
        {
            _cntx = context;
            _resources = localizer.GetLocalResources();
        }
        [HttpPost]
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
            var roles = _cntx.Roles.GetAll();
            var users = _cntx.Users.GetAll().ApplyArchivedFilter();

            if (!roles.Any())
            {
                var adminRole = new Role() { Name = "Admin", DisplayName = "Administrator", RoleType = RoleType.Admin };
                var superUserRole = new Role() { Name = "SU", DisplayName = "Super User", RoleType = RoleType.Superuser };
                var managerRole = new Role() { Name = "Mgr", DisplayName = "Manager", RoleType = RoleType.Manager };
                var userRole = new Role() { Name = "Usr", DisplayName = "User", RoleType = RoleType.User };
                _cntx.Roles.Insert(adminRole);
                _cntx.Roles.Insert(superUserRole);
                _cntx.Roles.Insert(managerRole);
                _cntx.Roles.Insert(userRole);

                var admin = new AppUser()
                {
                    FirstName = "Admin",
                    LastName = "Administrator",
                    EmailAddress = "admin@fake.com",
                    UserName = "admin",
                    Password = "admin"
                };
                var store = new Store { StoreCode = "test", StoreName = "Test Store" };
                var store1 = new Store { StoreCode = "test1", StoreName = "Test Store 1" };
                var store2 = new Store { StoreCode = "test2", StoreName = "Test Store 2" };

                _cntx.Users.Insert(admin);
                _cntx.Stores.Insert(store);
                _cntx.Stores.Insert(store1);
                _cntx.Stores.Insert(store2);
                _cntx.Save();

                var role = _cntx.Roles.GetById(adminRole.RoleId);
                _cntx.Context.AddRoleToUser(admin, role);
                _cntx.Users.AssignToStore(admin, store);

                _cntx.Save();
            }
            var aaa = CultureInfo.GetCultures(CultureTypes.AllCultures).Where(x => x.Name.StartsWith("uz"));
            //return $"CurrentCulture:{CultureInfo.CurrentCulture.Name}, CurrentUICulture:{CultureInfo.CurrentUICulture.Name}";

            ViewBag.Title = "Home";
            return View();
        }
    }
}
