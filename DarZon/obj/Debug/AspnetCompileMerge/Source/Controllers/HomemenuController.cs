using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc;
using DarZon.Models;
namespace DarZon.Controllers
{
    public class HomemenuController : Controller
    {
        // GET: Homemenu
        DARZANTESTEntities db = new DARZANTESTEntities();
        public ActionResult Homemenu()
        {
            string username = (string)Session["UserName"];
           
           
            var menulist = (from a in db.C_USER_MASTER.AsEnumerable() where a.U_User == username  orderby int.Parse(a.Code) select a).ToList();

            return PartialView(menulist);
        }
    }
}