using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DarZon.Models;
using DarZon;
namespace DarZon.Controllers
{
    public class OrderDetailsController : Controller
    {
        // GET: OrderDetails
        DARZANTESTEntities db = new DARZANTESTEntities();
       
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CustomerMaster(Orderlistmaster objorderlist)
        {
            SAPIntegration objsapint = new SAPIntegration();
            // objsapint.Customers(objorderlist);
            return View();
        }
        public PartialViewResult Orederlist(string whName)
        {
            if (whName == null)
                whName = "";
            var whid = (from a in db.OWHS.AsEnumerable() where a.WhsName.Trim() == whName.Trim() select a.WhsCode).FirstOrDefault();

            string WhreHouseID= "";

            if (TempData["wharehouse"] != null)
                WhreHouseID = TempData["wharehouse"].ToString();
            else
                WhreHouseID = (string)Session["WhareHouse"];
            string mainuser = System.Configuration.ConfigurationManager.AppSettings["wharehouse"].ToString();
             var ismain = (from a in db.OWHS where a.WhsCode == WhreHouseID select a.U_main).FirstOrDefault(); 
             if (ismain != null && ismain == "YES")
            {
               var ordrlist = (from a in db.ORDRs.AsEnumerable() where ((from c in db.RDR1.AsEnumerable() where c.U_TrailStatus == "Yes" && (c.U_TNo??"").Length<=0 && c.U_DSTS != "Yes" select c.DocEntry).Contains(a.DocEntry) && a.U_WhsCode== whid) select new Orderlistchild { websaleorder=a.U_WebSaleOrder,Orderno = a.DocNum.ToString(), Qty = int.Parse(decimal.Round((from b in db.RDR1 where b.DocEntry == a.DocEntry && b.U_ParentItem == "0" && b.U_TrailStatus == "Yes" && (b.U_TStatus=="" || b.U_TStatus==null) select b.DocEntry).Count(), 0).ToString()), Custname = a.CardName, Schedulefordelivery = (a.DocDueDate ?? DateTime.Now).ToString("dd/MM/yy"), Addonqty = (from g in db.RDR1 where g.DocEntry == a.DocEntry  && (from h in db.RDR1 where h.DocEntry == h.DocEntry && h.U_ParentItem == "0" && h.U_TrailStatus == "Yes" && (h.U_TStatus??"").Length <= 0 select h.U_MPI).Contains(g.U_ParentItem) select g.U_Fabric).Sum() ?? 0 }).ToList();
               
                return PartialView(ordrlist);
            }
            else
            {
                var ordrlist = (from a in db.ORDRs.AsEnumerable() where (!(from c in db.Transportaionchilds select c.orderno).Contains(a.DocNum) && (a.U_WhsCode == WhreHouseID || WhreHouseID == mainuser)) select new Orderlistchild { websaleorder = a.U_WebSaleOrder, Orderno = a.DocNum.ToString(), Qty = int.Parse(decimal.Round((from b in db.RDR1 where b.DocEntry == a.DocEntry && b.U_ParentItem == "0" select b.DocEntry).Count(), 0).ToString()), Custname = a.CardName, Schedulefordelivery = (a.DocDueDate ?? DateTime.Now).ToString("dd/MM/yy"), Addonqty = (from g in db.RDR1 where g.DocEntry == a.DocEntry  select g.U_Fabric).Sum() ?? 0 }).ToList();
                //objinvlist = (from a in db.ODLNs.AsEnumerable() join b in db.DLN1.AsEnumerable() on a.DocEntry equals b.DocEntry where a.DocNum == int.Parse(deliveryno) select new Invoicechild { ItemCode = b.ItemCode, Total = a.DocTotal ?? 0, description = b.Dscription, qty = int.Parse(decimal.Round((b.Quantity ?? 0), 0).ToString()), Rate = Convert.ToDecimal(b.Price), Tax = b.TaxCode ?? "0" }).ToList();
                return PartialView(ordrlist);
            }


        }
        public ActionResult Orderlist(int id)
        {

            var itemlist = (from a in db.ORDRs.AsEnumerable() join b in db.RDR1.AsEnumerable() on a.DocEntry equals b.DocEntry where a.DocNum == id select new Orderlistchild { websaleorder = a.U_WebSaleOrder, Orderno = a.DocNum.ToString(), Qty = int.Parse(decimal.Round((b.Quantity ?? 0), 0).ToString()), Custname = a.CardName, Schedulefordelivery = (a.DocDueDate ?? DateTime.Now).ToString("dd/MM/yy") }).ToList();
            return View(itemlist);
        }
    }
}