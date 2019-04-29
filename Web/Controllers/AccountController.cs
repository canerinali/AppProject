using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BLL.Service;
using DAL.Dto;

namespace Web.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginDto model)
        {
            var service = new CommonService();

            var result = service.Login(model);

            if (result!=null)
            {
                Session.Add("Login",result);
                return RedirectToAction("Index","Home");
            }
            else
            {
                ViewBag.message = "Bilgiler Hatalı";
                return View();
            }

            
        }

        public ActionResult Register()
        {
           
                return View(new LoginDto());
            
        }
        [HttpPost]
        public ActionResult Register(LoginDto model)
        {
            var service = new CommonService();
            var result = service.InsertLogin(model);
            if (result!=null)
            {
                Session.Add("Login", result);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.message = "Bilgiler Hatalı";
                return View();
            }
        }

        public ActionResult Logout()
        {
            Session.Remove("Login");
            return RedirectToAction("Login", "Account");
        }
    }
}
