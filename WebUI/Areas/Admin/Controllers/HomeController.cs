using Data.Model;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;

namespace WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        private ApplicationContext _db;
        private IWebHostEnvironment _env;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;

        public HomeController(ApplicationContext cntx, IWebHostEnvironment env, IHttpContextAccessor httpContextAccessor)
        {
            _db = cntx;
            _env = env;
            _httpContextAccessor = httpContextAccessor;
        }
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
            // Samples
            //var currentUser = _db.Users.FirstOrDefault(x => x.UserName == User.Identity.Name);
            //var userRoles = (from role in _db.Roles
            //                join ur in _db.UserRoles.Where(x => x.UserId == currentUser.UserId) on role.RoleId equals ur.RoleId
            //                select role).ToList();

            //var imagesDir = Path.Combine(_env.WebRootPath, "Images");

            //Emailer.SendMail(from: currentUser.EmailAddress, recipients: "user1@any.fake,user2@any.fake", subject: "Test", htmlText: "Test Message");
            return View();
        }
    }
}
