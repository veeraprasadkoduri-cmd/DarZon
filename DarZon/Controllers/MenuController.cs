using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DarZon.Models;

namespace DarZon.Controllers
{
    public class MenuController : Controller
    {
        // GET: Menu
     
        DARZANTESTEntities db = new DARZANTESTEntities();

     List<C_USER_MASTER> objlist = new List<C_USER_MASTER>();
        public ActionResult menu()
        {
            string username = (string)Session["UserName"];

            if(objlist!=null)
            {
             objlist = (from a in db.C_USER_MASTER.AsEnumerable() where a.U_User == username orderby int.Parse(a.Code)   select a).ToList();
            }

            return PartialView(objlist);
        }



    }
}