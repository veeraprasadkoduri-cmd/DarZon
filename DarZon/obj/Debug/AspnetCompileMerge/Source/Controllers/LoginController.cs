using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DarZon.Models;
using DarZon.DAL;
using System.Web.Security;

namespace DarZon.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        DARZANTESTEntities db = new DARZANTESTEntities();
        public ActionResult Login()
        {
            return View();
        }

        public JsonResult LoginSubmit(OHEM LoginUser)
        {

            OHEM objtbl = new OHEM();
            objtbl = LoginUser;

            Repository obj = new Repository();
          
            if (Session["UserName"] == null)
            {
                var users = obj.GetUserDetails(LoginUser);
                if (users != null)
                {
                    FormsAuthentication.SetAuthCookie(users.U_EMPCODE.ToString(), false);
                    Session["Usercode"] = users.U_EMPCODE.ToString();
                    Session["UserName"] = users.U_EMPCODE;
                    Session["WhareHouse"] = users.U_WareH;
                    var authTicket = new FormsAuthenticationTicket(1, users.U_EMPCODE.ToString(), DateTime.Now, DateTime.Now.AddMinutes(20), false, "All");
                    string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
                    var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                    HttpContext.Response.Cookies.Add(authCookie);


                    return Json(new { value = "Success" }, JsonRequestBehavior.AllowGet);
                }

                else
                {
                    // ModelState.AddModelError("", "Invalid login attempt.");
                    return Json(new { value = "Fail" }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { value = "Success" }, JsonRequestBehavior.AllowGet);

            }

        }

    }
}