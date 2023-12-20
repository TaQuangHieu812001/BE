using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FunitureApp.Areas.admin.Controllers
{
    [Area("admin")]
    public class AdminController : Controller
    {
        const string userAccount = "admin";
        const string userPass = "123456";
        private readonly IHttpContextAccessor _contextAccessor;
        public AdminController(IHttpContextAccessor httpContext) {
            _contextAccessor = httpContext;


        }

        public IActionResult Login()
        {

            return View("~/Areas/admin/Views/Login.cshtml");
        }
        [HttpPost]
        public IActionResult Login(string username,string password) {
            if(username!=userAccount&& password != userPass)
            {
                TempData["msg"] = "Tài khoản hoặc mật khẩu không đúng";
                return View("~/Areas/admin/Views/Login.cshtml");
            }
            _contextAccessor.HttpContext.Session.SetString("admin","admin");
            return Redirect("/admin/Products");
        }

    }
}