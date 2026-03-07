using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DarZon.Models;
using DarZon;

namespace DarZon.Controllers
{
    public class StatusDashboardController : Controller
    {
        // GET: StatusDashboard
        DARZANTESTEntities db = new DARZANTESTEntities();

        [UserAuthenticationFilters]
        public ActionResult StatusDashboard()
        {
            Statusdabrdmain obj = new Statusdabrdmain();
            obj.Objdate = new Tailorassignmentdates();
            obj.objchilddata = new List<StatusDashboard>();
            if (TempData["datatlist"] == null)
            {

                obj.objchilddata = (from a in db.ORDRs.AsEnumerable()
                                    join b in db.RDR1.AsEnumerable() on a.DocEntry equals b.DocEntry where a.DocStatus == "O" && b.U_ParentItem == "0"

                                    select new StatusDashboard
                                    {
                                        LabelNo = b.DocEntry.ToString() + "-" + a.DocNum.ToString() + "-" + b.LineNum.ToString(),
                                        CustomerDeliveryDate = (b.U_DelDate ?? DateTime.Now).ToString("dd/MM/yyyy"),
                                        saleorderdate = (a.DocDate ?? DateTime.Now).ToString("dd/MM/yyyy"),
                                        saleorderNo = a.DocNum.ToString(),
                                        InternalDeliveryDate = (b.U_IDelDate ?? DateTime.Now).ToString("dd/MM/yyyy"),
                                        TrailStatus = b.U_TrailStatus,
                                        MaterialDescription = b.Dscription,
                                        LineNo = b.LineNum.ToString(),
                                        tailorstatus = b.U_TrailStatus,
                                        Tstatus = b.U_TStatus,
                                        deiverystatus = b.U_DSTS,
                                        DeviationDays = ((DateTime.Now) - (b.U_DelDate ?? DateTime.Now)).Days.ToString(),
                                        websaleorder = a.U_WebSaleOrder,
                                        counttrans = (from g in db.Transportaionchilds where (from m in db.Transportaionchilds.AsEnumerable() where m.orderno == a.DocNum select m.Tno).Contains(g.Tno) select g.Tno).Distinct().Count(),
                                        status = (from k in db.Transportaionheads where (from m in db.Transportaionchilds.AsEnumerable() where m.orderno == a.DocNum select m.Tno).Contains(k.Tno) select k.Tstatus??"").ToList().LastOrDefault()

                                    }).ToList();

                    return View(obj);
             
            }
            else

            {
                obj.objchilddata= (List < StatusDashboard >) TempData["datatlist"];
                obj.Objdate =(Tailorassignmentdates) TempData["dates"];


                  return View(obj);

            }
        }
      

        //public ActionResult Orderstatuslistfromto(string fromdate, string todate)
        //{


        //    var dutchCulture = System.Globalization.CultureInfo.CreateSpecificCulture("nl-NL");
          
        //        DateTime frmdate = DateTime.ParseExact(fromdate, "dd/MM/yyyy", dutchCulture);
        //        DateTime tdate = DateTime.ParseExact(todate, "dd/MM/yyyy", dutchCulture);

        //        //where a.DocDate >= frmdate && a.DocDate <= tdate
        //        var orderlist = (from a in db.ORDRs.AsEnumerable() join c in db.OCRDs.AsEnumerable() on a.CardCode equals c.CardCode where a.DocDate >= frmdate && a.DocDate <= tdate select new DeliveryHead { SaleOrderNo = a.DocNum.ToString(), DelDate = (a.DocDate ?? DateTime.Now).ToString("dd/MM/yyyy"), CustomerName = a.CardName, CustomerNo = a.CardCode, PhoneNo = c.Cellular, AltPhoneNo = c.Phone2, DocTotal = Math.Round(a.DocTotal ?? 0, 2), TotalBefDis = Math.Round(((a.DocTotal ?? 0) - (a.VatSum ?? 0 - a.DiscSum ?? 0)), 2), DiscountP = Math.Round(a.DiscPrcnt ?? 0, 2), Tax = Math.Round(a.VatSum ?? 0, 2), saleorderdate = (a.DocDate ?? DateTime.Now).ToString("dd/MM/yyyy") }).ToList();
        //        return PartialView(orderlist);
           
          
        //}


