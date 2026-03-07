using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DarZon.Models;
using System.Text.RegularExpressions;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.IO;
using Microsoft.Reporting.WebForms;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Configuration;
using System.Net.Mail;
using System.IO;
//using Microsoft.Reporting.WinForms; 

namespace DarZon.Controllers
{
    public class SaleOrderController : Controller
    {
        // GET: SAleOrder
        DARZANTESTEntities db = new DARZANTESTEntities();
        [UserAuthenticationFilters]
        public ActionResult SaleOrder()
        {

          if(TempData["saleordersuccess"]!=null)
            {
                ViewBag.saleordersuccess = (string)TempData["saleordersuccess"];
            }

            saleorder objsaleorder = new saleorder();
            string whid = (string)Session["WhareHouse"];
            ViewBag.pickuserlist= (from a in db.OHEMs select new { Value = a.firstName + a.middleName + a.lastName, Text = a.firstName + a.middleName + a.lastName }).ToList();
            if (Session["UserName"] != null)
            {
                string username = (string)Session["UserName"];
                if (TempData["saleorder"] != null)
                {
                    objsaleorder = (saleorder)TempData["saleorder"];
                }
                else
                {
                    objsaleorder = getsaeleorder("N", 0);



                    if (objsaleorder.objsaleorderHeader == null)
                    {
                      
                        var sequenceno = (from a in db.Tbl_NumberSeries where a.WHID== whid select a ).FirstOrDefault();
                        if(sequenceno==null)
                        {
                            sequenceno = new Tbl_NumberSeries();
                            sequenceno.NumberPreix = "Pri";
                            sequenceno.Number = 1;
                            sequenceno.WHID = whid;
                            db.Tbl_NumberSeries.Add(sequenceno);
                            db.SaveChanges();

                        }

                        objsaleorder = new saleorder();
                        objsaleorder.objsaleorderHeader = new SaleOrderHeader();
                        objsaleorder.objsaleorderHeader.DocEntry = sequenceno.NumberPreix + "-" + sequenceno.WHID+"-" + sequenceno.Number.ToString();
                        objsaleorder.objsaleorderHeader.DocDate = DateTime.Now.ToString("dd/MM/yyyy");
                        objsaleorder.objsaleorderHeader.status = "O";

                    }
                    else
                    {
                        objsaleorder.objsaleorderHeader.status = "R";
                    }
                }
                var catglist = (from a in db.OITBs where (from b in db.OITMs select b.ItmsGrpCod).Contains(a.ItmsGrpCod) select new { Value = a.ItmsGrpCod, Text = a.ItmsGrpNam }).OrderBy(a=>a.Value).ToList();
                ViewBag.Itemcategory = catglist;

                objsaleorder.objsaleorderHeader.UserName = Session["UserName"].ToString();
                string mainuser = System.Configuration.ConfigurationManager.AppSettings["wharehouse"].ToString();
                string whereHouse = (string)Session["WhareHouse"];
                ViewBag.Saleordlist = (from a in db.SaleOrderHeaders where a.status == "C" && (a.WHSCODE== whereHouse || whereHouse == mainuser) select a).ToList();
                ViewBag.pricelist = (from a in db.C_DAYS_TARIFF select a).ToList();
                objsaleorder.objsaleorderHeader.Advance = Math.Round(objsaleorder.objsaleorderHeader.Advance ?? 0, 0);
                objsaleorder.objsaleorderHeader.BalanceAmount = Math.Round(objsaleorder.objsaleorderHeader.BalanceAmount ?? 0, 0);
                objsaleorder.objsaleorderHeader.doctotal = Math.Round(objsaleorder.objsaleorderHeader.doctotal ?? 0, 0);
                objsaleorder.objsaleorderHeader.totalAfterDiscount = Math.Round(objsaleorder.objsaleorderHeader.totalAfterDiscount ?? 0, 0);
                objsaleorder.objsaleorderHeader.Discount = Math.Round(objsaleorder.objsaleorderHeader.Discount ?? 0, 0);
                objsaleorder.objsaleorderHeader.totalBeorerDiscount = Math.Round(objsaleorder.objsaleorderHeader.totalBeorerDiscount ?? 0, 0);
                objsaleorder.objsaleorderHeader.cash = Math.Round(objsaleorder.objsaleorderHeader.cash ?? 0, 0);
                objsaleorder.objsaleorderHeader.card = Math.Round(objsaleorder.objsaleorderHeader.card ?? 0, 0);
                objsaleorder.objsaleorderHeader.OtherPayments = Math.Round(objsaleorder.objsaleorderHeader.OtherPayments ?? 0, 0);
                return View(objsaleorder);
            }
            else
            {
                return View("Login", "Login");

            }

        }
        public ActionResult customerdetails()
        {
            return View();
        }

        //public PartialViewResult AddItem(SaleOrderDetails objsaleorder)

