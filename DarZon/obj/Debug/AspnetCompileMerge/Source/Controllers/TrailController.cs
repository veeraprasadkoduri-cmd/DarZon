using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DarZon.Models;
using DarZon;
namespace DarZon.Controllers
{
    public class TrailController : Controller
    {
        // GET: Trail

        DARZANTESTEntities db = new DARZANTESTEntities();
        [UserAuthenticationFilters]
        public ActionResult Trail()
        {
            Tailorassignment objtril = new Tailorassignment();

            objtril.Objdate = new Tailorassignmentdates();
            objtril.objchilddata = new List<Tailorchilddata>();
            if (Session["UserName"] != null)
            {
              // ViewBag.username = Session["UserName"].ToString();
                // obj.Objdate.username = Session["UserName"].ToString();
                ViewBag.trailslist= trailOrderList("", "");
                objtril.Objdate.username = Session["UserName"].ToString();

                // ViewBag.orderlist = TrailListforfirst().Select(a=>a.Docentry);
            }
            else
            {
                //obj.Objdate.username = "";
            }




            return View(objtril);
        }

        public ActionResult TrailList()
        {
            List<Tailorchilddata> trail = new List<Tailorchilddata>();
            trail= TrailListforfirst();
            return PartialView(trail);
        }

        public List<Tailorchilddata> TrailListforfirst()
        {
            string uname = Session["UserName"].ToString();
            var username = (from a in db.OHEMs where a.U_EMPCODE == uname select a).FirstOrDefault();
            TempData["username"] = (username.firstName + username.middleName + username.lastName);
            string prdsgnr = TempData["username"].ToString();
            List<Tailorchilddata> ordrlist = new List<Tailorchilddata>();

            if (Session["UserName"].ToString() == "admin")
            {
                 ordrlist = (from a in db.ORDRs.AsEnumerable() where (from c in db.RDR1 where c.U_status == "Assigned" && (c.U_TrailStatus == null || c.U_TrailStatus == "No") select c.DocEntry).Contains(a.DocEntry) && a.DocStatus == "O" select new Tailorchilddata { saleorder = a.DocNum.ToString(), Docentry = a.DocEntry, custname = a.CardName, CustomerNo = a.CardCode, websaleoredr = a.U_WebSaleOrder,docdate=a.DocDate??DateTime.Now }).Distinct().ToList();
              
            }
            else
            {
                 ordrlist = (from a in db.ORDRs.AsEnumerable() where (from c in db.RDR1 where  c.U_status == "Assigned" && (c.U_TrailStatus == null || c.U_TrailStatus == "No") select c.DocEntry).Contains(a.DocEntry) && a.DocStatus == "O" select new Tailorchilddata { saleorder = a.DocNum.ToString(), Docentry = a.DocEntry, custname = a.CardName, CustomerNo = a.CardCode, websaleoredr = a.U_WebSaleOrder, docdate = a.DocDate ?? DateTime.Now }).Distinct().ToList();
                
            }
            return ordrlist;



        }


