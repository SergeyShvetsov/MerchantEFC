using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Model.Extensions;
using Data.Model.Interfaces;
using Data.Model.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using WebUI.Areas.Admin.Models;
using WebUI.Extensions;
using X.PagedList;

namespace WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,SuperUser,Manager")]
    public class ProductsController : Controller
    {
        private readonly AppConfig _config;
        private readonly IEntityContext _cntx;
        private readonly IStringLocalizer _resources;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;
        private readonly IEnumerable<Store> _availableStores;
        public ProductsController(IOptions<AppConfig> config, IEntityContext context, IStringLocalizerFactory localizer, IHttpContextAccessor httpContextAccessor)
        {
            _config = config.Value;
            _cntx = context;
            _resources = localizer.GetLocalResources();
            _httpContextAccessor = httpContextAccessor;
            _availableStores = _cntx.GetAvailableStores(_session);
        }
        public IActionResult List(int? page)
        {
            ViewBag.TabItem = "Products";

            // Устанавливаем номер страницы
            var pageNumber = page ?? 1;
            int pageSize = _config.Admin_RowsPerPage;

            var listOfStores = _cntx.Products.GetAll()
                .ApplyArchivedFilter()
                .ApplyAvailableFilter(_availableStores);

            var listOfStoresVM = listOfStores
                .Select(s => new ProductListVM(s))
                .OrderBy(o => o.Name)
                .ToList();

            // Устанавливаем постраничную навигацию
            var onePageOfStores = listOfStoresVM.ToPagedList(pageNumber, pageSize: pageSize);

            // Возвращаем в преставление
            return View(onePageOfStores);
        }
    }
}
