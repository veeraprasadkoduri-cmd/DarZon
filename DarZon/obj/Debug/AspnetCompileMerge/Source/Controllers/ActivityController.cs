using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DarZon.Models;
using DarZon.DAL;

namespace DarZon.Controllers
{
    public class ActivityController : Controller
    {
        // GET: Activity
        DARZANTESTEntities db = new DARZANTESTEntities();
        [UserAuthenticationFilters]
        public ActionResult Activity()

        {
            Activity objact = new Activity();
            objact.Assignempid = "Please Select";

            objact.Assignby = Session["UserName"].ToString();
            if (TempData["Transdata"] != null)
            {
                objact = (Activity)TempData["Transdata"];

                if(objact.status=="Y")
                {
                    objact.status = "Closed";
                }
                else
                {
                    objact.status = "Open";
                }


                ViewBag.status = objact.status;
                if (objact.Activitytype == "T")
                {
                    objact.Activitytype = "Task";
                }
                else if (objact.Activitytype == "N")
                {
                    objact.Activitytype = "Others";
                }
                else if (objact.Activitytype == "C")
                {
                    objact.Activitytype = "Phone Call";
                }
                else if (objact.Activitytype == "M")
                {
                    objact.Activitytype = "Meeting";
                }
                else if (objact.Activitytype == "E")
                {
                    objact.Activitytype = "Note";
                }
                else if (objact.Activitytype == "P")
                {
                    objact.Activitytype = "Campaign";
                }

                else
                {
                }
                    ViewBag.activitytype = objact.Activitytype;
            }
            else
            {


                if (TempData["predetails"] != null)
                    objact = (Activity)TempData["predetails"];


             


                ViewBag.activitytype = (new List<SelectListItem>
{
    new SelectListItem { Selected = false, Text = "Phone Call", Value = ("Phone Call")},
    new SelectListItem { Selected = false, Text = "Meeting", Value = ("Meeting")},
     new SelectListItem { Selected = false, Text = "Task", Value = "Task"},
       new SelectListItem { Selected = false, Text = "Note", Value = "Note"},
    new SelectListItem { Selected = false, Text = "Campaign", Value = "Campaign"},
     new SelectListItem { Selected = false, Text = "Other", Value = "Other"},
});
                ViewBag.status = (new List<SelectListItem>
{
    new SelectListItem { Selected = false, Text = "Open", Value = "Open"},
    new SelectListItem { Selected = false, Text = "Inactive", Value = "Inactive"},
     new SelectListItem { Selected = false, Text = "Close", Value = "Close"},

});
            }

            var user = (from a in db.OHEMs select new { Value = a.empID, Text = a.firstName + a.middleName + a.lastName }).ToList();
            ViewBag.userid = user;


            return View(objact);
        }

        [HttpPost]
        [AllowAnonymous]
        [ActionName("Activity")]
        [onAction(ButtonName = "Save")]
        public ActionResult Save(ActivityModel objact)
        {
            SAPIntegration objsapint = new SAPIntegration();
            string sapmsg = objsapint.Activity(objact);

            if (sapmsg == "Success")
            {

                return Content("<script language='javascript' type='text/javascript'>alert('Activity Details Submitted Successfully');window.location ='../Activity/Activity';</script>");

            }
            else
            {
                //return Content("<script language='javascript' type='text/javascript'>alert(" + sapmsg + ");</script>");
                //return RedirectToAction("CustomerMaster", "CustomerMaster", new { objcustomer = objcustomer });
                TempData["predetails"] = objact;
                return Content("<script language='javascript' type='text/javascript'>alert('" + sapmsg + "');window.location ='../Activity/Activity';</script>");
            }

        }

        public ActionResult Orderlist(string customerno)
        {

            var orderlist = (from a in db.ORDRs.AsEnumerable()
                             join c in db.OCRDs.AsEnumerable() on a.CardCode equals c.CardCode

                             where (c.CardCode == customerno || customerno.Length <= 0) && a.DocStatus=="O"
                             select new DeliveryHead
                             {
                                 websaleorder=a.U_WebSaleOrder,
                                 SaleOrderNo = a.DocNum.ToString(),
                                 DelDate = (a.DocDate ?? DateTime.Now).ToString("dd/MM/yyyy"),
                                 CustomerName = a.CardName,
                                 CustomerNo = a.CardCode,
                                 PhoneNo = c.Cellular,
                                 AltPhoneNo = c.Phone2,

                                 DocTotal = Math.Round((from d in db.RDR1 where d.DocEntry == a.DocEntry && d.U_TrailStatus == "Yes" select d.LineTotal).Sum() ?? 0, 0),
                                 TotalBefDis = Math.Round(((a.DocTotal ?? 0) - (a.VatSum ?? 0 - a.DiscSum ?? 0)), 0),
                                 DiscountP = Math.Round(a.DiscPrcnt ?? 0, 0),
                                 Tax = Math.Round(a.VatSum ?? 0, 0),
                                 saleorderdate = (a.DocDate ?? DateTime.Now).ToString("dd/MM/yyyy"),
                                 delveryNo = a.DocEntry.ToString()
                             }).ToList();


            return PartialView(orderlist);
        }




        public ActionResult Find(string custcode)
        {

            var actlist = (from a in db.OCLGs.AsEnumerable()
                           where a.Closed == "N" && a.ClgCode ==int.Parse(custcode)

                           select new Activity
                           {
                               Customercode = a.CardCode,
                               Customername = (from b in db.OCRDs where b.CardCode == a.CardCode select b.CardName).FirstOrDefault(),
                               mobileno = a.Tel,
                               Activitytype = a.Action,
                               Actvityno = a.ClgCode,
                                Assignempid =a.U_AssignBy,
                               Assignby = a.U_User,
                               Startdate = (a.Recontact ?? DateTime.Now).ToString("dd/MM/yyyy"),
                               enddate = (a.endDate ?? DateTime.Now).ToString("dd/MM/yyyy"),
                               saleorder = a.DocNum,
                               starttimeto = a.BeginTime.ToString(),
                               Endtime = a.ENDTime.ToString(),
                               status = a.Closed,
                               custcomments = a.Details,
                               operatorcomments = a.Notes
                           }).FirstOrDefault();
            TempData["Transdata"] = actlist;
            actlist.Activitytype = (from a in db.OCLGs.AsEnumerable()
                                    where a.Closed == "N" && a.ClgCode ==int.Parse(custcode)
                                    select a.Action ).FirstOrDefault();

            actlist.status = (from a in db.OCLGs.AsEnumerable()
                              where a.Closed == "N" && a.ClgCode ==int.Parse(custcode)
                              select a.Closed).FirstOrDefault();
            TempData["Transdata"] = actlist;
            return RedirectToAction("Activity", "Activity");
        }


        public ActionResult actlistpop()
        {
            var actlist = (from a in db.OCLGs.AsEnumerable()
                           where a.Closed == "N"
                           select new Activity
                           {
                               Actvityno = a.ClgCode,
                               saleorder = a.DocNum??"",
                               Customercode = a.CardCode,
                               Customername = (from b in db.OCRDs where b.CardCode == a.CardCode select b.CardName).FirstOrDefault(),
                               websaleorder=(from c in db.ORDRs.AsEnumerable() where c.DocEntry.ToString()== (a.DocNum ?? "") select c.U_WebSaleOrder??"" ).FirstOrDefault(),
                           }).ToList();



            return PartialView(actlist);

        }
    }
}