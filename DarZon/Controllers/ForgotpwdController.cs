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
    public class ForgotpwdController : Controller
    {
        // GET: Forgotpwd
  
        DARZANTESTEntities db = new DARZANTESTEntities();
 
        public ActionResult Index()
        {
            return View();
        }
   
        public JsonResult checkvalemail(string emailId)
        {
            string message;
            try
            {
               
                    var result = db.OHEMs.Where(a => a.email == emailId).FirstOrDefault();
                
                if (result != null)
                    {
                    Session["email"] = result.email.ToString();
                        // result.Password = newpwd;
                        // db.SaveChanges();



                    message = "Success";
                    }
                    else
                    {
                        message = "Fail";
                    }
               

                return Json(new { value = message}, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { value = "fail" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}