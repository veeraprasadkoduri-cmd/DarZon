using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DarZon.Models;
namespace DarZon.Controllers
{
    public class APIorderdetailsController : Controller
    {
        // GET: APIorderdetails
       public JsonResult APIorderdetails(string phno)
        {
            DARZANTESTEntities db = new DARZANTESTEntities();
            var objsaleorder = (from a in db.ORDRs.AsEnumerable()
                                join b in db.RDR1.AsEnumerable() on a.DocEntry equals b.DocEntry
                                join c in db.OCRDs.AsEnumerable() on a.CardCode equals
                                c.CardCode
                          
                                where c.Cellular == phno && b.U_ParentItem == "0"
                                select new SaleDetails
                                {
                                    customername = c.CardName,
                                    mobileno = phno,
                                    saleorderno = a.U_WebSaleOrder,
                                    linenumber = b.LineNum,
                                    mainitems = b.Dscription,
                                    deldate = (b.U_DelDate ?? DateTime.Now).ToString("dd/MM/yyyy"),
                                                   
                                    status = b.U_TrailStatus == "Yes" ? "Ready for Trial" : "In Production	",
                                   
                                    
                                    whareHouse = (from e in db.OWHS where e.WhsCode == a.U_WhsCode select e.WhsName).FirstOrDefault(),
                            });
            if (objsaleorder.ToList().Count <= 0)
            {


                return Json("No record  found", JsonRequestBehavior.AllowGet);

            }
            else
            {
                string saleorder = "";
                List<SaleDetails> objdetails = new List<SaleDetails>();
                int count = 0;
                foreach (SaleDetails objsaledetails in objsaleorder)
                {

                    SaleDetails objdetail = new SaleDetails();
                    if (objsaledetails.saleorderno!= saleorder)
                    {
                        count = 0;
                    }
                   
               
                    count = count + 1;
                    objdetail = objsaledetails;
                    objdetail.linenumber = count;
                    saleorder = objsaledetails.saleorderno;
                    objdetails.Add(objdetail);
                }


              

                return Json(objdetails, JsonRequestBehavior.AllowGet);
            }
        }



    }
}