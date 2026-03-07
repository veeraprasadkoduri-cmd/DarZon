using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace DarZon.Controllers
{
    public class HomeController : Controller
    {

    
        // GET: Home
        public ActionResult Home()
        {
            return View();
        }
        public ActionResult Logout()
        {
            Session.Abandon();

            return RedirectToAction("Login", "Login");
        }
    }
}