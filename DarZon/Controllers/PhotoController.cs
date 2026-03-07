using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DarZon.Controllers
{
    public class PhotoController : Controller
    {
        // GET: Photo
       
            [HttpGet]
            public ActionResult Index()
            {
                return View();
            }


       
    }
}