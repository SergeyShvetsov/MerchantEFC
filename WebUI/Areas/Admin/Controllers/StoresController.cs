using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
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
using X.PagedList;

namespace WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,SuperUser")]
    public class StoresController : Controller
    {
        private readonly AppConfig _config;
        private readonly IEntityContext _cntx;
        private readonly IStringLocalizer _resources;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;
        private readonly IEnumerable<Store> _availableStores;
        private readonly IEnumerable<City> _availableCities;

        public StoresController(IOptions<AppConfig> config, IEntityContext context, IStringLocalizerFactory localizer, IHttpContextAccessor httpContextAccessor)
        {
            _config = config.Value;
            _cntx = context;
            _resources = localizer.GetLocalResources();
            _httpContextAccessor = httpContextAccessor;
            _availableCities = _cntx.GetAvailableCities(_session);
            _availableStores = _cntx.GetAvailableStores(_session);
        }

        public IActionResult List(int? page)
        {
            ViewBag.TabItem = "Stores";

            // Устанавливаем номер страницы
            var pageNumber = page ?? 1;
            int pageSize = _config.Admin_RowsPerPage;

            var listOfStores = _cntx.Stores.GetAll()
                .ApplyArchivedFilter()
                .ApplyAvailableFilter(_availableStores);

            var listOfStoresVM = listOfStores
                .Select(s => new StoreListVM(s))
                .OrderBy(o => o.StoreName)
                .ToList();

            // Устанавливаем постраничную навигацию
            var onePageOfStores = listOfStoresVM.ToPagedList(pageNumber, pageSize: pageSize);

            // Возвращаем в преставление
            return View(onePageOfStores);
        }

        [HttpGet]
        public ActionResult CreateStore()
        {
            ViewBag.TabItem = "Stores";
            ViewBag.AvailableCities = _availableCities;
            return PartialView("_CreateStoreModal", new StoreEditVM());
        }
        [HttpPost]
        public ActionResult CreateStore(StoreEditVM model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.AvailableCities = _availableCities;
                return PartialView("_CreateStoreModal", model);
            }
            var code = model.Code.NormalizeCode();
            if (!_cntx.Stores.IsUniqCode(code))
            {
                model.Code = code;
                ViewBag.AvailableCities = _availableCities;
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
                CityId = model.CityId
            };
            _cntx.Stores.Insert(store);
            _cntx.Save();

            TempData["SM"] = _resources["NewStoreAdded"].Value;
            return RedirectToAction("List");
        }

        [HttpGet]
        public IActionResult DeleteStore(int id)
        {
            var store = _cntx.Stores.GetById(id);
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
            var store = _cntx.Stores.GetById(model.Id);
            _cntx.Stores.Delete(store);
            _cntx.Save();

            TempData["SM"] = _resources["StoreWasDeleted"].Value;
            return RedirectToAction("List");
        }

        [HttpGet]
        public ActionResult EditStore(int id)
        {
            ViewBag.TabItem = "Stores";
            ViewBag.AvailableCities = _availableCities;
            var store = _cntx.Stores.GetById(id);
            var model = new StoreEditVM(store);
            return PartialView("_EditStoreModal", model);
        }
        [HttpPost]
        public ActionResult EditStore(StoreEditVM model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.AvailableCities = _availableCities;
                return PartialView("_EditStoreModal", model);
            }
            var store = _cntx.Stores.GetById(model.Id);
            var code = model.Code.NormalizeCode();
            if (store.Code != code && !_cntx.Stores.IsUniqCode(code))
            {
                model.Code = code;
                ViewBag.AvailableCities = _availableCities;
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

            _cntx.Stores.Update(store);
            _cntx.Save();

            TempData["SM"] = _resources["StoreEdited"].Value;
            return RedirectToAction("List");
        }

    }
}