        public PartialViewResult AddItem(string category, string Itemcode, string Itemname, string Unitpeice,string noofdays)
        {
            //  return PartialView("_LineItmes", objsaleorder);
            //var catglist = (from a in db.OITBs   select new { Value = a.ItmsGrpCod, Text = a.ItmsGrpNam }).ToList();
            SaleorderwithMeasurement objsaleorder = new SaleorderwithMeasurement();
            objsaleorder.ItemCode = Itemcode;
            objsaleorder.ItemDescription = Itemname;
            objsaleorder.noofdays = noofdays;
            objsaleorder.UnitPrice = Math.Round(decimal.Parse(Unitpeice), 0);
            ViewBag.Itemcategory = category;
            return PartialView("AddItem", objsaleorder);
        }
        [HttpPost]
        [AllowAnonymous]
        [ActionName("SaleOrder")]
        [onAction(ButtonName = "Save")]
        public ActionResult measurementList(saleorder objsaleorder)
        {
            TempData["Cardcode"] = objsaleorder.objsaleorderHeader.CardCode;
            SapSaleorder objsalorde = new SapSaleorder();
            objsalorde.objsaleorder = objsaleorder.objsaleorderHeader;

            foreach (SaleorderwithMeasurement objmesu in objsaleorder.objlistsalDetails)
            {
                SaleOrderDetail objsale = (from a in db.SaleOrderDetails where a.Id == objmesu.Id select a).FirstOrDefault();
                 objsale.Intdeldate = objmesu.Intdeldate;
                objsale.Deliverydate = objmesu.Deliverydate;
                objsale.Remarks = objmesu.Remarks;
                db.Entry(objsale).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }


            objsalorde.objsaleorderdetails = (from a in db.SaleOrderDetails.AsEnumerable() where a.saleorderNo == objsaleorder.objsaleorderHeader.DocEntry && (int.Parse(a.quantity??"0")>0 || a.parentId=="0") select a).ToList();

            foreach (SaleorderwithMeasurement objdetails in objsaleorder.objlistsalDetails)
            {
                List<SaleOrderDetail> addon = (from a in db.SaleOrderDetails.AsEnumerable() where a.parentId == objdetails.Id.ToString() select a).ToList();
                objdetails.addonlist = new List<SaleOrderDetail>();
                objdetails.addonlist.AddRange(addon);

            }

            objsaleorder.objsaleorderHeader.status = "C";
            objsaleorder.objsaleorderHeader.WHSCODE= (string)Session["WhareHouse"];
            db.Entry(objsaleorder.objsaleorderHeader).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            SAPIntegration objsapint = new SAPIntegration();


            string sapmsg = objsapint.saleorder(objsalorde);
            string message = "";
            if (sapmsg.Length > 20)
            {
                message = sapmsg.Substring(0, 19); ;

            }
            if (sapmsg == "Success")
            {

                // return Content("<script language='javascript' type='text/javascript'>alert('Sale Order Submitted Successfully!');window.location ='../SaleOrder/Sendemail';</script>");
                TempData["saleordersuccess"] = "Sucess";
                TempData["saleorder"] = objsaleorder;
                return Content("<script language='javascript' type='text/javascript'>alert('Sale Order Submitted Successfully!');window.location ='../SaleOrder/SaleOrder';</script>");

                

            }
            else
            {

                //return Content("<script language='javascript' type='text/javascript'>alert(" + sapmsg + ");</script>");
                //return RedirectToAction("CustomerMaster", "CustomerMaster", new { objcustomer = objcustomer });
                objsaleorder.objsaleorderHeader.status = "R";
                objsaleorder.objsaleorderHeader.card = null;
                objsaleorder.objsaleorderHeader.cash = null;
                objsaleorder.objsaleorderHeader.carddetails ="";
                objsaleorder.objsaleorderHeader.OtherPayments = null;
                objsaleorder.objsaleorderHeader.OthpayDetails = "";
                objsaleorder.objsaleorderHeader.totalAfterDiscount =Math.Round(objsaleorder.objsaleorderHeader.totalBeorerDiscount??0,0);
                objsaleorder.objsaleorderHeader.Advance = null;
                objsaleorder.objsaleorderHeader.BalanceAmount= Math.Round(objsaleorder.objsaleorderHeader.totalBeorerDiscount??0,0);
                objsaleorder.objsaleorderHeader.Discount=null;
                objsaleorder.objsaleorderHeader.WHSCODE=(string)Session["WhareHouse"];
                db.Entry(objsaleorder.objsaleorderHeader).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                TempData["saleorder"] = objsaleorder;
                return Content("<script language='javascript' type='text/javascript'>alert('" + message + "');window.location ='../SaleOrder/SaleOrder';</script>");
            }

            
            //return RedirectToAction("SaleOrder", "SaleOrder");
        }
        public PartialViewResult AddExistingItem(SaleorderwithMeasurement objsaleorder)
        {
            return PartialView("AddItem", objsaleorder);
        }
        [HttpPost]
        [AllowAnonymous]
        [ActionName("SaleOrder")]
        [onAction(ButtonName = "Print")]
        public ActionResult Print(saleorder objsaleorder)
        {
             objsaleorder.objsaleorderHeader.Series = 1;
            return commanprint(objsaleorder);

        }

        [HttpPost]
        [AllowAnonymous]
        [ActionName("SaleOrder")]
        [onAction(ButtonName = "popupPrint")]
        public ActionResult popupPrint(saleorder objsaleorder)
        {
            //foreach (SaleorderwithMeasurement objdetails in objsaleorder.objlistsalDetails)
            //{
            //    List<SaleOrderDetail> addon = (from a in db.SaleOrderDetails.AsEnumerable() where a.parentId == objdetails.Id.ToString() select a).ToList();
            return commanprint(objsaleorder);



        }





        public ActionResult commanprint(saleorder objsaleorder)
        {
            LocalReport lr = new LocalReport();
            if (objsaleorder.objsaleorderHeader.Series== 1)
            {
                TempData.Remove("saleorder");
               
            }

            string path = Path.Combine(Server.MapPath("~/DarzanReport/Sale.rdlc"));
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                lr.ReportPath = path;
            }
            string websale = objsaleorder.objsaleorderHeader.DocEntry;

