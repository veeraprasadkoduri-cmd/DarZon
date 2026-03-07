using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DarZon.Models;

namespace DarZon.Controllers
{
    public class DeliveryController : Controller
    {
        DARZANTESTEntities db = new DARZANTESTEntities();
        // GET: Delivery
        [UserAuthenticationFilters]
        public ActionResult Delivery(deliveryModel objdelivery)
        {

            deliveryModel objdel = new deliveryModel();

            if (TempData["Delevery"] != null)
            {
                objdel = (deliveryModel)TempData["Delevery"];
            }
            else
            {
                objdel.delheqad = new DeliveryHead();
                objdel.objsaleorder = new List<SaleOrderDetail>();
            }
            objdel.delheqad.UserId = Session["UserName"].ToString();
            objdel.delheqad.DelDate = DateTime.Now.ToString("dd/MM/yyyy");

            List<deliverysearch> objhead = (from a in db.ORDRs.AsEnumerable()    where (from b in db.RDR1 where b.U_DSTS == "Yes" select b.DocEntry).Contains(a.DocEntry) select new deliverysearch { Customer = a.CardName, DeliverNo = a.DocNum.ToString(), Delverydate = (a.DocDate ?? DateTime.Now).ToString("dd/MM/yyyy"), id = a.DocEntry ,websaleorderno=a.U_WebSaleOrder}).Distinct().ToList();
            ViewBag.deliverylist = objhead;
            ViewBag.pickuserlist = (from a in db.OHEMs select new { Value = a.firstName + a.middleName + a.lastName, Text = a.firstName + a.middleName + a.lastName }).ToList();

            return View(objdel);
        }
        public ActionResult Orderlist()
        {

            var orderlist = (from a in db.ORDRs.AsEnumerable()
                             join c in db.OCRDs.AsEnumerable() on a.CardCode equals c.CardCode
                             where a.DocStatus == "O"
                       && (from b in db.RDR1 where b.U_TrailStatus == "Yes" && b.U_TStatus == "Yes" && b.U_DSTS != "Yes" select b.DocEntry).Contains(a.DocEntry)
                             select new DeliveryHead
                             {
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
                                 delveryNo = a.DocEntry.ToString(),
                                 websaleorder=a.U_WebSaleOrder
                             }).ToList();


            return View(orderlist);
        }
        public ActionResult orderdetails(string salorderNo)
        {

            List<SaleOrderDetail> orderlist = new List<SaleOrderDetail>();

            try
            {
                orderlist = (from a in db.ORDRs.AsEnumerable() join b in db.RDR1.AsEnumerable() on a.DocEntry equals b.DocEntry where a.DocEntry == int.Parse(salorderNo) && b.U_TrailStatus == "Yes" && b.U_TStatus == "Yes" && b.U_DSTS != "Yes" select new SaleOrderDetail { ItemCode = b.ItemCode, TaxAmount = Math.Round(b.VatPrcnt??0,0), ItemDescription = b.Dscription, quantity = decimal.Round(Math.Round((b.Quantity ?? 0), 0), 0).ToString(), UnitPrice =Math.Round(b.Price??0,0), taxCode = b.TaxCode, TotalAmount = decimal.Parse(Math.Round((b.LineTotal ?? 0), 0).ToString()), Deliverydate = (b.U_DelDate ?? DateTime.Now).ToString("dd/MM/yyyy"), linenumber = b.LineNum, saleorderNo = b.DocEntry.ToString(), }).ToList();

            }
            catch (Exception ex)
            {

            }
            return PartialView(orderlist);

        }
        [HttpPost]
        [AllowAnonymous]
        [ActionName("Delivery")]
        [onAction(ButtonName = "Save")]
        public ActionResult savedetails(deliveryModel objdeliverymodel)
        {

            SAPIntegration objsapint = new SAPIntegration();
            foreach (SaleOrderDetail objsal in objdeliverymodel.objsaleorder)
            {
                RDR1 obline = (from a in db.RDR1.AsEnumerable() where a.DocEntry == int.Parse(objsal.saleorderNo) && a.LineNum == objsal.linenumber select a).FirstOrDefault();
                if (obline != null)
                {
                    obline.U_DSTS = "Yes";
                    db.Entry(obline).State = System.Data.Entity.EntityState.Modified;

                    db.SaveChanges();
                }

            }

            //string sapmsg = objsapint.Delivery(objdeliverymodel);
            //string message = "";
            //if (sapmsg.Length > 20)
            //{
            //    message = sapmsg.Substring(0, 19); 

            //}
            //if (sapmsg == "Success")
            //{

            //    return Content("<script language='javascript' type='text/javascript'>alert('Delivery Details Submitted Successfully!');window.location ='../Delivery/Delivery';</script>");

            //}
            //else
            //{

            //    //return Content("<script language='javascript' type='text/javascript'>alert(" + sapmsg + ");</script>");
            //    //return RedirectToAction("CustomerMaster", "CustomerMaster", new { objcustomer = objcustomer });
            //    TempData["Delevery"] = objdeliverymodel;
            //    return Content("<script language='javascript' type='text/javascript'>alert('" + message + "');window.location ='../Delivery/Delivery';</script>");
            //}


            return Content("<script language='javascript' type='text/javascript'>alert('Delivery Details Submitted Successfully!');window.location ='../Delivery/Delivery';</script>");
            //return RedirectToAction("Delivery", "Delivery");
        }
        public ActionResult Finddelivery(int id)
        {
            deliveryModel objdel = new deliveryModel();
            var delivery = (from a in db.ORDRs where a.DocEntry == id  select a).FirstOrDefault();
            var deldetails = (from a in db.RDR1 where a.DocEntry == delivery.DocEntry && a.U_DSTS == "Yes" select a).ToList();

            objdel.delheqad = new DeliveryHead();
            objdel.delheqad.SaleOrderNo = delivery.U_WebSaleOrder;
            objdel.delheqad.CustomerName = delivery.CardName;
            objdel.delheqad.CustomerNo = delivery.CardCode;
            objdel.delheqad.DelDate = delivery.DocDate.ToString();
            objdel.delheqad.SaleOrderNo = delivery.U_WebSaleOrder;
            objdel.delheqad.saleorderdate = delivery.DocDate.ToString();
            objdel.delheqad.TotalBefDis = Math.Round(delivery.DocTotal ?? 0);
            objdel.delheqad.DocTotal = Math.Round(delivery.DocTotal ?? 0, 0);
            objdel.delheqad.delveryNo = delivery.DocNum.ToString();
            objdel.delheqad.AltDetails = delivery.Comments;


            objdel.objsaleorder = new List<SaleOrderDetail>();
            foreach (RDR1 objdln in deldetails)
            {
                SaleOrderDetail objdetails = new SaleOrderDetail();
                //objdetails.Advance = objdln.ad
                objdetails.ItemCode = objdln.ItemCode;
                objdetails.ItemCode = objdln.ItemCode;
                objdetails.ItemDescription = (from a in db.OITMs.AsEnumerable() where a.ItemCode == objdln.ItemCode select a.ItemName).FirstOrDefault();
                objdetails.quantity =Math.Round(objdln.Quantity??0,0).ToString();
                objdetails.UnitPrice = Math.Round(objdln.Price??0,0);
                objdetails.TotalAmount = Math.Round((objdln.Quantity * objdln.Price)??0,0);
                objdetails.Deliverydate = (objdln.U_DelDate ?? DateTime.Now).ToString("dd/MM/yyyy");
                objdetails.Remarks = objdln.U_Remarks;
                objdel.objsaleorder.Add(objdetails);
            }
            //objdel.delheqad.AltPhoneNo = delivery..ToString();
            TempData["Delevery"] = objdel;

            return RedirectToAction("Delivery", "Delivery");
        }
    }
}