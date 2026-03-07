using DarZon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace DarZon.Controllers
{
    public class TailorAssignedDashboardController : Controller
    {
        // GET: TailorAssignedDashboard
        DARZANTESTEntities db = new DARZANTESTEntities();
        [UserAuthenticationFilters]
        public ActionResult TailorAssignedDashboard()

        {
            Tailorassignment obj = new Tailorassignment();
            obj.Objdate = new Tailorassignmentdates();
            obj.objchilddata = new List<Tailorchilddata>();
            string uname = Session["UserName"].ToString();
            ViewBag.uname = uname;
            var username = (from a in db.OHEMs where a.U_EMPCODE == uname select a).FirstOrDefault();

            // List<Tailorchilddata> orderlist = new List<Tailorchilddata>();
           
    
         
          string  assignusername = (username.firstName + username.middleName + username.lastName);
            if (TempData["datatlist"] == null)
            {
                // List<Tailorchilddata> orderlist = new List<Tailorchilddata>();
                if (Session["UserName"].ToString() == "admin")
                {

                    obj.objchilddata = (from a in db.ORDRs.AsEnumerable() join b in db.RDR1.AsEnumerable() on a.DocEntry equals b.DocEntry where b.U_status == "Assigned" && b.U_ParentItem=="0" && b.U_DSTS!="Yes" select new Tailorchilddata { saleorder = a.DocNum.ToString(), status = b.U_status, lineno = b.LineNum, LabelNo = b.DocEntry.ToString() + "-" + a.DocNum.ToString() + "-" + b.LineNum.ToString(), itemcode = b.ItemCode, Description = b.Dscription, custname = a.CardName, schedulefordelivey = (a.DocDate ?? DateTime.Now).ToString("dd/MM/yyyy"), internaldeliveydate = (b.U_DelDate ?? DateTime.Now).ToString("dd/MM/yyyy"), Trailstatus = b.U_TrailStatus, mastertailor = b.U_mastertailor, MPI=b.U_MPI,websaleoredr=a.U_WebSaleOrder }).ToList();
                }
                else
                {

                    obj.objchilddata = (from a in db.ORDRs.AsEnumerable() join b in db.RDR1.AsEnumerable() on a.DocEntry equals b.DocEntry where b.U_status == "Assigned"&& b.U_prdesigner == assignusername && b.U_DSTS != "Yes" &&  b.U_ParentItem == "0" select new Tailorchilddata { saleorder = a.DocNum.ToString(), status = b.U_status, lineno = b.LineNum, itemcode = b.ItemCode, Description = b.Dscription, custname = a.CardName, LabelNo = b.DocEntry.ToString() + "-" + a.DocNum.ToString() + "-" + b.LineNum.ToString(), schedulefordelivey = (a.DocDate ?? DateTime.Now).ToString("dd/MM/yyyy"), internaldeliveydate = (b.U_DelDate ?? DateTime.Now).ToString("dd/MM/yyyy"), Trailstatus = b.U_TrailStatus, mastertailor = b.U_mastertailor, MPI = b.U_MPI, websaleoredr = a.U_WebSaleOrder }).ToList();

                }
                return View(obj);
              
            }
            else
            {
                 obj.objchilddata =(List<Tailorchilddata>) TempData["datatlist"];
                obj.Objdate = (Tailorassignmentdates)TempData["dates"];

                return View(obj);
            }
          

        }
        // where  b.U_mastertailor != null && b.U_prdesigner != null
        public PartialViewResult TrailDashboardadditem(string fromdate,string todate)
        {
            List<Tailorchilddata> objinvlist = new List<Tailorchilddata>();

            //ViewBag.Itemcategory = category;
            var user = (from a in db.OHEMs select new { Value = a.empID, Text = a.firstName + a.middleName + a.lastName }).ToList();
            ViewBag.userlist = user;
            objinvlist = (from a in db.ORDRs.AsEnumerable() join b in db.RDR1.AsEnumerable() on a.DocEntry equals b.DocEntry  select new Tailorchilddata { saleorder = a.DocNum.ToString(), status = b.U_status, lineno = b.LineNum, itemcode = b.ItemCode, Description = b.Dscription, LabelNo = b.DocEntry.ToString() + "-" + a.DocNum.ToString() + "-" + b.LineNum.ToString(), custname = a.CardName, schedulefordelivey = (a.DocDate ?? DateTime.Now).ToString("dd/MM/yyyy"), internaldeliveydate = (b.U_DelDate ?? DateTime.Now).ToString("dd/MM/yyyy"), Trailstatus = b.U_TrailStatus,websaleoredr=a.U_WebSaleOrder }).ToList();

            //  custname = a.CardName,itemcode = Convert.ToInt32(b.ItemCode),Description = b.Dscription,status = a.DocStatus a.DocDate>= frmdate && a.DocDate<= tdate &&
            return PartialView("Trailadditem", objinvlist);
        }
        public ActionResult searchdata(string fromdate,string todate)
        {
            Tailorassignmentdates objdateto = new Tailorassignmentdates();
            string uname = Session["UserName"].ToString();
            var username = (from a in db.OHEMs where a.U_EMPCODE == uname select a).FirstOrDefault();
            TempData["username"] = (username.firstName + username.middleName + username.lastName);

            List<Tailorchilddata> orderlist = new List<Tailorchilddata>();
            var dutchCulture = System.Globalization.CultureInfo.CreateSpecificCulture("nl-NL");
            DateTime frmdate = DateTime.ParseExact(fromdate, "dd/MM/yyyy", null);
            DateTime tdate = DateTime.ParseExact(todate, "dd/MM/yyyy", null);
           
            var user = (from a in db.OHEMs select new { Value = a.empID, Text = a.firstName + a.middleName + a.lastName }).ToList();
            ViewBag.userlist = user;
            if (Session["UserName"].ToString() == "admin")
            {
                orderlist = (from a in db.ORDRs.AsEnumerable() join b in db.RDR1.AsEnumerable() on a.DocEntry equals b.DocEntry where DateTime.ParseExact((a.DocDate ?? DateTime.Now).ToString("dd/MM/yyyy"), "dd/MM/yyyy", null) >= frmdate && DateTime.ParseExact((a.DocDate ?? DateTime.Now).ToString("dd/MM/yyyy"), "dd/MM/yyyy", null) <= tdate && b.U_status == "Assigned"  && b.U_ParentItem=="0" && b.U_DSTS !="Yes" select new Tailorchilddata { saleorder = a.DocNum.ToString(), LabelNo = b.DocEntry.ToString() + "-" + a.DocNum.ToString() + "-" + b.LineNum.ToString(), status = b.U_status, lineno = b.LineNum, itemcode = b.ItemCode, Description = b.Dscription, custname = a.CardName, schedulefordelivey = (a.DocDate ?? DateTime.Now).ToString("dd/MM/yyyy"), internaldeliveydate = (b.U_DelDate ?? DateTime.Now).ToString("dd/MM/yyyy"), Trailstatus = b.U_TrailStatus, mastertailor = b.U_mastertailor, MPI = b.U_MPI,websaleoredr=a.U_WebSaleOrder }).ToList();
            }
            else
            {
                orderlist = (from a in db.ORDRs.AsEnumerable() join b in db.RDR1.AsEnumerable() on a.DocEntry equals b.DocEntry where DateTime.ParseExact((a.DocDate ?? DateTime.Now).ToString("dd/MM/yyyy"), "dd/MM/yyyy", null) >= frmdate && DateTime.ParseExact((a.DocDate ?? DateTime.Now).ToString("dd/MM/yyyy"), "dd/MM/yyyy", null) <= tdate && b.U_status == "Assigned" && b.U_prdesigner == TempData["username"].ToString() && b.U_ParentItem == "0"  && b.U_DSTS != "Yes"  select new Tailorchilddata { saleorder = a.DocNum.ToString(), LabelNo = b.DocEntry.ToString() + "-" + a.DocNum.ToString() + "-" + b.LineNum.ToString(), status = b.U_status, lineno = b.LineNum, itemcode = b.ItemCode, Description = b.Dscription, custname = a.CardName, schedulefordelivey = (a.DocDate ?? DateTime.Now).ToString("dd/MM/yyyy"), internaldeliveydate = (b.U_DelDate ?? DateTime.Now).ToString("dd/MM/yyyy"), Trailstatus = b.U_TrailStatus, mastertailor = b.U_mastertailor, MPI = b.U_MPI,websaleoredr=a.U_WebSaleOrder }).ToList();

            }
            objdateto.fromdate = fromdate;
            objdateto.todate = todate;
            TempData["dates"] = objdateto;
            TempData["datatlist"] = orderlist;
            return Json(new { value = "Success" }, JsonRequestBehavior.AllowGet);
        }


    }
}