            using (db = new DARZANTESTEntities())
            {


                List<Saleorderreportchild> orderlist = new List<Saleorderreportchild>();

                SaleorderreportHead headprint = new SaleorderreportHead();

                headprint = (from a in db.ORDRs.AsEnumerable()
                             join b in db.RDR1.AsEnumerable() on a.DocEntry equals b.DocEntry
                             where a.U_WebSaleOrder == websale
                             select new SaleorderreportHead
                             {
                                 U_DelDate = (from g in db.ORDRs.AsEnumerable()
                                              join h in db.RDR1.AsEnumerable() on a.DocEntry equals b.DocEntry
                                              orderby h.U_DelDate descending
                                              where a.U_WebSaleOrder == websale
                                              select (h.U_DelDate ?? DateTime.Now).ToString("dd/MM/yyyy")).FirstOrDefault(),
                                 CardName = a.CardName,
                                 DocNum = a.DocNum,
                                 Cellular = (from c in db.OCRDs where a.CardCode == c.CardCode select c.Cellular).FirstOrDefault(),
                                 CardCode = a.CardCode,
                                 Street = (from c in db.CRD1 where c.CardCode == a.CardCode where c.AdresType == "S" select c.Street).FirstOrDefault(),

                                 Block = (from c in db.CRD1 where c.CardCode == a.CardCode where c.AdresType == "S" select c.Block + "," + c.City).FirstOrDefault(),
                                 ItmsGrpNam = ((from d in db.OITMs join e in db.OITBs on d.ItmsGrpCod equals e.ItmsGrpCod where d.ItemCode == b.ItemCode select e.ItmsGrpNam).FirstOrDefault()),
                                 TotalBeforeDiscount = (a.DocTotal ?? 0 - a.VatSum ?? 0 - a.DiscSum ?? 0),
                                 DiscPrcnt = a.DiscPrcnt,
                                 VatSum = a.VatSum,
                                 DocTotal = a.DocTotal,
                                 DiscSum = a.DiscSum,
                                 U_Adv=a.U_Adv??0,
                                 DocDate = (a.DocDate ?? DateTime.Now).ToString("dd/MM/yyyy"),
                                 //  U_DelDate = (b.U_DelDate ?? DateTime.Now).ToString("dd/MM/yyyy"),
                                 U_WebSaleOrder = a.U_WebSaleOrder,
                                 Address = (from d in db.OWHS.AsEnumerable() where b.WhsCode == d.WhsCode select (d.Street ?? "").ToString() + "," + (d.StreetNo ?? "").ToString()).ToList().FirstOrDefault(),
                                 County = (from d in db.OWHS where b.WhsCode == d.WhsCode select d.County).ToList().FirstOrDefault()

                             }).FirstOrDefault();


                List<Saleorderreportchild> objchild = (from a in db.ORDRs.AsEnumerable()
                                                       join b in db.RDR1.AsEnumerable() on a.DocEntry equals b.DocEntry
                                                       where a.U_WebSaleOrder == websale && b.U_ParentItem == "0"
                                                       select new Saleorderreportchild
                                                       {

                                                           newlinetotal = (from rr in db.RDR1 where rr.U_ParentItem == b.U_MPI select rr.LineTotal ?? 0).ToList(),
                                                           newDscription = (from rr in db.RDR1 where rr.U_ParentItem == b.U_MPI select "(" + rr.Dscription + ")").ToList(),
                                                           Dscription = (b.Dscription),
                                                           Quantity = b.Quantity,
                                                           Price = b.Price,
                                                           LineTotal = b.LineTotal,

                                                           LineNum = b.LineNum,
                                                           U_MPI = b.U_MPI,
                                                           DocDate = (a.DocDate ?? DateTime.Now).ToString("dd/MM/yyyy"),
                                                           U_DelDate = (b.U_DelDate ?? DateTime.Now).ToString("dd/MM/yyyy"),

                                                       }).ToList();


                foreach (Saleorderreportchild objresult in objchild)
                {





                    for (int i = 0; i < objresult.newDscription.Count; i++)
                    {

                        objresult.Dscription = objresult.Dscription + objresult.newDscription[i];
                    }


                    for (int i = 0; i < objresult.newlinetotal.Count; i++)
                    {
                        objresult.LineTotal = objresult.LineTotal + objresult.newlinetotal[i];

                    }


                }


                orderlist.AddRange(objchild);






                //foreach(var item in objchild)
                //{
                //    List<Saleorderreportchild> objdsc =(from rr in db.RDR1  where rr.U_ParentItem == item.U_MPI select new Saleorderreportchild { Dscription =item.Dscription+","+ rr.Dscription }).ToList();

                //    orderlist.AddRange(objdsc);
                //}
                // orderlist.AddRange(objchild);
                List<SaleorderreportHead> objheaderlist = new List<SaleorderreportHead>();
                objheaderlist.Add(headprint);

                //headprint.outletfrom = orderuserdeatails.objsaleorderHeader.Outletfrom;
                //headprint.outletto = orderuserdeatails.objsaleorderHeader.outletto;
                //headprint.Username = orderuserdeatails.objsaleorderHeader.UserID;
                //// headprint.Outletfrom = orderuserdeatails.objsaleorderHeader.Outletfrom;
                //headprint.Pickupuser = orderuserdeatails.objsaleorderHeader.pickupuser;
                //headprint.Tno = orderuserdeatails.objsaleorderHeader.Tno;
                //headprint.Tstatus = orderuserdeatails.objsaleorderHeader.Tstatus;
                //List<LogisticPrintHead> objheaderlist = new List<LogisticPrintHead>();
                //objheaderlist.Add(headprint);
                //foreach (var item in orderuserdeatails.objchild)
                //{
                //    List<Logisticprint> objchild = (from a in db.LabelDetails.AsEnumerable() where a.DocNum == item.Orderno select new Logisticprint { DocNum = a.DocNum, MainQty = a.MainQty, AddonQty = a.AddonQty, Fabric = a.Fabric, LabelNo = a.LabelNo }).ToList();

                //    orderlist.AddRange(objchild);
                //}
                //   int dno =Convert.ToInt32(TempData["delno"]);






                //LogisticPrintHead headprint = new LogisticPrintHead();
                //var orderlist = (from a in db.ORDRs
                //                 join b in db.RDR1 on a.DocEntry equals b.DocEntry
                //                 where a.U_WebSaleOrder == websale
                //                 select new
                //                 {
                //                     CardName = a.CardName,
                //                     DocNum = a.DocNum,
                //                     //CardCode = (from c in db.OCRDs where a.CardCode == c.CardCode select c.CardCode).FirstOrDefault(),
                //                     CardCode = a.CardCode,
                //                     ItmsGrpNam = ((from d in db.OITMs join e in db.OITBs on d.ItmsGrpCod equals e.ItmsGrpCod where d.ItemCode == b.ItemCode select e.ItmsGrpNam).FirstOrDefault()),

                //                     //Address = (from d in db.CRD1 where a.CardCode == d.CardCode && d.AdresType == "B" select d.Address + " " + d.Street + " " + d.City + " " + d.ZipCode).FirstOrDefault(),
                //                     b.Dscription,
                //                     b.Quantity,
                //                     Price = b.Price,
                //                     b.LineTotal,
                //                     TotalBeforeDiscount = (a.DocTotal ?? 0 - a.VatSum ?? 0 - a.DiscSum ?? 0),
                //                     a.DiscPrcnt,
                //                     a.VatSum,
                //                     a.DocTotal,
                //                     a.DiscSum,
                //                     a.DocDate,
                //                     b.U_DelDate,
                //                     b.U_ItemG,
                //                   //  Linenumber = (b.LineNum + a.DocNum + b.DocEntry),
                //                     b.DocEntry,
                //                     b.LineNum
                //                 }).ToList().Distinct();


                ReportDataSource rd = new ReportDataSource("Ordr", objheaderlist);
                // ReportDataSource rd = new ReportDataSource("Ordr", objheaderlist);

                lr.DataSources.Add(rd);
                //**********************************************************

                ReportDataSource rd1 = new ReportDataSource("Rdr1", orderlist);
                lr.DataSources.Add(rd1);


                ReportDataSource rd2 = new ReportDataSource("Ocrd", objheaderlist);
                lr.DataSources.Add(rd2);

                ReportDataSource rd3 = new ReportDataSource("Oitm", objheaderlist);
                lr.DataSources.Add(rd3);

                ReportDataSource rd4 = new ReportDataSource("Oitb", objheaderlist);
                lr.DataSources.Add(rd4);
                ReportDataSource rd5 = new ReportDataSource("Crd1", objheaderlist);
                lr.DataSources.Add(rd5);


                string reporType = "PDF";
                string mimetypr;
                string encodeing;
                string filenameexns;
                string deviceinfo = "<DeviceInfo>" + "<OutputFormat>PDF</OutputFormat>" +
                    "<PageWidth>5.8in</PageWidth>" + "<PageHeight>8.3in</PageHeight>" +
                    "</DeviceInfo>";
                Warning[] warnings;
                string[] streams;
                byte[] renderedbytes;
                //.ProcessingMode = ProcessingMode.Local;
                renderedbytes = lr.Render(reporType, deviceinfo, out mimetypr, out encodeing, out filenameexns, out streams, out warnings);

                return File(renderedbytes, mimetypr);
            }
           

        }

