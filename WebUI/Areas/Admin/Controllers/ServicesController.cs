using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Model;
using Data.Model.Extensions;
using Data.Model.Lucene;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using WebUI.Extensions;
using WebUI.Services;

namespace WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ServicesController : Controller
    {
        private readonly ApplicationContext _cntx;
        private readonly IStringLocalizer _resources;
        private readonly ICatalogService _catalog;
        private readonly AppConfig _config;
        private readonly IWebHostEnvironment _env;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;

        private readonly IEnumerable<Category> _categories;

        public ServicesController(ApplicationContext context,
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

        public IActionResult Index()
        {
            ViewBag.TabItem = "Services";
            return View();
        }

        public IActionResult RebuildSearchIndex()
        {
            var Indexer = new ProductIndex(_env.WebRootPath);
            var cnt = Indexer.Build(_cntx.GetAllCatalogItems(new CatalogFilters()));
            Indexer.Dispose();

            TempData["SM"] = $"Создано {cnt} индексов.";
            return View("Index");
        }
    }
}
