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

namespace DarZon.Controllers
{
    public class InvoiceController : Controller
    {
        // GET: Invoice
        DARZANTESTEntities db = new DARZANTESTEntities();
       // [UserAuthenticationFilters]
        public ActionResult Invoice()
        {
            if (TempData["InvSuccess"] != null)

                ViewBag.popalt = (string)TempData["InvSuccess"];
            else
                ViewBag.popalt = "";
           Invoicemodel objinvhead = new Invoicemodel();
            objinvhead.invhead = new InvoiceHead();
            objinvhead.invhead.UserId = Session["UserName"].ToString();
            objinvhead.objinvlist = new List<Invoicechild>();
            
          if(  TempData["cno"]!=null)
            {
                string tno = (string)TempData["cno"];
                TempData["cno"] = tno;
            }
            
            if (TempData["InvoiceObj"]!=null)
            {
                objinvhead = (Invoicemodel)TempData["InvoiceObj"];
            }

            objinvhead.invhead.DeliveryDate = DateTime.Now.ToString("dd/MM/yyyy");
            return View(objinvhead);
        }
        public ActionResult Deliverylist()
        {
            var list = (from b in db.RDR1 where (b.U_DSTS != "Yes" || b.U_DSTS == null) && b.U_ParentItem == "0" select b.DocEntry).ToList();
            var orderlist = (from a in db.ORDRs.AsEnumerable() where !(list).Contains(a.DocEntry) && a.DocStatus == "O" select new InvoiceHead { websaleorder = a.U_WebSaleOrder, DeliveryNo = a.DocNum.ToString(), DeliveryDate = (a.DocDate??DateTime.Now).ToString("dd/MM/yyyy"), CustomerName = a.CardName, CustomerNo = a.CardCode, DocTotal = Math.Round(a.DocTotal ?? 0, 0), TotalBefDis = Math.Round(((a.DocTotal ?? 0) - ((a.VatSum ?? 0) + (a.DiscSum ?? 0))), 0), Tax = Math.Round(a.VatSum ?? 0, 0), Advance = (from x in db.ORCTs where x.U_SAPSO == a.DocNum.ToString() && x.PayNoDoc == "Y" && x.DocType == "C" && x.U_SONo == a.U_WebSaleOrder select x.NoDocSum).Sum() }).ToList();
            return View(orderlist);
        }

        public ActionResult Invoicelist()
        {
            var orderlist = (from a in db.OINVs.AsEnumerable() select new InvoiceHead { websaleorder = a.U_WebSaleOrder, InvoiceNo = a.DocNum.ToString(), DeliveryDate = (a.DocDate ?? DateTime.Now).ToString("dd/MM/yyyy"), CustomerName = a.CardName, CustomerNo = a.CardCode, DocTotal = Math.Round(a.DocTotal ?? 0, 0), TotalBefDis = Math.Round(((a.DocTotal ?? 0) - ((a.VatSum ?? 0) + (a.DiscSum ?? 0))), 0), Tax = Math.Round(a.VatSum ?? 0, 0), DeliveryNo = a.NumAtCard, Advance = Math.Round(a.U_Adv ?? 0, 0), CurrentAdvance = Math.Round(a.U_BAmt ?? 0, 0),discount= Math.Round(a.DiscPrcnt??0,0) }).ToList();
            return View(orderlist);
        }

        public ActionResult invoicedetails(string InvNo)
        {
            TempData["delno"] = InvNo;
            List<Invoicechild> objinvlist = new List<Invoicechild>();

            try
            {
                objinvlist = (from a in db.OINVs.AsEnumerable() join b in db.INV1.AsEnumerable() on a.DocEntry equals b.DocEntry where a.DocNum == int.Parse(InvNo) && b.U_ParentItem == "0" select new Invoicechild { ItemCode = b.ItemCode, Total = Math.Round((from x in db.INV1 where x.U_ParentItem == b.U_MPI || x.U_MPI == b.U_MPI select x.LineTotal ?? 0).Sum(), 0), description = b.Dscription, qty = int.Parse(decimal.Round(Math.Round(b.Quantity ?? 0, 0), 0).ToString()), Rate = Convert.ToDecimal(Math.Round(b.Price ?? 0, 0)), Tax = b.TaxCode ?? "0", websaleorder = a.U_WebSaleOrder }).ToList();
                //TempData["delno"] = deliveryno;
            }
            catch (Exception ex)
            {

            }
            return PartialView(objinvlist);

        }




