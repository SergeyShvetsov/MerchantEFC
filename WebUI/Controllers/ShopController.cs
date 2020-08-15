using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Data.Model;
using Data.Model.Extensions;
using Data.Model.Interfaces;
using Data.Model.Models;
using Data.Tools.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using WebUI.Extensions;
using WebUI.Models;
using WebUI.Resources;
using WebUI.Services;
using X.PagedList;

namespace WebUI.Controllers
{
    public class ShopController : Controller
    {
        private readonly ApplicationContext _cntx;
        private readonly IStringLocalizer _resources;
        private readonly ICatalogService _catalog;
        private readonly AppConfig _config;
        private readonly IEnumerable<Category> _categories;

        public ShopController(ApplicationContext context, IStringLocalizerFactory localizer, ICatalogService catalog, IWebHostEnvironment _env, IOptions<AppConfig> config)
        {
            _cntx = context;
            _resources = localizer.GetLocalResources();

            _categories = catalog.Categories;
            _catalog = catalog;

            _config = config.Value;
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

            if (!_cntx.AppUsers.Any())
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

                _cntx.AppUsers.Add(admin);
                _cntx.SaveChanges();
            }

            ViewBag.Title = "Home";
            return View();
        }
        public IActionResult Category(string cat, IFormCollection form)
        {
            var c = cat == null ? (string)TempData["cat"] : cat;

            var model = _categories.First(x => x.Code == c);
            return View(model);
        }

        public IActionResult Catalog(int? page, string c)
        {
            var pageNumber = page ?? 1;
            int pageSize = _config.Admin_RowsPerPage;
            var cats = _catalog.GetProductCategories().Select(s => s.Code);
            if (!c.IsNullOrEmpty())
            {
                cats = cats.Where(x => x.StartsWith(c));
            }

            var listOfProducts = _cntx.Products
                                 .Select(s => new ProductCardVM(s));

            var onePageOfStores = listOfProducts.ToPagedList(pageNumber, pageSize: pageSize);

            ViewBag.Title = "Catalog";
            return View(onePageOfStores);
        }
    }
}