        public ActionResult Orderstatuslistfrmdatetodate(string fromdate,string todate)
        {

            //StatusDashboard obj1 = new StatusDashboard();

            var dutchCulture = System.Globalization.CultureInfo.CreateSpecificCulture("nl-NL");

            DateTime frmdate = DateTime.ParseExact(fromdate, "dd/MM/yyyy", dutchCulture);
            DateTime tdate = DateTime.ParseExact(todate, "dd/MM/yyyy", dutchCulture);

            //where a.DocDate >= frmdate && a.DocDate <= tdate
            var orderlist = (from a in db.ORDRs.AsEnumerable() join c in db.OCRDs.AsEnumerable() on a.CardCode equals c.CardCode where a.DocDate >= frmdate && a.DocDate <= tdate select new DeliveryHead { SaleOrderNo = a.DocNum.ToString(), DelDate = (a.DocDate ?? DateTime.Now).ToString("dd/MM/yyyy"), CustomerName = a.CardName, CustomerNo = a.CardCode, PhoneNo = c.Cellular, AltPhoneNo = c.Phone2, DocTotal = Math.Round(a.DocTotal ?? 0, 2), TotalBefDis = Math.Round(((a.DocTotal ?? 0) - (a.VatSum ?? 0 - a.DiscSum ?? 0)), 2), DiscountP = Math.Round(a.DiscPrcnt ?? 0, 2), Tax = Math.Round(a.VatSum ?? 0, 2), saleorderdate = (a.DocDate ?? DateTime.Now).ToString("dd/MM/yyyy"),websaleorder=a.U_WebSaleOrder}).ToList();
                return PartialView(orderlist);
            
            
           
        }
        public ActionResult Orderstatusladditem(string fromdate, string todate)
        {
            List<StatusDashboard> objinvlist = new List<StatusDashboard>();
            Tailorassignmentdates objdate = new Tailorassignmentdates();
            //StatusDashboard obj1 = new StatusDashboard();

            var dutchCulture = System.Globalization.CultureInfo.CreateSpecificCulture("nl-NL");

            DateTime frmdate = DateTime.ParseExact(fromdate, "dd/MM/yyyy", dutchCulture);
            DateTime tdate = DateTime.ParseExact(todate, "dd/MM/yyyy", dutchCulture);

            //where a.DocDate >= frmdate && a.DocDate <= tdate
            var orderlist = (from a in db.ORDRs.AsEnumerable()
                             join b in db.RDR1.AsEnumerable() on a.DocEntry equals b.DocEntry
                             where a.DocDate >= frmdate && a.DocDate <= tdate && a.DocStatus == "O" && b.U_ParentItem == "0"

                             select new StatusDashboard
                             {
                                 LabelNo = b.DocEntry.ToString()+"-" + a.DocNum.ToString()+ "-"+ b.LineNum.ToString(),
                                 CustomerDeliveryDate = (b.U_DelDate ?? DateTime.Now).ToString("dd/MM/yyyy"),
                                 saleorderNo = a.DocNum.ToString(),
                                 InternalDeliveryDate = (b.U_IDelDate ?? DateTime.Now).ToString("dd/MM/yyyy"),
                                 TrailStatus = b.U_TrailStatus,
                                 MaterialDescription = b.Dscription,
                                 LineNo = b.LineNum.ToString(),
                                 tailorstatus = b.U_status,
                                 Tstatus = b.U_TStatus,
                                 deiverystatus = b.U_DSTS,
                                 websaleorder=a.U_WebSaleOrder,
                                 DeviationDays = ((DateTime.Now) - (b.U_DelDate ?? DateTime.Now)).Days.ToString(),
                                 saleorderdate = (a.DocDate ?? DateTime.Now).ToString("dd/MM/yyyy"),
                                 counttrans = (from g in db.Transportaionchilds where g.orderno == a.DocNum select g.Tno).Distinct().Count(),
                                 status = (from k in db.Transportaionheads where (from m in db.Transportaionchilds.AsEnumerable() where m.orderno == a.DocNum select m.Tno).Contains(k.Tno) select k.Tstatus).ToList().LastOrDefault()
                             }).ToList();

            TempData["datatlist"] = orderlist;
            objdate.todate = todate;
            objdate.fromdate = fromdate;
            TempData["dates"] = objdate;


            return Json(new { fdate = fromdate, ttdate = todate }, JsonRequestBehavior.AllowGet);

        }
    }
}