        //Trailadditem
        public PartialViewResult Trailadditem( string fromdate, string todate)
        {
            List<Tailorchilddata> objinvlist = trailOrderList(fromdate, todate);
            //  custname = a.CardName,itemcode = Convert.ToInt32(b.ItemCode),Description = b.Dscription,status = a.DocStatus a.DocDate>= frmdate && a.DocDate<= tdate &&
            return PartialView("Trailadditem", objinvlist);
        }
        public List<Tailorchilddata> trailOrderList(string fromdate, string todate)
        {
            var dutchCulture = System.Globalization.CultureInfo.CreateSpecificCulture("nl-NL");
            DateTime frmdate = new DateTime();
            DateTime tdate = new DateTime();
            if (fromdate != "")
            {
                 frmdate = DateTime.ParseExact(fromdate, "dd/MM/yyyy", dutchCulture);
                 tdate = DateTime.ParseExact(todate, "dd/MM/yyyy", dutchCulture);
            }
            List<int> docmunbers = new List<int>();
            if (fromdate=="")
             docmunbers = TrailListforfirst().Select(a => a.Docentry).ToList();
            else
                docmunbers = TrailListforfirst().Where(a=> DateTime.ParseExact(a.docdate.ToString("dd/MM/yyyy"),"dd/MM/yyyy", dutchCulture )>= frmdate && DateTime.ParseExact(a.docdate.ToString("dd/MM/yyyy"), "dd/MM/yyyy", dutchCulture)<= tdate).Select(a => a.Docentry).ToList();
            List<Tailorchilddata> objinvlist = new List<Tailorchilddata>();
            ViewBag.status = (new List<SelectListItem>
{
    new SelectListItem { Selected = false, Text = "Yes", Value = ("Yes")},
    new SelectListItem { Selected = false, Text = "No", Value = ("No")},

});
            //ViewBag.Itemcategory = category;
            var user = (from a in db.OHEMs select new { Value = a.empID, Text = a.firstName + a.middleName + a.lastName }).ToList();
            ViewBag.userlist = user;

            if (fromdate == "")
                objinvlist = (from a in db.ORDRs.AsEnumerable() join b in db.RDR1.AsEnumerable() on a.DocEntry equals b.DocEntry where docmunbers.Contains(a.DocEntry) && b.U_status == "Assigned" && (b.U_TrailStatus == null || b.U_TrailStatus == "No") && b.U_ParentItem == "0" select new Tailorchilddata { saleorder = a.DocNum.ToString(), status = b.U_status, lineno = b.LineNum, itemcode = b.ItemCode, Description = b.Dscription, Docentry = a.DocEntry, custname = a.CardName, prdesigner = b.U_prdesigner, mastertailor = b.U_mastertailor, schedulefordelivey = (b.U_DelDate ?? DateTime.Now).ToString("dd/MM/yyyy"), internaldeliveydate = (DateTime.ParseExact(((b.U_DelDate ?? DateTime.Now).ToString("dd/MM/yyyy")), "dd/MM/yyyy", null).AddDays(-3)).ToString("dd/MM/yyyy"), websaleoredr = a.U_WebSaleOrder, LabelNo = b.DocEntry.ToString() + "-" + a.DocNum.ToString() + "-" + b.LineNum.ToString() }).ToList();
            else
                objinvlist = (from a in db.ORDRs.AsEnumerable() join b in db.RDR1.AsEnumerable() on a.DocEntry equals b.DocEntry where a.DocDate >= frmdate && a.DocDate <= tdate && docmunbers.Contains(a.DocEntry) && b.U_status == "Assigned" && (b.U_TrailStatus == null || b.U_TrailStatus == "No") && b.U_ParentItem == "0" select new Tailorchilddata { saleorder = a.DocNum.ToString(), status = b.U_status, lineno = b.LineNum, itemcode = b.ItemCode, Description = b.Dscription, Docentry = a.DocEntry, custname = a.CardName, prdesigner = b.U_prdesigner, mastertailor = b.U_mastertailor, schedulefordelivey = (b.U_DelDate ?? DateTime.Now).ToString("dd/MM/yyyy"), internaldeliveydate = (DateTime.ParseExact(((b.U_DelDate ?? DateTime.Now).ToString("dd/MM/yyyy")), "dd/MM/yyyy", null).AddDays(-3)).ToString("dd/MM/yyyy"), websaleoredr = a.U_WebSaleOrder, LabelNo = b.DocEntry.ToString() + "-" + a.DocNum.ToString() + "-" + b.LineNum.ToString() }).ToList();

            return objinvlist;

        }



        [HttpPost]
        [AllowAnonymous]
        [ActionName("Trail")]
        [onAction(ButtonName = "Save")]
        public ActionResult Save(Tailorassignment objinvlist)
        {
            foreach (var items in objinvlist.objchilddata)
            {
                RDR1 objrdr = (from a in db.ORDRs.AsEnumerable() join b in db.RDR1.AsEnumerable() on a.DocEntry equals b.DocEntry where a.DocEntry == items.Docentry && b.LineNum ==items.lineno select b).FirstOrDefault();

                objrdr.U_TrailStatus = items.Trailstatus;
                db.Entry(objrdr).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
           // string docnum = ViewData["docnum"].ToString();
          //  return View();
            return Content("<script language='javascript' type='text/javascript'>alert('Ready for Trial Update Successfully');window.location ='../Trail/Trail';</script>");
        }
    }
}