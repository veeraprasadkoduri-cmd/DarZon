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
    public class SignupController : Controller
    {
        // GET: Signup
        // GET: Registration
        //  [HttpGet]
        [UserAuthenticationFilters]
        public ActionResult Signup()
        {
            return View();
        }

        // public ActionResult Signup(Tbl_Usermaster user)
        // {

        //     string message = string.Empty;
        //     if (ModelState.IsValid)
        //     {
        //         DARZANTESTEntities1 usersEntities = new DARZANTESTEntities1();
        //         user.UserId = 5;
        //         user.Roles = "Employee";
        //         usersEntities.Tbl_Usermaster.Add(user);
        //         usersEntities.SaveChanges();
        //         ViewBag.Message = "login Sucessfull";
        //     }

        //     else
        //     {
        //         ViewBag.Message = "Registration failed";
        //         //}
        //     }


        //return View();
        //public JsonResult Regform(Tbl_Usermaster LoginUser)
        //{
        //    DARZANTESTEntities db = new DARZANTESTEntities();

           
        //    var result = (from user in db.Tbl_Usermaster where (user.Email == LoginUser.Email || user.UserName == LoginUser.UserName) select user).FirstOrDefault();
        //    // r = from a in db.Tbl_Usermaster.Where(a => a.UserName == LoginUser.UserName || a.Email == LoginUser.Email)select new {};
        //    // var result = db.Tbl_Usermaster.Where(a => a.Email == LoginUser.Email || a.UserName == LoginUser.UserName).FirstOrDefault();
        //    // var result=   from a in db.Tbl_Usermaster.Where (a => a.Email == LoginUser.Email || a.UserName == LoginUser.UserName) select new { Value = a.UserName, Text = a.Email };

        //    if (result == null)
        //    {
               
        //        //  user.UserId = 5;
        //        LoginUser.Roles = "Employee";
        //        db.Tbl_Usermaster.Add(LoginUser);
        //        db.SaveChanges();
        //      //  ViewBag.Message = "login Sucessfull";

        //        return Json(new { value = "Success" }, JsonRequestBehavior.AllowGet);
        //    }

        //    else
        //    {
        //        // ModelState.AddModelError("", "Invalid login attempt.");
        //        return Json(new { value = "Fail" }, JsonRequestBehavior.AllowGet);
        //    }
       // }
    }
}