        public ActionResult saleorderfind(int id)
        {

            TempData["saleorder"] = getsaeleorder("F", id);


            return RedirectToAction("SaleOrder", "SaleOrder");

        }

        public saleorder getsaeleorder(string status, int id)
        {
            saleorder objsal = new saleorder();
            objsal.objlistsalDetails = new List<SaleorderwithMeasurement>();
            List<SaleOrderDetail> objdetails = new List<SaleOrderDetail>();

            if (status == "N")
            {
                string username = (string)Session["UserName"];

                var saleheader= (from a in db.SaleOrderHeaders.AsEnumerable() where (a.status == "O" || a.status == "P" || a.status == "R") && a.UserName == username select a).LastOrDefault();
                objsal.objsaleorderHeader = saleheader;
                if (objsal.objsaleorderHeader != null)
                    objdetails = (from a in db.SaleOrderDetails.AsEnumerable() where a.saleorderNo == objsal.objsaleorderHeader.DocEntry && a.parentId == "0" select a).ToList();

            }
            else
            {
                objsal.objsaleorderHeader = (from a in db.SaleOrderHeaders.AsEnumerable() where a.Id == id && a.status == "C" select a).LastOrDefault();
                objdetails = (from a in db.SaleOrderDetails.AsEnumerable() where a.saleorderNo == objsal.objsaleorderHeader.DocEntry && a.parentId == "0" select a).ToList();


            }
            foreach (SaleOrderDetail objordet in objdetails)
            {
                SaleorderwithMeasurement objmesdet = new SaleorderwithMeasurement();
                objmesdet.Advance =Math.Round(objordet.Advance??0,0);
                objmesdet.balance = Math.Round(objordet.balance??0,0);
                objmesdet.cardcoe = objordet.cardcoe;
                objmesdet.category = objordet.category;
                objmesdet.CategoryDetails = objordet.CategoryDetails;
                objmesdet.COGSCostingCode = objordet.COGSCostingCode;
                objmesdet.curstatus = objordet.curstatus;
                objmesdet.deldays = objordet.deldays;
                objmesdet.Deliverydate = objordet.Deliverydate;
                objmesdet.Id = objordet.Id;
                objmesdet.ItemCode = objordet.ItemCode;
                objmesdet.ItemDescription = objordet.ItemDescription;
                objmesdet.MeterialCost = Math.Round(objordet.MeterialCost??0);
                objmesdet.parentId = objordet.parentId;
                objmesdet.predelchar = objordet.predelchar;
                objmesdet.quantity = objordet.quantity;
                objmesdet.Remarks = objordet.Remarks;
                objmesdet.saleorderNo = objordet.saleorderNo;
                objmesdet.ServiceCost =Math.Round( objordet.ServiceCost??0,0);
                objmesdet.status = objordet.status;
                objmesdet.TaxAmount = Math.Round(objordet.TaxAmount??0,0);
                objmesdet.taxCode = objordet.taxCode;
                objmesdet.TaxRate = Math.Round(objordet.TaxRate??0,0);
                objmesdet.TotalAmount =Math.Round(objordet.TotalAmount??0,0);
                objmesdet.UnitPrice = Math.Round(objordet.UnitPrice??0,0);
                objmesdet.NoFabraic = objordet.NoFabraic;
                objmesdet.noofdays = (from a in db.OITMs where a.ItemCode == objordet.ItemCode select a.U_Days).FirstOrDefault();
                objmesdet.Intdeldate = objordet.Intdeldate;
                objsal.objlistsalDetails.Add(objmesdet);

            }

            return objsal;

        }

