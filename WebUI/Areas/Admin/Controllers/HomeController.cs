using Data.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System;
using System.IO;
using System.Linq;
using WebUI.Extensions;

namespace WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,SuperUser")]
    public class HomeController : Controller
    {
        private ApplicationContext _db;
        private IWebHostEnvironment _env;
        private IStringLocalizer _resources;

        public HomeController(ApplicationContext cntx, IWebHostEnvironment env, IStringLocalizerFactory localizer)
        {
            _db = cntx;
            _env = env;
            _resources = localizer.GetLocalResources();
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
            ViewBag.Title = _resources["Administration"];
            ViewBag.TabItem = "Home";
            return View();
        }
    }
}
