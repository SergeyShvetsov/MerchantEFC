using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Data.Model;
using Data.Model.Extensions;
using Data.Model.Interfaces;
using Data.Model.Models;
using Data.Tools.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        private readonly IWebHostEnvironment _env;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;

        private readonly IEnumerable<Category> _categories;

        public ShopController(ApplicationContext context,
            IStringLocalizerFactory localizer,
            ICatalogService catalog,
            IWebHostEnvironment env,
            IHttpContextAccessor httpContextAccessor,
            IOptions<AppConfig> config)
        {
            _cntx = context;
            _resources = localizer.GetLocalResources();
            _env = env;
            _httpContextAccessor = httpContextAccessor;
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

        public IActionResult Catalog(int? page)
        {
            var pageNumber = page ?? 1;
            int pageSize = _config.Admin_RowsPerPage;

            var filters = _session.Get<CatalogFilters>("Filters");
            if (filters == null)
            {
                filters = new CatalogFilters();
                _session.Set<CatalogFilters>("Filters", filters);
            }

            var items = _cntx.GetAllCatalogItems(filters.Category);

            switch (filters.OrderBy)
            {
                case OrderBy.PriceDesc:
                    items = items.OrderByDescending(o => o.SalesPrice ?? o.Price);
                    break;
                default:
                    items = items.OrderByDescending(o => o.Votes == 0 ? 0 : (double)o.Points / o.Votes);
                    break;
            }

            var onePageOfStores = items.ToPagedList(pageNumber, pageSize: pageSize);

            ViewBag.Title = "Catalog";
            return View(onePageOfStores);
        }
        public IActionResult SetCategory(string c)
        {
            var filters = _session.Get<CatalogFilters>("Filters");
            if (filters == null) filters = new CatalogFilters();
            filters.Category = c;
            _session.Set("Filters", filters);

            return RedirectToAction("Catalog");
        }
        public IActionResult ProductImage(int? id, ImageSize s = ImageSize.Medium)
        {
            byte[] buffer;

            var pr = _cntx.SiteImages.AsNoTracking().FirstOrDefault(x => x.ObjectId == id && x.ImageType == ImageType.ProductImage);
            if (pr == null)
            {
                var uplDir = Path.Combine(_env.WebRootPath, "Images\\no_image.png");
                buffer = System.IO.File.ReadAllBytes(uplDir);
            }
            else
            {
                buffer = pr.ObjImage;
            }

            var img = buffer.GetImage().Scale((int)s).ToArray();
            return File(img, "image/png");
        }
        public IActionResult SiteImage(int? id, ImageSize s = ImageSize.Medium)
        {
            byte[] buffer;

            var pr = _cntx.SiteImages.AsNoTracking().FirstOrDefault(x => x.Id == id);
            if (pr == null)
            {
                var uplDir = Path.Combine(_env.WebRootPath, "Images\\no_image.png");
                buffer = System.IO.File.ReadAllBytes(uplDir);
            }
            else
            {
                buffer = pr.ObjImage;
            }

            var img = buffer.GetImage().Scale((int)s).ToArray();
            return File(img, "image/png");
        }
    }
}