        public JsonResult deleteItem(int id)
        {
            try
            {

                var saleordeparentdetails = (from a in db.SaleOrderDetails.AsEnumerable() where a.Id == id  select a).FirstOrDefault();
                var salordedetails = (from a in db.SaleOrderDetails.AsEnumerable() where a.Id == id || a.parentId == id.ToString() select a).ToList();
                db.SaleOrderDetails.RemoveRange(salordedetails);
                db.SaveChanges();
                var measurement = (from a in db.Mesurements.AsEnumerable() where a.LineId == id.ToString() select a).ToList();
                db.Mesurements.RemoveRange(measurement);
                db.SaveChanges();

                var saleorderheader = (from a in db.SaleOrderHeaders.AsEnumerable() where a.DocEntry == saleordeparentdetails.saleorderNo select a).FirstOrDefault();
                saleorderheader.doctotal = Math.Round((saleorderheader.doctotal - saleordeparentdetails.TotalAmount)??0,0);
                saleorderheader.totalBeorerDiscount=Math.Round((saleorderheader.totalBeorerDiscount- saleordeparentdetails.TotalAmount)??0,0);
                saleorderheader.totalAfterDiscount =Math.Round((saleorderheader.totalBeorerDiscount * ((100 - saleorderheader.Discount) / 100))??0,0);
                saleorderheader.BalanceAmount = Math.Round((saleorderheader.totalAfterDiscount - saleorderheader.Advance) ??0, 0);
                db.Entry(saleorderheader).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
              //  var discount = Math.Round(saleorderheader.Discount ?? 0, 0);

                return Json(new { value = "Success" }, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                return Json(new { value = "Fail" }, JsonRequestBehavior.AllowGet);
            }


        }

        public JsonResult getpercentage(string fromdate,string todate,string lineid,string Nodays)
        {

            DateTime datefrom = DateTime.ParseExact(fromdate, "dd/MM/yyyy", null);
            DateTime dateto = DateTime.ParseExact(todate, "dd/MM/yyyy", null);
            int id = 0;
            decimal lineunitprice = 0;
            decimal totalline = 0;
            while (datefrom<dateto)
            {
                datefrom = datefrom.AddDays(1);
                if(datefrom.DayOfWeek != DayOfWeek.Sunday)
                {
                    id = id + 1;            
                }

            }
            //double totaldays = (dateto - datefrom).TotalDays*5;
            //double weekno = (datefrom.DayOfWeek - dateto.DayOfWeek)*2;
            //double calcBusinessDays =1 +( (totaldays-weekno) / 7);
            //if (dateto.DayOfWeek == DayOfWeek.Saturday)
            //    calcBusinessDays--;
            //if (datefrom.DayOfWeek == DayOfWeek.Sunday)
            //    calcBusinessDays –;
            var percentage = "0";
            decimal headamount = 0;
            decimal total = 0;
           // if (id < int.Parse(Nodays))
         //   {
                int x=0;
                percentage = (from a in db.C_DAYS_TARIFF.AsEnumerable() where int.Parse(a.U_FromDays) <= id && int.Parse(a.U_ToDays) >= id select a.U_Percent).FirstOrDefault();

                if (int.TryParse(percentage, out x))
                {

                }
                    var item = (from a in db.SaleOrderDetails.AsEnumerable() where a.Id == int.Parse(lineid) select a).FirstOrDefault();
                    var saleorderhead = (from a in db.SaleOrderHeaders where a.DocEntry == item.saleorderNo select a).FirstOrDefault();
                    decimal unitprice = (from a in db.ITM1 where a.ItemCode == item.ItemCode where a.PriceList == 1 select a.Price).FirstOrDefault()??0;
                    item.TotalAmount = (from a in db.SaleOrderDetails.AsEnumerable() where a.parentId == lineid select a.TotalAmount).Sum()+ unitprice;
                    decimal amount = ((item.TotalAmount??0) * x / 100);
                    int increaseamount = int.Parse(Math.Round(amount, 0).ToString());
                    item.TotalAmount = Math.Round((item.TotalAmount + increaseamount) ?? 0, 0);
                    totalline = Math.Round(item.TotalAmount??0, 0);
                    item.UnitPrice =Math.Round((unitprice + increaseamount),0);
                    lineunitprice = Math.Round((unitprice + increaseamount), 0);
                    item.Deliverydate = todate;
                    db.Entry(item).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    headamount = (from a in db.SaleOrderDetails where a.saleorderNo == item.saleorderNo && a.parentId == "0" select a.TotalAmount).Sum()??0;
                    saleorderhead.totalAfterDiscount = headamount;
                    saleorderhead.totalBeorerDiscount = headamount;
                    total= Math.Round(headamount, 0);
                    db.Entry(saleorderhead).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                 // percentage = increaseamount.ToString();
                //  }
            return Json(new { peramount = lineunitprice, linetotal = totalline, totalamount= total }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AllowAnonymous]
        [ActionName("SaleOrder")]
        [onAction(ButtonName = "ClearItems")]

        public ActionResult ClearAllItems(saleorder objsaleorder)
        {
            if(objsaleorder.objlistsalDetails!=null)
            {

                var saleorderdetails = (from a in db.SaleOrderDetails where a.saleorderNo == objsaleorder.objsaleorderHeader.DocEntry select a).ToList();
                var measurements = (from a in db.Mesurements.AsEnumerable() where saleorderdetails.Select(b => b.Id).Contains(int.Parse(a.LineId)) select a).ToList();
                db.Mesurements.RemoveRange(measurements);
                db.SaleOrderDetails.RemoveRange(saleorderdetails);
                objsaleorder.objsaleorderHeader.cash = null;
                objsaleorder.objsaleorderHeader.card = null;
                objsaleorder.objsaleorderHeader.carddetails = "";
                objsaleorder.objsaleorderHeader.OtherPayments = null;
                objsaleorder.objsaleorderHeader.OthpayDetails = "";
                objsaleorder.objsaleorderHeader.Advance = null;
                objsaleorder.objsaleorderHeader.BalanceAmount = 0;
                objsaleorder.objsaleorderHeader.Discount = 0;
                objsaleorder.objsaleorderHeader.doctotal = 0;
                objsaleorder.objsaleorderHeader.totalAfterDiscount = 0;
                objsaleorder.objsaleorderHeader.totalBeorerDiscount = 0;
                objsaleorder.objsaleorderHeader.totaltax = 0;
                db.Entry(objsaleorder.objsaleorderHeader).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
               
            }

            return RedirectToAction("SaleOrder", "SaleOrder");
        }


        [HttpPost]
        [AllowAnonymous]
        [ActionName("SaleOrder")]
        [onAction(ButtonName = "FSendMail")]

        public ActionResult FSendemail(saleorder objsaleorder)
        {
            string Toaddress = (from a in db.OCRDs.AsEnumerable() where a.CardCode == objsaleorder.objsaleorderHeader.CardCode select a.E_Mail).FirstOrDefault();
            string cname = (from a in db.OCRDs.AsEnumerable() where a.CardCode == objsaleorder.objsaleorderHeader.CardCode select a.CardName).FirstOrDefault();
            return ComnSendMail(objsaleorder, cname, Toaddress,"N");
        }


        [HttpPost]
        [AllowAnonymous]
        [ActionName("SaleOrder")]
        [onAction(ButtonName = "SendMail")]

        public ActionResult Sendemail(saleorder objsaleorder)
        {
            string Toaddress = (from a in db.OCRDs.AsEnumerable() where a.CardCode == TempData["Cardcode"].ToString() select a.E_Mail).FirstOrDefault();
            string cname = (from a in db.OCRDs.AsEnumerable() where a.CardCode == TempData["Cardcode"].ToString() select a.CardName).FirstOrDefault();
          
            return ComnSendMail(objsaleorder, cname, Toaddress,"Y");



        }

        public void Softcopysale(saleorder objsaleorder)
        {
            LocalReport lr = new LocalReport();
            if (objsaleorder.objsaleorderHeader.Series == 1)
            {
                TempData.Remove("saleorder");

            }

            string path = Path.Combine(Server.MapPath("~/DarzanReport/Saleordersoft.rdlc"));
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                lr.ReportPath = path;
            }
            string websale = objsaleorder.objsaleorderHeader.DocEntry;

            using (db = new DARZANTESTEntities())
            {


                List<Saleorderreportchild> orderlist = new List<Saleorderreportchild>();

                SaleorderreportHead headprint = new SaleorderreportHead();

                headprint = (from a in db.ORDRs.AsEnumerable()
                             join b in db.RDR1.AsEnumerable() on a.DocEntry equals b.DocEntry
                             where a.U_WebSaleOrder == websale
                             select new SaleorderreportHead
                             {
                                 U_DelDate = (from g in db.ORDRs.AsEnumerable()
                                              join h in db.RDR1.AsEnumerable() on a.DocEntry equals b.DocEntry
                                              orderby h.U_DelDate descending
                                              where a.U_WebSaleOrder == websale
                                              select (h.U_DelDate ?? DateTime.Now).ToString("dd/MM/yyyy")).FirstOrDefault(),
                                 CardName = a.CardName,
                                 DocNum = a.DocNum,
                                 Cellular = (from c in db.OCRDs where a.CardCode == c.CardCode select c.Cellular).FirstOrDefault(),
                                 CardCode = a.CardCode,
                                 Street = (from c in db.CRD1 where c.CardCode == a.CardCode && c.AdresType == "S" select c.Street).ToList().FirstOrDefault(),

                                 Block = (from c in db.CRD1.AsEnumerable() where c.CardCode == a.CardCode where c.AdresType == "S" select (c.Block ?? "").ToString() + "," + (c.Building ?? "").ToString()).ToList().FirstOrDefault(),
                                 DocDueDate = (from c in db.ORDRs.AsEnumerable() where c.DocEntry == b.BaseEntry select (c.DocDate ?? DateTime.Now).ToString("dd/MM/yyyy")).FirstOrDefault(),
                                 ItmsGrpNam = ((from d in db.OITMs join e in db.OITBs on d.ItmsGrpCod equals e.ItmsGrpCod where d.ItemCode == b.ItemCode select e.ItmsGrpNam).FirstOrDefault()),
                                 TotalBeforeDiscount = (a.DocTotal ?? 0 - a.VatSum ?? 0 - a.DiscSum ?? 0),
                                 DiscPrcnt = a.DiscPrcnt,
                                 VatSum = a.VatSum,
                                 DocTotal = a.DocTotal,
                                 DiscSum = a.DiscSum,
                                 U_Adv = a.U_Adv ?? 0,
                                 DocDate = (a.DocDate ?? DateTime.Now).ToString("dd/MM/yyyy"),
                                 //  U_DelDate = (b.U_DelDate ?? DateTime.Now).ToString("dd/MM/yyyy"),
                                 U_WebSaleOrder = a.U_WebSaleOrder,
                                 Address = (from d in db.OWHS.AsEnumerable() where b.WhsCode == d.WhsCode select (d.Street ?? "").ToString() + "," + (d.StreetNo ?? "").ToString()).ToList().FirstOrDefault(),
                                 County = (from d in db.OWHS where b.WhsCode == d.WhsCode select d.County).ToList().FirstOrDefault()

                             }).FirstOrDefault();


                List<Saleorderreportchild> objchild = (from a in db.ORDRs.AsEnumerable()
                                                       join b in db.RDR1.AsEnumerable() on a.DocEntry equals b.DocEntry
                                                       where a.U_WebSaleOrder == websale && b.U_ParentItem == "0"
                                                       select new Saleorderreportchild
                                                       {

                                                           newlinetotal = (from rr in db.RDR1 where rr.U_ParentItem == b.U_MPI select rr.LineTotal ?? 0).ToList(),
                                                           newDscription = (from rr in db.RDR1 where rr.U_ParentItem == b.U_MPI select "(" + rr.Dscription + ")").ToList(),
                                                           Dscription = (b.Dscription),
                                                           Quantity = b.Quantity,
                                                           Price = b.Price,
                                                           LineTotal = b.LineTotal,

                                                           LineNum = b.LineNum,
                                                           U_MPI = b.U_MPI,
                                                           DocDate = (a.DocDate ?? DateTime.Now).ToString("dd/MM/yyyy"),
                                                           U_DelDate = (b.U_DelDate ?? DateTime.Now).ToString("dd/MM/yyyy"),

                                                       }).ToList();


                foreach (Saleorderreportchild objresult in objchild)
                {





                    for (int i = 0; i < objresult.newDscription.Count; i++)
                    {

                        objresult.Dscription = objresult.Dscription + objresult.newDscription[i];
                    }


                    for (int i = 0; i < objresult.newlinetotal.Count; i++)
                    {
                        objresult.LineTotal = objresult.LineTotal + objresult.newlinetotal[i];

                    }


                }


                orderlist.AddRange(objchild);






                //foreach(var item in objchild)
                //{
                //    List<Saleorderreportchild> objdsc =(from rr in db.RDR1  where rr.U_ParentItem == item.U_MPI select new Saleorderreportchild { Dscription =item.Dscription+","+ rr.Dscription }).ToList();

                //    orderlist.AddRange(objdsc);
                //}
                // orderlist.AddRange(objchild);
                List<SaleorderreportHead> objheaderlist = new List<SaleorderreportHead>();
                objheaderlist.Add(headprint);

                //headprint.outletfrom = orderuserdeatails.objsaleorderHeader.Outletfrom;
                //headprint.outletto = orderuserdeatails.objsaleorderHeader.outletto;
                //headprint.Username = orderuserdeatails.objsaleorderHeader.UserID;
                //// headprint.Outletfrom = orderuserdeatails.objsaleorderHeader.Outletfrom;
                //headprint.Pickupuser = orderuserdeatails.objsaleorderHeader.pickupuser;
                //headprint.Tno = orderuserdeatails.objsaleorderHeader.Tno;
                //headprint.Tstatus = orderuserdeatails.objsaleorderHeader.Tstatus;
                //List<LogisticPrintHead> objheaderlist = new List<LogisticPrintHead>();
                //objheaderlist.Add(headprint);
                //foreach (var item in orderuserdeatails.objchild)
                //{
                //    List<Logisticprint> objchild = (from a in db.LabelDetails.AsEnumerable() where a.DocNum == item.Orderno select new Logisticprint { DocNum = a.DocNum, MainQty = a.MainQty, AddonQty = a.AddonQty, Fabric = a.Fabric, LabelNo = a.LabelNo }).ToList();

                //    orderlist.AddRange(objchild);
                //}
                //   int dno =Convert.ToInt32(TempData["delno"]);






                //LogisticPrintHead headprint = new LogisticPrintHead();
                //var orderlist = (from a in db.ORDRs
                //                 join b in db.RDR1 on a.DocEntry equals b.DocEntry
                //                 where a.U_WebSaleOrder == websale
                //                 select new
                //                 {
                //                     CardName = a.CardName,
                //                     DocNum = a.DocNum,
                //                     //CardCode = (from c in db.OCRDs where a.CardCode == c.CardCode select c.CardCode).FirstOrDefault(),
                //                     CardCode = a.CardCode,
                //                     ItmsGrpNam = ((from d in db.OITMs join e in db.OITBs on d.ItmsGrpCod equals e.ItmsGrpCod where d.ItemCode == b.ItemCode select e.ItmsGrpNam).FirstOrDefault()),

                //                     //Address = (from d in db.CRD1 where a.CardCode == d.CardCode && d.AdresType == "B" select d.Address + " " + d.Street + " " + d.City + " " + d.ZipCode).FirstOrDefault(),
                //                     b.Dscription,
                //                     b.Quantity,
                //                     Price = b.Price,
                //                     b.LineTotal,
                //                     TotalBeforeDiscount = (a.DocTotal ?? 0 - a.VatSum ?? 0 - a.DiscSum ?? 0),
                //                     a.DiscPrcnt,
                //                     a.VatSum,
                //                     a.DocTotal,
                //                     a.DiscSum,
                //                     a.DocDate,
                //                     b.U_DelDate,
                //                     b.U_ItemG,
                //                   //  Linenumber = (b.LineNum + a.DocNum + b.DocEntry),
                //                     b.DocEntry,
                //                     b.LineNum
                //                 }).ToList().Distinct();
                ReportDataSource rd = new ReportDataSource("Ordr", objheaderlist);
                lr.DataSources.Add(rd);
                //**********************************************************

                ReportDataSource rd1 = new ReportDataSource("Rdr1", orderlist);
                lr.DataSources.Add(rd1);


                ReportDataSource rd2 = new ReportDataSource("Ocrd", objheaderlist);
                lr.DataSources.Add(rd2);

                ReportDataSource rd3 = new ReportDataSource("Oitm", objheaderlist);
                lr.DataSources.Add(rd3);

                ReportDataSource rd4 = new ReportDataSource("Oitb", objheaderlist);
                lr.DataSources.Add(rd4);
                ReportDataSource rd5 = new ReportDataSource("Crd1", objheaderlist);
                lr.DataSources.Add(rd5);


                string reporType = "PDF";
                string mimetypr;
                string encodeing;
                string filenameexns;
                string deviceinfo = "<DeviceInfo>" + "<OutputFormat>PDF</OutputFormat>" +
                    "<PageWidth>7.5in</PageWidth>" + "<PageHeight>8.5in</PageHeight>" +
                    "<MarginTop>0.5in</MarginTop>" + "<MarginLeft>lin</MarginLeft>" + "<MarginRight>0in</MarginRight>"
                    + "<MarginButtom>0.5in</MarginButtom>" + "</DeviceInfo>";
                Warning[] warnings;
                string[] streams;
                byte[] renderedbytes;
                //.ProcessingMode = ProcessingMode.Local;
                renderedbytes = lr.Render(reporType, deviceinfo, out mimetypr, out encodeing, out filenameexns, out streams, out warnings);
                if (System.IO.File.Exists(Server.MapPath("~/Documents/Saleorder.pdf")))
                {
                    System.IO.File.Delete(Server.MapPath("~/Documents/Saleorder.pdf"));
                }

                FileStream fileStream = new FileStream(Server.MapPath("~/Documents/Saleorder.pdf"), FileMode.Create);

                for (int i = 0; i < renderedbytes.Length; i++)
                {
                    fileStream.WriteByte(renderedbytes[i]);
                }
                fileStream.Close();
            }
           // return RedirectToAction("Sendemail", "Invoice");


        }


        public ActionResult ComnSendMail(saleorder objsaleorder,string cname,string Toaddress,string check)
        {
            Softcopysale(objsaleorder);

            if (Toaddress != "")
            {
                string mail = "confirmation@darzan.in";
                //string mail = "nhcl143@gmail.com";
                if ((mail == "") || (mail == "NULL"))
                {
                    mail = "confirmation@darzan.in";
                    //email = "hrms@reliabilityengineering.in";
                }


                //  string combine;
                string pwd = "xypiqzyshpmikyek";
                //string pwd = "srini@123";

                MailMessage message = new MailMessage();
                SmtpClient smtpClient = new SmtpClient();
                //mail = ViewState["Email"].ToString() ;
                // Set the sender's address

                MailAddress fromAddress = new MailAddress(mail);
                message.From = fromAddress;



                //string Toaddress = "phstmngr@induscancer.com";
                message.Attachments.Add(new Attachment(Server.MapPath("~/Documents/Saleorder.pdf")));
                message.To.Add(new MailAddress(Toaddress));


                message.Subject = "Order Confirmation Email";
                message.IsBodyHtml = true;
                //string body = "Leave";
                string body = "<div style='border:3px #468dc5 double; width:700px; font:12px Helvetica Neue, Helvetica, sans-serif,Arial'>";
                body = body + "<div style='padding:10px'>";
                body = body + "Dear " + cname + ",<br /><br />";
                body = body + "Thank You For Order We Will be touch via Email & SMS once we are able to Confirm it.<br /><br />";
                message.Body = body;

                // mm.IsBodyHtml = false;
                // Set the SMTP server to be used to send the message

                SmtpClient client = new SmtpClient();
                client.EnableSsl = true;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(mail, pwd);
                client.Host = "smtp.gmail.com";
                client.Port = 587;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.Send(message);



            }
            if(check=="Y")
            TempData["saleordersuccess"] = "Sucess";

            TempData["saleorder"] = objsaleorder;
            return Content("<script language='javascript' type='text/javascript'>alert('Your message has been successfully sent.');window.location ='../SaleOrder/SaleOrder';</script>");

        }


        [HttpPost]
        [AllowAnonymous]
        [ActionName("SaleOrder")]
        [onAction(ButtonName = "Clear")]
        public ActionResult clearsession()
        {
          
            return RedirectToAction("Saleorder", "Saleorder");
        }

        [HttpPost]
        [AllowAnonymous]
        [ActionName("SaleOrder")]
        [onAction(ButtonName = "close")]
        public ActionResult CloseSaleorder()
        {
            TempData.Clear();
            return RedirectToAction("Saleorder", "Saleorder");
        }

    }
}