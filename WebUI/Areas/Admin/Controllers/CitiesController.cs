using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Data.Model.Extensions;
using Data.Model.Interfaces;
using Data.Model.Models;
using Data.Tools.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
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
    public class CitiesController : Controller
    {
        private readonly AppConfig _config;
        private readonly IEntityContext _cntx;
        private readonly IStringLocalizer _resources;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;
        private readonly IEnumerable<City> _availableCities;

        public CitiesController(IOptions<AppConfig> config, IEntityContext context, IStringLocalizerFactory localizer, IHttpContextAccessor httpContextAccessor)
        {
            // _cntx = cntx;
            _config = config.Value;
            _cntx = context;
            _resources = localizer.GetLocalResources();
            _httpContextAccessor = httpContextAccessor;
            _availableCities = _cntx.GetAvailableCities(_session);
        }

        public ActionResult List(int? page)
        {
            ViewBag.TabItem = "Cities";

            // Устанавливаем номер страницы
            var pageNumber = page ?? 1;
            int pageSize = _config.Admin_RowsPerPage;

            var listOfCities = _cntx.Cities.GetAll()
                .ApplyArchivedFilter()
                .ApplyAvailableFilter(_availableCities);

            var listOfCitiesVM = listOfCities
                .Select(s => new CityListVM(s))
                .OrderBy(o => o.Name_ru)
                .ToList();

            // Устанавливаем постраничную навигацию
            var onePageOfCities = listOfCitiesVM.ToPagedList(pageNumber, pageSize: pageSize);

            // Возвращаем в преставление
            return View(onePageOfCities);
        }

        [HttpGet]
        public ActionResult CreateCity()
        {
            ViewBag.TabItem = "Cities";
            return PartialView("_CreateCityModal", new CityListVM());
        }

        [HttpPost]
        public ActionResult CreateCity(CityListVM model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_CreateCityModal", model);
            }
            var code = model.Code.NormalizeCode();
            if (!_cntx.Cities.IsUniqCode(code))
            {
                model.Code = code;
                ModelState.AddModelError("", string.Format(_resources["CodeIsTaken"], code));
                return PartialView("_CreateCityModal", model);
            }
            var city = new City()
            {
                Code = code,
                Name_ru = model.Name_ru,
                Name_uz_c = model.Name_uz_c,
                Name_uz_l = model.Name_uz_l
            };
            _cntx.Cities.Insert(city);
            _cntx.Save();

            TempData["SM"] = _resources["NewCityAdded"].Value;
            return RedirectToAction("List");
        }

        [HttpGet]
        public IActionResult DeleteCity(int id)
        {
            var city = _cntx.Cities.GetById(id);
            var model = new CityDeleteVM
            {
                Id = id,
                CityName = city.Name
            };

            return PartialView("_DeleteCityModal", model);
        }
        [HttpPost]
        public IActionResult DeleteCity(CityDeleteVM model)
        {
            var city = _cntx.Cities.GetById(model.Id);
            _cntx.Cities.Delete(city);
            _cntx.Save();

            TempData["SM"] = _resources["CityWasDeleted"].Value;
            return RedirectToAction("List");
        }

        [HttpGet]
        public ActionResult EditCity(int id)
        {
            ViewBag.TabItem = "Cities";
            var city = _cntx.Cities.GetById(id);
            var model = new CityListVM(city);
            return PartialView("_EditCityModal",model);
        }

        [HttpPost]
        public ActionResult EditCity(CityListVM model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_EditCityModal", model);
            }
            var city = _cntx.Cities.GetById(model.Id);
            var code = model.Code.NormalizeCode();
            if (city.Code != code && !_cntx.Cities.IsUniqCode(code))
            {
                model.Code = code;
                ModelState.AddModelError("", string.Format(_resources["CodeIsTaken"], code));
                return PartialView("_EditCityModal", model);
            }

            city.Code = code;
            city.Name_ru = model.Name_ru;
            city.Name_uz_c = model.Name_uz_c;
            city.Name_uz_l = model.Name_uz_l;

            _cntx.Cities.Update(city);
            _cntx.Save();

            TempData["SM"] = _resources["CityEdited"].Value;
            return RedirectToAction("List");
        }
    }
}
