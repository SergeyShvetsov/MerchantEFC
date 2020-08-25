using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Data.Model;
using Data.Model.Extensions;
using Data.Model.Interfaces;
using Data.Model.Models;
using Data.Tools.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using WebUI.Areas.Admin.Models;
using WebUI.Extensions;

namespace WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Supervisor")]
    public class CitiesController : Controller
    {
        private readonly AppConfig _config;
        private readonly ApplicationContext _cntx;
        private readonly IStringLocalizer _resources;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;
        private readonly IQueryable<City> _availableCities;

        public CitiesController(IOptions<AppConfig> config, ApplicationContext context, IStringLocalizerFactory localizer, IHttpContextAccessor httpContextAccessor)
        {
            // _cntx = cntx;
            _config = config.Value;
            _cntx = context;
            _resources = localizer.GetLocalResources();
            _httpContextAccessor = httpContextAccessor;
            _availableCities = _cntx.Cities.ApplySecurityFilter(_session);
        }

        public ActionResult List(int? page)
        {
            ViewBag.TabItem = "Cities";

            var pageNumber = page ?? 1;
            int pageSize = _config.Admin_RowsPerPage;

            var tmp = _availableCities.AsNoTracking().OrderBy(o => o.Name_ru);
            return View(tmp.ToPagedList(pageNumber, pageSize));
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
            if (_cntx.Cities.Any(x => x.Code == code && !x.IsArchived))
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
            _cntx.Cities.Add(city);
            _cntx.SaveChanges();

            TempData["SM"] = _resources["NewCityAdded"].Value;
            return RedirectToAction("List");
        }

        [HttpGet]
        public IActionResult DeleteCity(int id)
        {
            var city = _cntx.Cities.Find(id);
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
            var city = _cntx.Cities.Find(model.Id);
            city.Archive(_cntx);
            _cntx.SaveChanges();

            TempData["SM"] = _resources["CityWasDeleted"].Value;
            return RedirectToAction("List");
        }

        [HttpGet]
        public ActionResult EditCity(int id)
        {
            ViewBag.TabItem = "Cities";
            var city = _cntx.Cities.Find(id);
            var model = new CityListVM(city);
            return PartialView("_EditCityModal", model);
        }

        [HttpPost]
        public ActionResult EditCity(CityListVM model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_EditCityModal", model);
            }
            var city = _cntx.Cities.Find(model.Id);
            var code = model.Code.NormalizeCode();
            if (city.Code != code && _cntx.Cities.Any(x => x.Code == code))
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
            _cntx.SaveChanges();

            TempData["SM"] = _resources["CityEdited"].Value;
            return RedirectToAction("List");
        }
    }
}
