using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DarZon.Models;
namespace DarZon.Controllers
{
    public class MeasurmentdetailsController : Controller
    {
        DARZANTESTEntities db = new DARZANTESTEntities();
        // GET: Measurmentdetails
        [UserAuthenticationFilters]
        public ActionResult Measurmentdetails(string SaleOrder, string itemcode, int? id, int category, string custId, string custdetails,string nodays,string salorderdate,string pickupuser,string deliverydate,string cusphonenumber,string currentstatus)

        {
            List<SaleOrderDetail> objdetails = new List<SaleOrderDetail>();
            List<Mesurement> objmeasurment = new List<Mesurement>();
            SaleorderwithMeasurement objsalworderwithmeasurment = new SaleorderwithMeasurement();
            objsalworderwithmeasurment.CusPhoneNo = cusphonenumber;
            // var catglist = (from a in db.OITBs select new { Value = a.ItmsGrpCod, Text = a.ItmsGrpNam }).ToList();
          //  ViewBag.ItemcategoryList = catglist;
           // var catdetails = catglist.Where(a => a.Value == category).Select(a => a.Text).FirstOrDefault();
            ViewBag.Itemcode = category;
            var Altercode = (from a in db.OITBs where a.ItmsGrpCod == category select new { a.U_Alteration, a.ItmsGrpNam}).ToList().FirstOrDefault();
            ViewBag.alter = Altercode.U_Alteration ?? "No";
            var catdetails = Altercode.ItmsGrpNam;
            var salorder = db.SaleOrderHeaders.Where(a => a.DocEntry == SaleOrder).Select(a=>a).FirstOrDefault();
            if (salorder == null)
            {
                try
                {
                    SaleOrderHeader objsaleorder = new SaleOrderHeader();
                    objsaleorder.DocEntry = SaleOrder;
                    objsaleorder.CardCode = custId;
                    objsaleorder.CustomerName = custdetails;
                    objsaleorder.status = "O";
                    objsaleorder.DocDate = salorderdate.Replace("-","/");
                    objsaleorder.Pickupuser = pickupuser;
                   if (Session["UserName"] != null)
                    {
                        objsaleorder.UserName = Session["UserName"].ToString();
                    }
                    else
                    {
                        objsaleorder.UserName = "";
                    }


                    db.SaleOrderHeaders.Add(objsaleorder);
                    int Number = int.Parse(SaleOrder.Split('-')[2]);
                    string whereHouse = SaleOrder.Split('-')[1].ToString();
                    
                        Tbl_NumberSeries objnum = (from a in db.Tbl_NumberSeries where a.WHID == whereHouse select a).FirstOrDefault();
                        objnum.Number = Number + 1;
                        db.Entry(objnum).State = System.Data.Entity.EntityState.Modified;
                  
                        db.SaveChanges();
                        
                }
                catch (Exception ex)
                {
                }
            }
            else
            {
                salorder.CardCode = custId;
                salorder.CustomerName = custdetails;
                salorder.DocDate = salorderdate.Replace("-","/");
                salorder.Pickupuser = pickupuser;
                db.Entry(salorder).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            if (id == 0)
            {

                //var itemlist = (from a in db.OITMs join b in db.ITM1 on a.ItemCode equals b.ItemCode where b.PriceList == pricelist && a.ItmsGrpCod == id select new SaleorderwithMeasurement { ItemCode = a.ItemCode, ItemDescription = a.ItemName, UnitPrice = b.Price }).ToList();
                var lineid = (from c in db.SaleOrderDetails.AsEnumerable() where c.saleorderNo == SaleOrder && c.category == category.ToString() select c.Id.ToString()).LastOrDefault();
                 var prvmeasurements = (from a in db.Mesurements.AsEnumerable() where a.LineId== lineid select a).ToList();

                if (prvmeasurements != null && prvmeasurements.Count > 0)
                {
                    foreach (var measurement in prvmeasurements)
                    {
                        Mesurement objmeasu = new Mesurement();
                        objmeasu.imagepath = measurement.imagepath;
                        objmeasu.MeasurmentId = measurement.MeasurmentId;
                        objmeasu.MeasurmentName = measurement.MeasurmentName;
                        objmeasu.mesurementDetails = measurement.mesurementDetails;
                        objmeasurment.Add(objmeasu);
                    }


                }

                else { 
                var measu = (from a in db.C_MEASUREMENTS.AsEnumerable() where a.U_category == category.ToString() select new { a.Code, a.Name }).ToList();

                foreach (var measurement in measu)
                {
                    Mesurement objmeasu = new Mesurement();
                    objmeasu.MeasurmentId = measurement.Code;
                    objmeasu.MeasurmentName = measurement.Name;
                    objmeasurment.Add(objmeasu);
                }




                }
                objsalworderwithmeasurment.status = "Add";
            }
            else
            {
                objdetails = (from a in db.SaleOrderDetails.AsEnumerable() where a.parentId == id.ToString() select a).ToList();
                objmeasurment = (from a in db.Mesurements.AsEnumerable() where a.LineId == id.ToString() select a).ToList();
                var detalheader = (from a in db.SaleOrderDetails.AsEnumerable() where a.Id == id select a).FirstOrDefault();
                objsalworderwithmeasurment.Advance = detalheader.Advance;
                objsalworderwithmeasurment.balance = detalheader.balance;
                objsalworderwithmeasurment.cardcoe = detalheader.cardcoe;
                objsalworderwithmeasurment.category = detalheader.category;
                objsalworderwithmeasurment.CategoryDetails = detalheader.CategoryDetails;
                objsalworderwithmeasurment.COGSCostingCode = detalheader.COGSCostingCode;
                if (currentstatus == "C")
                    objsalworderwithmeasurment.curstatus = "C";
                else
                    objsalworderwithmeasurment.curstatus = detalheader.curstatus;
                objsalworderwithmeasurment.deldays = detalheader.deldays;
                objsalworderwithmeasurment.Deliverydate = detalheader.Deliverydate.Replace("-","/");
                objsalworderwithmeasurment.Intdeldate = detalheader.Intdeldate.Replace("-", "/");
                objsalworderwithmeasurment.Id = detalheader.Id;
                objsalworderwithmeasurment.ItemCode = detalheader.ItemCode;
                objsalworderwithmeasurment.ItemDescription = detalheader.ItemDescription;
                objsalworderwithmeasurment.MeterialCost = detalheader.MeterialCost;
                objsalworderwithmeasurment.parentId = detalheader.parentId;
                objsalworderwithmeasurment.predelchar = detalheader.predelchar;
                objsalworderwithmeasurment.quantity = detalheader.quantity;
                objsalworderwithmeasurment.Remarks = detalheader.Remarks;
                objsalworderwithmeasurment.saleorderNo = detalheader.saleorderNo;
                objsalworderwithmeasurment.ServiceCost = detalheader.ServiceCost;
                objsalworderwithmeasurment.maxQty = detalheader.maxQty;
                objsalworderwithmeasurment.minQty = detalheader.minQty;
                objsalworderwithmeasurment.status = detalheader.status;
                objsalworderwithmeasurment.TaxAmount = detalheader.TaxAmount;
                objsalworderwithmeasurment.taxCode = detalheader.taxCode;
                objsalworderwithmeasurment.TaxRate = detalheader.TaxRate;
                objsalworderwithmeasurment.TotalAmount =Math.Round(detalheader.TotalAmount??0,0);
                objsalworderwithmeasurment.UnitPrice = Math.Round(detalheader.UnitPrice??0,0);
                objsalworderwithmeasurment.NoFabraic = detalheader.NoFabraic;
                objsalworderwithmeasurment.FitOpt = detalheader.FitOpt;
                objsalworderwithmeasurment.MeterialImage = detalheader.MeterialImage;
                objsalworderwithmeasurment.SampleImage = detalheader.SampleImage;
                objsalworderwithmeasurment.OtherImage = detalheader.OtherImage;
            }

            objsalworderwithmeasurment.saleorderNo = SaleOrder;
            objsalworderwithmeasurment.addonlist = objdetails;
            objsalworderwithmeasurment.objmesurementlist = objmeasurment;
            objsalworderwithmeasurment.CategoryDetails = catdetails;
            objsalworderwithmeasurment.noofdays = nodays;
            objsalworderwithmeasurment.Deliverydate = deliverydate;

          
     ViewBag.ItemList  = (from a in db.OITMs
                            where a.U_SubGrp == "Addon" && a.ItmsGrpCod == category
                            select new
                            {
                                Value = a.ItemCode,
                                Text = a.ItemName,
                               
                            }).ToList();





            return View(objsalworderwithmeasurment);

        }
        [HttpPost]
        [AllowAnonymous]
        [ActionName("Measurmentdetails")]
        [onAction(ButtonName = "Save")]
        public ActionResult Save(SaleorderwithMeasurement objsaleorder)
        {
            TempData["saleorder"] = Commansave(objsaleorder);
            return RedirectToAction("SaleOrder", "SaleOrder");
        }
        [HttpPost]
        [AllowAnonymous]
        [ActionName("Measurmentdetails")]
        [onAction(ButtonName = "Update")]
        public ActionResult Update(SaleorderwithMeasurement objsaleorder)
        {
            int lineid = 0;

            SaleOrderDetail objdetaild = new SaleOrderDetail();
            objdetaild.Advance = objsaleorder.Advance;
            objdetaild.saleorderNo = objsaleorder.saleorderNo;
            objdetaild.balance = objsaleorder.balance;
            objdetaild.cardcoe = objsaleorder.cardcoe;
            objdetaild.curstatus = objsaleorder.curstatus;
            objdetaild.Deliverydate = objsaleorder.Deliverydate.Replace("-","/");
            objdetaild.Intdeldate = objsaleorder.Intdeldate.Replace("-", "/");
            objdetaild.ItemCode = objsaleorder.ItemCode;
            objdetaild.ItemDescription = objsaleorder.ItemDescription;
            objdetaild.UnitPrice = objsaleorder.UnitPrice;
            objdetaild.TotalAmount = objsaleorder.TotalAmount;
            objdetaild.TaxAmount = objsaleorder.TaxAmount;
            objdetaild.category = objsaleorder.category;
            objdetaild.parentId = "0";
            objdetaild.Id = 0;
            objdetaild.taxCode = (from a in db.OITMs where a.ItemCode == objsaleorder.ItemCode select a.U_Taxcode).FirstOrDefault();
            var salordedetails = (from a in db.SaleOrderDetails where a.Id == objsaleorder.Id select a).FirstOrDefault();
            db.SaleOrderDetails.Remove(salordedetails);

            db.SaveChanges();
            if (objsaleorder.addonlist != null)
            {
                var addonlist = (from a in db.SaleOrderDetails.AsEnumerable() where objsaleorder.addonlist.Select(b => b.Id).Contains(a.Id) select a).ToList();
                db.SaleOrderDetails.RemoveRange(addonlist);
            }

            if (objsaleorder.objmesurementlist != null)
            {
                var measurlist = (from a in db.Mesurements.AsEnumerable() where objsaleorder.objmesurementlist.Select(b => b.id).Contains(a.id) select a).ToList();

                db.Mesurements.RemoveRange(measurlist);
            }

            db.SaveChanges();
            objsaleorder.Id = 0;

            TempData["saleorder"] = Commansave(objsaleorder);


            return RedirectToAction("SaleOrder", "SaleOrder");
        }
        public saleorder Commansave(SaleorderwithMeasurement objsaleorder)
        {
            int lineid = 0;
            saleorder objsale = new saleorder();
            objsale.custPhoneNo = objsaleorder.CusPhoneNo;
            objsale.objlistsalDetails = new List<SaleorderwithMeasurement>();
            objsale.objsaleorderHeader = (from a in db.SaleOrderHeaders where a.DocEntry == objsaleorder.saleorderNo select a).FirstOrDefault();
            if (TempData["Updatesaleorder"] != null)
            {
                objsaleorder = (SaleorderwithMeasurement)TempData["Updatesaleorder"];
            }

            if (objsaleorder.Id == 0)
            {
                SaleOrderDetail objdetaild = new SaleOrderDetail();
                objdetaild.Advance =Math.Round(objsaleorder.Advance??0,0);
                objdetaild.saleorderNo = objsaleorder.saleorderNo;
                objdetaild.balance = Math.Round(objsaleorder.balance??0,2);
                objdetaild.cardcoe = objsaleorder.cardcoe;
                objdetaild.curstatus = objsaleorder.curstatus;
                DateTime datefrom = DateTime.ParseExact(objsale.objsaleorderHeader.DocDate, "dd/MM/yyyy", null);
                DateTime dateto= datefrom;
                if (objsaleorder.Deliverydate == null || objsaleorder.Deliverydate.Length < 0)
                {
                    for (int i = 1; i <= int.Parse(objsaleorder.noofdays); i++)
                    {


                        if (dateto.DayOfWeek == DayOfWeek.Sunday)
                        {
                            i = i - 1;
                        }
                        dateto = dateto.AddDays(1);


                    }
                    objdetaild.Deliverydate = dateto.ToString("dd/MM/yyyy").Replace("-", "/");
                    objdetaild.Intdeldate = dateto.ToString("dd/MM/yyyy").Replace("-", "/");
                }
                else
                {
                    objdetaild.Deliverydate = objsaleorder.Deliverydate;
                    objdetaild.Intdeldate = objsaleorder.Intdeldate;
                }

               
                objdetaild.ItemCode = objsaleorder.ItemCode;
                objdetaild.ItemDescription = objsaleorder.ItemDescription;
                objdetaild.UnitPrice = Math.Round(objsaleorder.UnitPrice??0,0);
                objdetaild.TotalAmount = Math.Round(objsaleorder.TotalAmount??0,0);
                objdetaild.TaxAmount = Math.Round(objsaleorder.TaxAmount??0,0);
                objdetaild.category = objsaleorder.category;
                objdetaild.NoFabraic = objsaleorder.NoFabraic;
                objdetaild.FitOpt = objsaleorder.FitOpt;
                objdetaild.MeterialImage = objsaleorder.MeterialImage;
                objdetaild.OtherImage = objsaleorder.OtherImage;
                objdetaild.SampleImage = objsaleorder.SampleImage;
                objdetaild.CategoryDetails = objsaleorder.CategoryDetails;
                objdetaild.parentId = "0";
             
                objdetaild.taxCode = (from a in db.OITMs where a.ItemCode == objsaleorder.ItemCode select a.U_Taxcode).FirstOrDefault();
                objdetaild.Remarks = objsaleorder.Remarks;
                db.SaleOrderDetails.Add(objdetaild);
                db.SaveChanges();

                lineid = (from a in db.SaleOrderDetails.AsEnumerable() where a.saleorderNo == objdetaild.saleorderNo select a.Id).LastOrDefault();

                if (objsaleorder.addonlist != null)
                {
                    foreach (SaleOrderDetail objaddon in objsaleorder.addonlist)
                    {
                        objaddon.parentId = lineid.ToString();
                        objaddon.saleorderNo = objsaleorder.saleorderNo;
                        db.SaleOrderDetails.Add(objaddon);
                        db.SaveChanges();

                    }
                }
                if(objsaleorder.objmesurementlist!=null)
                { 
                foreach (Mesurement objmeasu in objsaleorder.objmesurementlist)
                    {
                        objmeasu.LineId = lineid.ToString();
                        db.Mesurements.Add(objmeasu);
                        db.SaveChanges();
                    }
                }
            }
           if(objsale.objsaleorderHeader.status!="C")
           objsale.objsaleorderHeader.status = "P";
            List<SaleOrderDetail> objdetsils = (from a in db.SaleOrderDetails where a.parentId == "0" && a.saleorderNo == objsaleorder.saleorderNo select a).ToList();
            var total = objdetsils.Sum(a => a.TotalAmount);
            objsale.objsaleorderHeader.totalBeorerDiscount = Math.Round(total??0,0);
            objsale.objsaleorderHeader.totalAfterDiscount = Math.Round(total??0,0);
            objsale.objsaleorderHeader.BalanceAmount = Math.Round(total??0,0);
            objsale.objsaleorderHeader.Discount = Math.Round(objsale.objsaleorderHeader.Discount ?? 0, 0);

            db.Entry(objsale.objsaleorderHeader).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            foreach (SaleOrderDetail objd in objdetsils)
            {
                SaleorderwithMeasurement objswithmeasere = new SaleorderwithMeasurement();
                objswithmeasere.addonlist = new List<SaleOrderDetail>();
                objswithmeasere.objmesurementlist = new List<Mesurement>();
                objswithmeasere.Advance = Math.Round(objd.Advance??0,0);
                objswithmeasere.balance = Math.Round(objd.balance??0,0);
                objswithmeasere.cardcoe = objd.cardcoe;
                objswithmeasere.COGSCostingCode = objd.COGSCostingCode;
                objswithmeasere.curstatus = objd.curstatus;
                objswithmeasere.deldays = objd.deldays;
                objswithmeasere.Deliverydate = objd.Deliverydate.Replace("-","/");
                objswithmeasere.Intdeldate = objd.Intdeldate;
                objswithmeasere.Id = objd.Id;
                objswithmeasere.ItemCode = objd.ItemCode;
                objswithmeasere.ItemDescription = objd.ItemDescription;
                objswithmeasere.MeterialCost = Math.Round(objd.MeterialCost??0,0);
                objswithmeasere.parentId = objd.parentId;
                objswithmeasere.predelchar = objd.predelchar;
                objswithmeasere.quantity = objd.quantity;
                objswithmeasere.Remarks = objd.Remarks;
                objswithmeasere.saleorderNo = objd.saleorderNo;
                objswithmeasere.ServiceCost = Math.Round(objd.ServiceCost??0,0);
                objswithmeasere.maxQty = objd.maxQty;
                objswithmeasere.minQty = objd.minQty;
                objswithmeasere.status = objd.status;
                objswithmeasere.TaxAmount = Math.Round(objd.TaxAmount??0,0);
                objswithmeasere.TotalAmount = Math.Round(objd.TotalAmount??0,0);
                objswithmeasere.UnitPrice = Math.Round(objd.UnitPrice??0,0);
                objswithmeasere.CategoryDetails = objd.CategoryDetails;
                objswithmeasere.category = objd.category;
                objswithmeasere.taxCode = objd.taxCode;
                objswithmeasere.NoFabraic = objd.NoFabraic;
                objswithmeasere.FitOpt = objd.FitOpt;
                objswithmeasere.maxQty = objd.maxQty;
                objswithmeasere.minQty = objd.minQty;
                objswithmeasere.MeterialImage = objd.MeterialImage;
                objswithmeasere.OtherImage = objd.OtherImage;
                objswithmeasere.SampleImage = objd.SampleImage;

                objswithmeasere.noofdays = (from a in db.OITMs where a.ItemCode == objd.ItemCode select a.U_Days).FirstOrDefault();
                objsale.objlistsalDetails.Add(objswithmeasere);

            }
            return objsale;



        }
        public ActionResult viewmeasurement(string parentId)
        {
            SaleorderwithMeasurement objsalworderwithmeasurment = new SaleorderwithMeasurement();

            int ID = int.Parse(parentId);
            var head = (from a in db.SaleOrderDetails where a.Id == ID select a).FirstOrDefault();
            var curstatus = (from a in db.SaleOrderHeaders where a.DocEntry == head.saleorderNo select a.status).FirstOrDefault();
            objsalworderwithmeasurment.Id = head.Id;
            objsalworderwithmeasurment.Advance = head.Advance;
            objsalworderwithmeasurment.balance = head.balance;
            objsalworderwithmeasurment.cardcoe = head.cardcoe;
            objsalworderwithmeasurment.category = head.category;
            objsalworderwithmeasurment.CategoryDetails = head.CategoryDetails;
            objsalworderwithmeasurment.COGSCostingCode = head.COGSCostingCode;
            objsalworderwithmeasurment.curstatus = curstatus;
            objsalworderwithmeasurment.deldays = head.deldays;
            objsalworderwithmeasurment.Deliverydate = head.Deliverydate;
            objsalworderwithmeasurment.Intdeldate = head.Intdeldate;
            objsalworderwithmeasurment.IsCustomermeterial = head.IsCustomermeterial;
            objsalworderwithmeasurment.ItemCode = head.ItemCode;
            objsalworderwithmeasurment.ItemDescription = head.ItemDescription;
            objsalworderwithmeasurment.linenumber = head.linenumber;
            objsalworderwithmeasurment.MeterialCost = head.MeterialCost;
            objsalworderwithmeasurment.parentId = head.parentId;
            objsalworderwithmeasurment.predelchar = head.predelchar;
            objsalworderwithmeasurment.quantity = head.quantity;
            objsalworderwithmeasurment.Remarks = head.Remarks;
            objsalworderwithmeasurment.status = "Retrive";
            objsalworderwithmeasurment.saleorderNo = head.saleorderNo;
            objsalworderwithmeasurment.ServiceCost = head.ServiceCost;
             objsalworderwithmeasurment.minQty = head.minQty;
            objsalworderwithmeasurment.maxQty = head.maxQty;
            objsalworderwithmeasurment.TaxAmount = head.TaxAmount;
            objsalworderwithmeasurment.taxCode = head.taxCode;
            objsalworderwithmeasurment.TaxRate = head.TaxRate;
            objsalworderwithmeasurment.TotalAmount =Math.Round(head.TotalAmount??0,0);
            objsalworderwithmeasurment.UnitPrice = Math.Round(head.UnitPrice??0,0);

            objsalworderwithmeasurment.addonlist = (from a in db.SaleOrderDetails where a.parentId == parentId select a).ToList();
            objsalworderwithmeasurment.objmesurementlist = (from a in db.Mesurements where a.LineId == parentId select a).ToList();
             return View("Measurmentdetails", objsalworderwithmeasurment);

        }
        public JsonResult subgrouplist(string id)
        {
            //int pricelist = int.Parse(System.Configuration.ConfigurationManager.AppSettings["PricelistNo"].ToString());
            //int meterialprice = int.Parse(System.Configuration.ConfigurationManager.AppSettings["meterialcost"].ToString());

            //var servicecost = (from b in db.ITM1 where b.ItemCode == a.ItemCode && b.PriceList == pricelist select Math.Round((b.Price ?? 0), 0)).FirstOrDefault();
            //var meterialcost = (from b in db.ITM1 where b.ItemCode == a.ItemCode && b.PriceList == meterialprice select Math.Round(b.Price ?? 0, 0)).FirstOrDefault();

            var itemlist = (from a in db.OITMs
                            where a.U_APID.Contains(id)
                            select new SelectListItem
                            {
                                Value = a.ItemCode,
                                Text = a.ItemName,

                            });


            return Json(itemlist, JsonRequestBehavior.AllowGet);

        }

        public JsonResult mainsubgrouplist(string id)
        {
            //int pricelist = int.Parse(System.Configuration.ConfigurationManager.AppSettings["PricelistNo"].ToString());
            //int meterialprice = int.Parse(System.Configuration.ConfigurationManager.AppSettings["meterialcost"].ToString());

            //var servicecost = (from b in db.ITM1 where b.ItemCode == a.ItemCode && b.PriceList == pricelist select Math.Round((b.Price ?? 0), 0)).FirstOrDefault();
            //var meterialcost = (from b in db.ITM1 where b.ItemCode == a.ItemCode && b.PriceList == meterialprice select Math.Round(b.Price ?? 0, 0)).FirstOrDefault();

            var itemlist = (from a in db.OITMs
                            where a.U_SubGrp == "Addon" &&  a.U_ParentItem.Contains(id)
                            select new SelectListItem
                            {
                                Value = a.ItemCode,
                                Text = a.ItemName,

                            });


            return Json(itemlist, JsonRequestBehavior.AllowGet);

        }


        public PartialViewResult addItem(string mainaddon,string subaddonid,string itemname)
        {
            SaleOrderDetail objsaleorder = new SaleOrderDetail();
            objsaleorder.ItemCode = subaddonid;
            objsaleorder.ItemDescription = itemname;
            objsaleorder.COGSCostingCode = mainaddon;
            int pricelist = int.Parse(System.Configuration.ConfigurationManager.AppSettings["PricelistNo"].ToString());
            int meterialprice = int.Parse(System.Configuration.ConfigurationManager.AppSettings["meterialcost"].ToString());

            var servicecost = (from b in db.ITM1 where b.ItemCode == subaddonid && b.PriceList == pricelist select Math.Round((b.Price ?? 0), 0)).FirstOrDefault();
            var meterialcost = (from b in db.ITM1 where b.ItemCode == subaddonid && b.PriceList == meterialprice select Math.Round(b.Price ?? 0, 0)).FirstOrDefault();
            var minmax= (from b in db.OITMs where b.ItemCode == subaddonid  select new {minqty=((b.MinLevel??0)== 0 ?1: b.MinLevel), maxqty= ((b.MaxLevel ?? 0) == 0 ? 1000000 : b.MaxLevel) }).FirstOrDefault();

            objsaleorder.MeterialCost = Math.Round(meterialcost,0);
            objsaleorder.ServiceCost = Math.Round(servicecost,0);
            objsaleorder.TotalAmount = Math.Round(servicecost,0)+ Math.Round(meterialcost, 0)*Math.Round(minmax.minqty??1,0);
            objsaleorder.quantity = decimal.ToInt32(minmax.minqty??1).ToString();
            objsaleorder.taxCode=(from a in db.OITMs where a.ItemCode == subaddonid select a.U_Taxcode).FirstOrDefault();
            objsaleorder.minQty = decimal.ToInt32(minmax.minqty??1);
            objsaleorder.maxQty = decimal.ToInt32(minmax.maxqty??1);
            objsaleorder.IsCustomermeterial = false;
           return AddAddonItem(objsaleorder);
        }
        public PartialViewResult AddAddonItem(SaleOrderDetail objsaleorder)
        {


            return PartialView("AddAddonItem", objsaleorder);


        }


    }
}