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
    public class CompaniesController : Controller
    {
        private readonly AppConfig _config;
        private readonly ApplicationContext _cntx;
        private readonly IStringLocalizer _resources;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;
        private readonly IQueryable<Company> _availableCompanies;

        public CompaniesController(IOptions<AppConfig> config, ApplicationContext context, IStringLocalizerFactory localizer, IHttpContextAccessor httpContextAccessor)
        {
            // _cntx = cntx;
            _config = config.Value;
            _cntx = context;
            _resources = localizer.GetLocalResources();
            _httpContextAccessor = httpContextAccessor;
            _availableCompanies = _cntx.Companies.ApplySecurityFilter(_session);
        }

        public ActionResult List(int? page)
        {
            ViewBag.TabItem = "Companies";

            var pageNumber = page ?? 1;
            int pageSize = _config.Admin_RowsPerPage;

            var tmp = _availableCompanies.AsNoTracking().OrderBy(o => o.Name);
            return View(tmp.ToPagedList(pageNumber, pageSize));
        }

        [HttpGet]
        public ActionResult CreateCompany()
        {
            ViewBag.TabItem = "Companies";
            return PartialView("_CreateCompanyModal", new CompanyListVM());
        }

        [HttpPost]
        public ActionResult CreateCompany(CompanyListVM model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_CreateCompanyModal", model);
            }
            var code = model.Code.NormalizeCode();
            if (_cntx.Companies.Any(x => x.Code == code && !x.IsArchived))
            {
                model.Code = code;
                ModelState.AddModelError("", string.Format(_resources["CodeIsTaken"], code));
                return PartialView("_CreateCompanyModal", model);
            }
            var company = new Company()
            {
                Code = code,
                Name = model.Name,
                EmailAddress = model.Email,
                Phone = model.Phone,
                IsActive = model.IsActive,
                IsBlocked = model.IsBlocked
            };
            _cntx.Companies.Add(company);
            _cntx.SaveChanges();

            TempData["SM"] = _resources["NewCompanyAdded"].Value;
            return RedirectToAction("List");
        }

        [HttpGet]
        public IActionResult DeleteCompany(int id)
        {
            var company = _cntx.Companies.Find(id);
            var model = new CompanyDeleteVM
            {
                Id = id,
                CompanyName = company.Name
            };

            return PartialView("_DeleteCompanyModal", model);
        }
        [HttpPost]
        public IActionResult DeleteCompany(CityDeleteVM model)
        {
            var company = _cntx.Companies.Find(model.Id);
            company.Archive(_cntx);
            _cntx.SaveChanges();

            TempData["SM"] = _resources["CompanyWasDeleted"].Value;
            return RedirectToAction("List");
        }

        [HttpGet]
        public ActionResult EditCompany(int id)
        {
            ViewBag.TabItem = "Companies";
            var company = _cntx.Companies.Find(id);
            var model = new CompanyListVM(company);
            return PartialView("_EditCompanyModal", model);
        }

        [HttpPost]
        public ActionResult EditCompany(CompanyListVM model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_EditCompanyModal", model);
            }
            var company = _cntx.Companies.Find(model.Id);
            var code = model.Code.NormalizeCode();
            if (company.Code != code && _cntx.Companies.Any(x => x.Code == code))
            {
                model.Code = code;
                ModelState.AddModelError("", string.Format(_resources["CodeIsTaken"], code));
                return PartialView("_EditCityModal", model);
            }

            company.Code = code;
            company.Name = model.Name;
            company.EmailAddress = model.Email;
            company.Phone = model.Phone;
            company.IsActive = model.IsActive;
            company.IsBlocked = model.IsBlocked;

            _cntx.Companies.Update(company);
            _cntx.SaveChanges();

            TempData["SM"] = _resources["CompanyEdited"].Value;
            return RedirectToAction("List");
        }
    }
}
