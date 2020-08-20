using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Data.Model;
using Data.Model.Extensions;
using Data.Model.Interfaces;
using Data.Model.Models;
using Data.Tools.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using WebUI.Areas.Admin.Models;
using WebUI.Extensions;

namespace WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,SuperUser")]
    public class StoresController : Controller
    {
        private readonly AppConfig _config;
        private readonly ApplicationContext _cntx;
        private readonly IStringLocalizer _resources;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;
        private readonly IQueryable<Store> _availableStores;
        private readonly IQueryable<City> _availableCities;
        private readonly IQueryable<Company> _availableCompanies;

        public StoresController(IOptions<AppConfig> config, ApplicationContext context, IStringLocalizerFactory localizer, IHttpContextAccessor httpContextAccessor)
        {
            _config = config.Value;
            _cntx = context;
            _resources = localizer.GetLocalResources();
            _httpContextAccessor = httpContextAccessor;
            _availableCities = _cntx.Cities.ApplySecurityFilter(_session);
            _availableCompanies = _cntx.Companies.ApplySecurityFilter(_session);
            _availableStores = _cntx.Stores.ApplySecurityFilter(_session);
        }

        public IActionResult List(int? page)
        {
            ViewBag.TabItem = "Stores";

            // Устанавливаем номер страницы
            var pageNumber = page ?? 1;
            int pageSize = _config.Admin_RowsPerPage;

            var tmp = _availableStores.OrderBy(o => o.Name);
            return View(tmp.ToPagedList(pageNumber, pageSize));
        }

        [HttpGet]
        public ActionResult CreateStore()
        {
            ViewBag.TabItem = "Stores";
            ViewBag.AvailableCities = _availableCities.ToList();
            ViewBag.AvailableCompanies = _availableCompanies.ToList();
            return PartialView("_CreateStoreModal", new StoreEditVM());
        }
        [HttpPost]
        public ActionResult CreateStore(StoreEditVM model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.AvailableCities = _availableCities.ToList();
                ViewBag.AvailableCompanies = _availableCompanies.ToList();
                return PartialView("_CreateStoreModal", model);
            }
            var code = model.Code.NormalizeCode();
            if (_cntx.Stores.Any(x => x.Code == code && !x.IsArchived))
            {
                model.Code = code;
                ViewBag.AvailableCities = _availableCities.ToList();
                ViewBag.AvailableCompanies = _availableCompanies.ToList();
                ModelState.AddModelError("", string.Format(_resources["CodeIsTaken"], code));
                return PartialView("_CreateStoreModal", model);
            }

            var store = new Store()
            {
                Code = code,
                Name = model.Name,
                EmailAddress = model.Email,
                Phone = model.Phone,
                TIN = model.TIN,
                IsActive = model.IsActive,
                IsBlocked = model.IsBlocked,
                CityId = model.CityId,
                CompanyId = model.CompanyId == 0 ? null : model.CompanyId
            };
            _cntx.Stores.Update(store);
            _cntx.SaveChanges();

            TempData["SM"] = _resources["NewStoreAdded"].Value;
            return RedirectToAction("List");
        }

        [HttpGet]
        public IActionResult DeleteStore(int id)
        {
            var store = _cntx.Stores.Find(id);
            var model = new StoreDeleteVM
            {
                Id = id,
                StoreName = store.Name
            };

            return PartialView("_DeleteStoreModal", model);
        }
        [HttpPost]
        public IActionResult DeleteStore(StoreDeleteVM model)
        {
            var store = _cntx.Stores.Find(model.Id);
            store.Archive(_cntx);
            _cntx.SaveChanges();

            TempData["SM"] = _resources["StoreWasDeleted"].Value;
            return RedirectToAction("List");
        }

        [HttpGet]
        public ActionResult EditStore(int id)
        {
            ViewBag.TabItem = "Stores";
            ViewBag.AvailableCities = _availableCities.ToList();
            ViewBag.AvailableCompanies = _availableCompanies.ToList();
            var store = _cntx.Stores.Find(id);
            var model = new StoreEditVM(store);
            return PartialView("_EditStoreModal", model);
        }
        [HttpPost]
        public ActionResult EditStore(StoreEditVM model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.AvailableCities = _availableCities.ToList();
                ViewBag.AvailableCompanies = _availableCompanies.ToList();
                return PartialView("_EditStoreModal", model);
            }
            var store = _cntx.Stores.Find(model.Id);
            var code = model.Code.NormalizeCode();
            if (store.Code != code && _cntx.Stores.Any(x => x.Code == code && !x.IsArchived))
            {
                model.Code = code;
                ViewBag.AvailableCities = _availableCities.ToList();
                ViewBag.AvailableCompanies = _availableCompanies.ToList();
                ModelState.AddModelError("", string.Format(_resources["CodeIsTaken"], code));
                return PartialView("_EditStoreModal", model);
            }

            store.Code = code;
            store.Name = model.Name;
            store.EmailAddress = model.Email;
            store.Phone = model.Phone;
            store.TIN = model.TIN;
            store.IsActive = model.IsActive;
            store.IsBlocked = model.IsBlocked;
            store.CityId = model.CityId;
            store.CompanyId = model.CompanyId == 0 ? null : model.CompanyId;
            _cntx.Stores.Update(store);
            _cntx.SaveChanges();

            TempData["SM"] = _resources["StoreEdited"].Value;
            return RedirectToAction("List");
        }

    }
}
