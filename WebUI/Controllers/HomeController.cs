using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Data.Model;
using Data.Model.Extensions;
using Data.Model.Interfaces;
using Data.Model.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using WebUI.Extensions;
using WebUI.Resources;
using WebUI.Services;

namespace WebUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly IEntityContext _cntx;
        private readonly IStringLocalizer _resources;
        private readonly List<Category> _catalog;
        public HomeController(IEntityContext context, IStringLocalizerFactory localizer, ICatalogService catalog, IWebHostEnvironment _env)
        {
            _cntx = context;
            _resources = localizer.GetLocalResources();
            _catalog = catalog.GetCategories(_env.WebRootPath).ToList();
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
            var users = _cntx.Users.GetAll().ApplyArchivedFilter();

            if (!users.Any())
            {
                var admin = new AppUser()
                {
                    FirstName = "Admin",
                    LastName = "Administrator",
                    EmailAddress = "admin@fake.com",
                    UserName = "admin",
                    Password = "admin",
                    UserRole = RoleType.Admin
                };
                var store = new Store { StoreCode = "test", StoreName = "Test Store" };
                var store1 = new Store { StoreCode = "test1", StoreName = "Test Store 1" };
                var store2 = new Store { StoreCode = "test2", StoreName = "Test Store 2" };

                _cntx.Users.Insert(admin);
                _cntx.Stores.Insert(store);
                _cntx.Stores.Insert(store1);
                _cntx.Stores.Insert(store2);
                _cntx.Save();

                _cntx.Users.AssignToStore(admin, store);

                _cntx.Save();
            }

            ViewBag.Title = "Home";
            return View();
        }
        public IActionResult Category(string cat, IFormCollection form)
        {
            var c = cat == null ? (string)TempData["cat"] : cat;

            var model = _catalog.First(x => x.Code == c);
            return View(model);
        }
    }
}
