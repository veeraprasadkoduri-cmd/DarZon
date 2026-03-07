using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DarZon.Models;
using DarZon;
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
   
    public class CustomeDetailsrController : Controller
    {

        
        // GET: CustomerMaster
        DARZANTESTEntities db = new DARZANTESTEntities();
        
        public ActionResult CustomerMaster()
        {
            CustomerMaster objcus = new CustomerMaster();
            if (TempData["predetails"] != null)
            objcus = (CustomerMaster)TempData["predetails"];
            // var states = (from a in db.OCSTs select new { Value = a.Code, Text = a.Name });
            ViewBag.state = (new List<SelectListItem>
{
    new SelectListItem { Selected = false, Text = "TS", Value = "TS"}
            });

            return View(objcus);
        }
        [HttpPost]
        [AllowAnonymous]
        [ActionName("CustomerMaster")]
        [onAction(ButtonName = "Save")]
        public ActionResult CustomerMasterSave(CustomerMaster objcustomer)
        {
            SAPIntegration objsapint = new SAPIntegration();
           
                string sapmsg = objsapint.Customers(objcustomer);

                if (sapmsg == "Success")
                {

                    return Content("<script language='javascript' type='text/javascript'>alert('Customer Details Submitted Successfully');window.location ='../CustomeDetailsr/CustomerMaster';</script>");

                }
                else
                {
                    //return Content("<script language='javascript' type='text/javascript'>alert("+ sapmsg +");</script>");
                    TempData["predetails"] = objcustomer;
                    return Content("<script language='javascript' type='text/javascript'>alert('" + sapmsg + "');</script>");
                }
            
        }

        public ActionResult customerlist()
        {
            var custlist = (from a in db.OCRDs.AsEnumerable() join b in db.CRD1.AsEnumerable() on a.CardCode equals b.CardCode where b.AdresType == "S" select new CustomerMaster { AltrphoneNo = a.Phone2, Street = b.Street, Block = b.Block, city = b.City, customercode = a.CardCode, CustomerName = a.CardName, PhoneNumber = a.Cellular, Emailid = a.E_Mail, DoorNo = b.Address3, Landmark = b.Address2, Pincode = b.ZipCode, State = b.State, Area = b.Building, Anniversery = ((a.U_DOA ?? DateTime.Now).ToString("dd/MM/yyyy")), DBO = ((a.U_DOB ?? DateTime.Now).ToString("dd/MM/yyyy")) }).ToList();
            return View(custlist);
        }
        public ActionResult ItemList(int? id)
        {
            try
            {
                int pricelist = int.Parse(System.Configuration.ConfigurationManager.AppSettings["PricelistNo"].ToString());
                var itemlist = (from a in db.OITMs.AsEnumerable() join b in db.ITM1 on a.ItemCode equals b.ItemCode where b.PriceList == pricelist && a.ItmsGrpCod == id && a.U_SubGrp != "Addon" && (a.U_APID==null || a.U_APID.Length<=0)  select new SaleorderwithMeasurement { ItemCode = a.ItemCode, ItemDescription = a.ItemName, UnitPrice = Math.Round(b.Price ?? 0, 0), noofdays = a.U_Days ?? "5" }).ToList();
                return PartialView(itemlist);
            }
            catch (Exception ex)
            {
                throw (ex);
            }


        }
        public JsonResult checkphno(string newpwd)
        {
            string message;

            //string emailId = Session["email"].ToString();
            int phnocount = (from a in db.OCRDs where a.Cellular == newpwd select a.Cellular).Count();
            if (phnocount == 0)
            {

                message = "Success";
                    }
            else
            {
                message = "Fail";
            }



            return Json(new { value = message }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AllowAnonymous]
        [ActionName("CustomerMaster")]
        [onAction(ButtonName = "Update")]
        public ActionResult CustomerMasterUpdate(CustomerMaster objcustomer)
        {
            SAPIntegration objsapint = new SAPIntegration();

            string sapmsg = objsapint.Customers(objcustomer);

            if (sapmsg == "Success")
            {

                return Content("<script language='javascript' type='text/javascript'>alert('Customer Details Update Successfully');window.location ='../CustomeDetailsr/CustomerMaster';</script>");

            }
            else
            {
                //return Content("<script language='javascript' type='text/javascript'>alert("+ sapmsg +");</script>");
                TempData["predetails"] = objcustomer;
                return Content("<script language='javascript' type='text/javascript'>alert('" + sapmsg + "');</script>");
            }

        }

        


    }
}