        public ActionResult invdetails(string deliveryno)
        {
            // TempData["delno"] = deliveryno;
            List<Invoicechild> objinvlist = new List<Invoicechild>();

            try
            {
                objinvlist = (from a in db.ORDRs.AsEnumerable() join b in db.RDR1.AsEnumerable() on a.DocEntry equals b.DocEntry where b.U_ParentItem == "0" && a.DocNum == int.Parse(deliveryno) select new Invoicechild { websaleorder = a.U_WebSaleOrder, ItemCode = b.ItemCode, Total = Math.Round((from x in db.RDR1 where x.U_ParentItem == b.U_MPI || x.U_MPI == b.U_MPI select Math.Round(x.LineTotal ?? 0, 0)).Sum(), 0), description = b.Dscription, qty = int.Parse(decimal.Round(Math.Round(b.Quantity ?? 0, 0), 0).ToString()), Rate = Convert.ToDecimal(Math.Round(b.Price ?? 0, 0)), Tax = b.TaxCode ?? "0", Line = b.U_ParentItem == "0" ? b.U_MPI : b.U_ParentItem }).ToList();
                //TempData["delno"] = deliveryno;
            }
            catch (Exception ex)
            {

            }
            return PartialView(objinvlist);

        }
        [HttpPost]
        [AllowAnonymous]
        [ActionName("Invoice")]
        [onAction(ButtonName = "Save")]
        public ActionResult savedetails(Invoicemodel objinvhead)
        {
            // return RedirectToAction("PublicAreaReport", "Invoice");
            //return View();
           // TempData["delno"] = objinvhead.invhead.InvoiceNo;
            List<Invoicechild> objinvlist = new List<Invoicechild>();
            TempData["cname"] = objinvhead.invhead.CustomerName;
            TempData["cno"] = objinvhead.invhead.CustomerNo;
            objinvlist = (from a in db.ORDRs.AsEnumerable() join b in db.RDR1.AsEnumerable() on a.DocEntry equals b.DocEntry where a.DocNum == int.Parse(objinvhead.invhead.DeliveryNo) select new Invoicechild { ItemCode = b.ItemCode, Total = Math.Round(b.LineTotal ?? 0, 0), description = b.Dscription, qty = int.Parse(decimal.Round(Math.Round(b.Quantity ?? 0, 0), 0).ToString()), Rate = Convert.ToDecimal(Math.Round(b.Price ?? 0, 0)), Tax = b.TaxCode ?? "0", Line = b.U_ParentItem == "0" ? b.U_MPI : b.U_ParentItem }).ToList();
            objinvhead.objinvlist = new List<Invoicechild>();
            objinvhead.objinvlist = objinvlist;
            //  return Content("<script language='javascript' type='text/javascript'>alert('Invoice details Submitted Successfully!');window.location ='../Invoice/Sendemail';</script>");
            TempData["InvSuccess"] = "Success";
            SAPIntegration objsapint = new SAPIntegration();
            objinvhead.invhead.WHCODE = (string)Session["WhareHouse"];
            string sapmsg = objsapint.SalesInvoice(objinvhead);
            string message = "";
            if (sapmsg.Length > 20)
            {
                message = sapmsg.Substring(0, 19); ;

            }


            objinvhead.invhead.InvoiceNo = (from a in db.OINVs.AsEnumerable() join b in db.INV1.AsEnumerable() on a.DocEntry equals b.DocEntry where b.BaseEntry ==int.Parse(objinvhead.invhead.DeliveryNo) select a.DocNum).FirstOrDefault().ToString();
            TempData["InvoiceObj"] = objinvhead;
            // var invno=(from a in db.INV1 where )
           
            if (sapmsg == "Success")
            {
                TempData["InvSuccess"] = "Success";
                
                return Content("<script language='javascript' type='text/javascript'>alert('Invoice details Submitted Successfully!');window.location ='../Invoice/Invoice';</script>");

            }
            else
            {

               
                 return Content("<script language='javascript' type='text/javascript'>alert('" + message + "');window.location ='../Invoice/Invoice';</script>");

               // return Content("<script language='javascript' type='text/javascript'>alert('Invoice details Submitted Successfully!');window.location ='../Invoice/Invoice';</script>");



            }



        }
        [HttpPost]
        [AllowAnonymous]
        [ActionName("Invoice")]
        [onAction(ButtonName = "Print")]
        public ActionResult Print(Invoicemodel objinvhead)
        {
            TempData["delno"] = objinvhead.invhead.InvoiceNo;
            TempData["cname"] = objinvhead.invhead.CustomerName;
            TempData["cno"] = objinvhead.invhead.CustomerNo;
            TempData["InvoiceObj"] = objinvhead;
            TempData["InvSuccess"] = "Success";
            return RedirectToAction("Invoicehardcopy", "Invoice");

            // return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ActionName("Invoice")]
        [onAction(ButtonName = "Softcopy")]
        public ActionResult InvoiceReport(Invoicemodel objinvhead)
        {
            return ComnInvoiceReport(objinvhead);
        }
        [HttpPost]
        [AllowAnonymous]
        [ActionName("Invoice")]
        [onAction(ButtonName = "FSoftcopy")]
        public ActionResult FInvoiceReport(Invoicemodel objinvhead)
        {
            return ComnInvoiceReport(objinvhead);
        }

        public ActionResult ComnInvoiceReport(Invoicemodel objinvhead)
        {
            TempData["delno"] = objinvhead.invhead.InvoiceNo;
            TempData["cname"] = objinvhead.invhead.CustomerName;
            TempData["cno"] = objinvhead.invhead.CustomerNo;

            TempData["InvoiceObj"] = objinvhead;
            LocalReport lr = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/DarzanReport/Invoice.rdlc"));
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                lr.ReportPath = path;
            }
            List<Invoicechild> objinvlist = new List<Invoicechild>();

            using (db = new DARZANTESTEntities())
            {

                int dno = Convert.ToInt32(TempData["delno"]);
                //var orderlist = (from a in db.OINVs
                //                 join b in db.INV1 on a.DocEntry equals b.DocEntry
                //                 where a.DocNum == dno
                //                 select new
                //                 {
                //                     CardName = a.CardName,
                //                     DocNum = a.DocNum,
                //                     Cellular = (from c in db.OCRDs where a.CardCode == c.CardCode select c.Cellular).FirstOrDefault(),
                //                     Address = (from d in db.CRD1 where a.CardCode == d.CardCode && d.AdresType == "B" select d.Address).FirstOrDefault(),
                //                     //Address = (from d in db.CRD1 where a.CardCode == d.CardCode && d.AdresType == "B" select d.Address + " " + d.Street + " " + d.City + " " + d.ZipCode).FirstOrDefault(),


                //                     b.Dscription,
                //                     b.Quantity,
                //                     Price = b.Price,
                //                     b.LineTotal,
                //                     TotalBeforeDiscount = (a.DocTotal ?? 0 - a.VatSum ?? 0 - a.DiscSum ?? 0),
                //                     a.DiscPrcnt,
                //                     a.VatSum,

                //                     a.DocTotal,
                //                     a.DocDate,
                //                     a.DiscSum
                //                 }).ToList();



                //**********************************************

                List<Saleorderreportchild> orderlist = new List<Saleorderreportchild>();

                SaleorderreportHead headprint = new SaleorderreportHead();
                headprint = (from a in db.OINVs.AsEnumerable()

                             join b in db.INV1.AsEnumerable() on a.DocEntry equals b.DocEntry
                             where a.DocNum == dno
                             select new SaleorderreportHead
                             {
                                 CardName = a.CardName,
                                 DocNum = a.DocNum,
                                 Cellular = (from c in db.OCRDs where a.CardCode == c.CardCode select c.Cellular).FirstOrDefault(),

                                 Street = (from c in db.CRD1 where c.CardCode == a.CardCode && c.AdresType == "S" select c.Street).ToList().FirstOrDefault(),

                                 Block = (from c in db.CRD1.AsEnumerable() where c.CardCode == a.CardCode where c.AdresType == "S" select (c.Block ?? "").ToString() + "," + (c.Building ?? "").ToString()).ToList().FirstOrDefault(),
                                 DocDueDate = (from c in db.ORDRs.AsEnumerable() where c.DocEntry == b.BaseEntry select (c.DocDate ?? DateTime.Now).ToString("dd/MM/yyyy")).FirstOrDefault(),
                                 CardCode = a.CardCode,
                                 ItmsGrpNam = ((from d in db.OITMs join e in db.OITBs on d.ItmsGrpCod equals e.ItmsGrpCod where d.ItemCode == b.ItemCode select e.ItmsGrpNam).FirstOrDefault()),
                                 TotalBeforeDiscount = (a.DocTotal ?? 0 - a.VatSum ?? 0 - a.DiscSum ?? 0),
                                 DiscPrcnt = a.DiscPrcnt,
                                 VatSum = a.VatSum,
                                 DocTotal = a.DocTotal,
                                 DiscSum = a.DiscSum,
                                 DocDate = (a.DocDate ?? DateTime.Now).ToString("dd/MM/yyyy"),
                                 U_DelDate = (b.U_DelDate ?? DateTime.Now).ToString("dd/MM/yyyy"),
                                 U_WebSaleOrder = a.U_WebSaleOrder,
                                 U_Advance = a.U_Adv,
                                 Address = (from d in db.OWHS.AsEnumerable() where b.WhsCode == d.WhsCode select (d.Street ?? "").ToString() + "," + (d.StreetNo ?? "").ToString()).ToList().FirstOrDefault(),
                                 County = (from d in db.OWHS where b.WhsCode == d.WhsCode select d.County).ToList().FirstOrDefault()
                             }).FirstOrDefault();


                List<Saleorderreportchild> objchild = (from a in db.OINVs.AsEnumerable()
                                                       join b in db.INV1.AsEnumerable() on a.DocEntry equals b.DocEntry
                                                       where a.DocNum == dno && b.U_ParentItem == "0"
                                                       select new Saleorderreportchild
                                                       {

                                                           newlinetotal = (from rr in db.INV1 where rr.U_ParentItem == b.U_MPI select rr.LineTotal ?? 0).ToList(),
                                                           newDscription = (from rr in db.INV1 where rr.U_ParentItem == b.U_MPI select "(" + rr.Dscription + ")").ToList(),
                                                           Dscription = b.Dscription,
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


                    //for (int i = 0; i < objresult.newlinetotal.Count; i++)
                    //{
                    //    //objresult.LineTotal = objresult.LineTotal + objresult.newlinetotal[i];

                    //}
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



                //*****************************************

                ReportDataSource rd = new ReportDataSource("Oinv", objheaderlist);
                lr.DataSources.Add(rd);
                //**********************************************************



                //*******************************************

                ReportDataSource rd1 = new ReportDataSource("Inv1", orderlist);
                lr.DataSources.Add(rd1);
                ReportDataSource rd2 = new ReportDataSource("RDR1", orderlist);
                lr.DataSources.Add(rd1);
                TempData["delno"] = dno.ToString();
                //************************************************************

                ReportDataSource rd3 = new ReportDataSource("Ocrd", objheaderlist);
                lr.DataSources.Add(rd3);
                ReportDataSource rd4 = new ReportDataSource("Crd1", objheaderlist);
                lr.DataSources.Add(rd4);
                TempData["objheaderlist"] = objheaderlist;
                TempData["orderlist"] = orderlist;
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
                renderedbytes = lr.Render(reporType, deviceinfo, out mimetypr, out encodeing, out filenameexns, out streams, out warnings);

                //return File(renderedbytes, mimetypr);
                // byte[] bytes = lr.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamids, out warnings);

                // CODE TO SAVE THE REPORT FILE ON SERVER
                if (System.IO.File.Exists(Server.MapPath("~/Documents/Invoice.pdf")))
                {
                    System.IO.File.Delete(Server.MapPath("~/Documents/Invoice.pdf"));
                }

                FileStream fileStream = new FileStream(Server.MapPath("~/Documents/Invoice.pdf"), FileMode.Create);

                for (int i = 0; i < renderedbytes.Length; i++)
                {
                    fileStream.WriteByte(renderedbytes[i]);
                }
                fileStream.Close();
            }
            return RedirectToAction("Sendemail", "Invoice");


        }

        public ActionResult Sendemail()
        {
            Invoicemodel objinvhead = new Invoicemodel();
            if (TempData["InvoiceObj"] !=null)
            {
               objinvhead=(Invoicemodel)TempData["InvoiceObj"];
            }
            string cno = TempData["cno"].ToString().Trim();
            var Toaddress = (from a in db.OCRDs.AsEnumerable() where a.CardCode == cno select a.E_Mail).FirstOrDefault();
            //  var Toaddress = "anilkumarch@nhclindia.com";
            if (Toaddress != null)
            {
                string mail = "confirmation@darzan.in";

                if ((mail == "") || (mail == "NULL"))
                {
                    mail = "confirmation@darzan.in";
                    //email = "hrms@reliabilityengineering.in";
                }


                //  string combine;
                string pwd = "xypiqzyshpmikyek";

                MailMessage message = new MailMessage();
                SmtpClient smtpClient = new SmtpClient();
                //mail = ViewState["Email"].ToString() ;
                // Set the sender's address

                MailAddress fromAddress = new MailAddress(mail);
                message.From = fromAddress;


                //string Toaddress = "phstmngr@induscancer.com";

                message.To.Add(new MailAddress(Toaddress));

                message.Attachments.Add(new Attachment(Server.MapPath("~/Documents/Invoice.pdf")));
                message.Subject = "Payment Confirmation Email";
                message.IsBodyHtml = true;
                //string body = "Leave";
                string body = "<div style='border:3px #468dc5 double; width:700px; font:12px Helvetica Neue, Helvetica, sans-serif,Arial'>";
                body = body + "<div style='padding:10px'>";
                body = body + "Dear " + TempData["cname"].ToString() + ",<br /><br />";
                body = body + "Thank you for doing business with us  Please let us know if you have any queries. <br />please Find attached invoice copy for your reference<br />";
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
            TempData["cno"] = cno;

            TempData["InvoiceObj"] = objinvhead;
            TempData["InvSuccess"] = "Success";
            return RedirectToAction("Invoice", "Invoice");


        }


        public ActionResult Invoicehardcopy()
        {


            LocalReport lr = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/DarzanReport/InvoiceHardcopy.rdlc"));
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                lr.ReportPath = path;
            }
            List<Invoicechild> objinvlist = new List<Invoicechild>();

            using (db = new DARZANTESTEntities())
            {

                int dno = Convert.ToInt32(TempData["delno"]);
                //var orderlist = (from a in db.OINVs
                //                 join b in db.INV1 on a.DocEntry equals b.DocEntry
                //                 where a.DocNum == dno
                //                 select new
                //                 {
                //                     CardName = a.CardName,
                //                     DocNum = a.DocNum,
                //                     Cellular = (from c in db.OCRDs where a.CardCode == c.CardCode select c.Cellular).FirstOrDefault(),
                //                     Address = (from d in db.CRD1 where a.CardCode == d.CardCode && d.AdresType == "B" select d.Address).FirstOrDefault(),
                //                     //Address = (from d in db.CRD1 where a.CardCode == d.CardCode && d.AdresType == "B" select d.Address + " " + d.Street + " " + d.City + " " + d.ZipCode).FirstOrDefault(),


                //                     b.Dscription,
                //                     b.Quantity,
                //                     Price = b.Price,
                //                     b.LineTotal,
                //                     TotalBeforeDiscount = (a.DocTotal ?? 0 - a.VatSum ?? 0 - a.DiscSum ?? 0),
                //                     a.DiscPrcnt,
                //                     a.VatSum,

                //                     a.DocTotal,
                //                     a.DocDate,
                //                     a.DiscSum
                //                 }).ToList();



                //**********************************************

                List<Saleorderreportchild> orderlist = new List<Saleorderreportchild>();

                SaleorderreportHead headprint = new SaleorderreportHead();
                headprint = (from a in db.OINVs.AsEnumerable()

                             join b in db.INV1.AsEnumerable() on a.DocEntry equals b.DocEntry
                             where a.DocNum == dno
                             select new SaleorderreportHead
                             {
                                 CardName = a.CardName,
                                 DocNum = a.DocNum,
                                 Cellular = (from c in db.OCRDs where a.CardCode == c.CardCode select (c.Cellular??"").ToString()).FirstOrDefault(),
                                 Address = (from d in db.CRD1 where a.CardCode == d.CardCode && d.AdresType == "B" select d.Address).FirstOrDefault(),
                                 Street = (from c in db.CRD1 where c.CardCode == a.CardCode && c.AdresType == "S" select c.Street).FirstOrDefault(),
                                 DocDueDate = (from c in db.ORDRs.AsEnumerable() where c.DocEntry == b.BaseEntry select (c.DocDate ?? DateTime.Now).ToString("dd/MM/yyyy")).FirstOrDefault(),
                                 Block = (from c in db.CRD1.AsEnumerable() where c.CardCode == a.CardCode where c.AdresType == "S" select   (c.Block??"").ToString() + "," + (c.Building??"").ToString()).FirstOrDefault(),
                                 U_Adv = a.U_Adv??0,
                                 CardCode = a.CardCode,
                                 ItmsGrpNam = ((from d in db.OITMs join e in db.OITBs on d.ItmsGrpCod equals e.ItmsGrpCod where d.ItemCode == b.ItemCode select e.ItmsGrpNam).FirstOrDefault()),
                                 TotalBeforeDiscount = ((a.DocTotal ?? 0) - (a.VatSum ?? 0) - (a.DiscSum ?? 0)),
                                 DiscPrcnt = a.DiscPrcnt,
                                 VatSum = a.VatSum,
                                 DocTotal = a.DocTotal,
                                 DiscSum = a.DiscSum,
                                 DocDate = (a.DocDate ?? DateTime.Now).ToString("dd/MM/yyyy"),
                                 U_DelDate = (b.U_DelDate ?? DateTime.Now).ToString("dd/MM/yyyy"),
                                 U_WebSaleOrder = a.U_WebSaleOrder

                             }).FirstOrDefault();


                List<Saleorderreportchild> objchild = (from a in db.OINVs.AsEnumerable()
                                                       join b in db.INV1.AsEnumerable() on a.DocEntry equals b.DocEntry
                                                       where a.DocNum == dno && b.U_ParentItem == "0"
                                                       select new Saleorderreportchild
                                                       {

                                                           newlinetotal = (from rr in db.INV1 where rr.U_ParentItem == b.U_MPI select rr.LineTotal ?? 0).ToList(),
                                                           newDscription = (from rr in db.INV1 where rr.U_ParentItem == b.U_MPI select "(" + rr.Dscription + ")").ToList(),
                                                           Dscription = b.Dscription,
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


                    //for (int i = 0; i < objresult.newlinetotal.Count; i++)
                    //{
                    //    //objresult.LineTotal = objresult.LineTotal + objresult.newlinetotal[i];

                    //}
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



                //*****************************************






                TempData["delno"]=dno.ToString();




                ReportDataSource rd = new ReportDataSource("Oinv", objheaderlist);
                lr.DataSources.Add(rd);
                //**********************************************************



                //*******************************************

                ReportDataSource rd1 = new ReportDataSource("Inv1", orderlist);
                lr.DataSources.Add(rd1);


                //************************************************************

                ReportDataSource rd2 = new ReportDataSource("Ocrd", objheaderlist);
                lr.DataSources.Add(rd2);
                ReportDataSource rd3 = new ReportDataSource("Crd1", objheaderlist);
                lr.DataSources.Add(rd3);
                TempData["objheaderlist"] = objheaderlist;
                TempData["orderlist"] = orderlist;
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
                renderedbytes = lr.Render(reporType, deviceinfo, out mimetypr, out encodeing, out filenameexns, out streams, out warnings);

                //return File(renderedbytes, mimetypr);
                // byte[] bytes = lr.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamids, out warnings);

                // CODE TO SAVE THE REPORT FILE ON SERVER
                renderedbytes = lr.Render(reporType, deviceinfo, out mimetypr, out encodeing, out filenameexns, out streams, out warnings);

                return File(renderedbytes, mimetypr);
            }

        }



    }

}
