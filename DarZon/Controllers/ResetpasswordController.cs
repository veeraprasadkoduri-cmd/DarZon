using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DarZon.DAL;
using System.Web.Security;
using DarZon.Models;

namespace DarZon.Controllers
{
    public class ResetpasswordController : Controller
    {

        DARZANTESTEntities db = new DARZANTESTEntities();
        // GET: Resetpassword
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult Resetpwd(string newpwd)
        {
            string message;
            try
            {
                string emailId = Session["email"].ToString();
                if (emailId != null)
                {
                    var result = (from user in db.OHEMs where (user.email == emailId) select user).FirstOrDefault();

                    if (result != null)
                    {
                       
                        result.U_Password = newpwd;
                        db.SaveChanges();
                        

                        message = "Success";
                    }
                    else
                    {
                        message = "Fail";
                    }
                }
                else
                {
                    message = "Fail";
                }

                return Json(new { value = message }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { value = "fail" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}