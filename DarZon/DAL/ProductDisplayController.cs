using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SolarPrime.Models.MetaDataClasses;
using SolarPrime.Models;

namespace SolarPrime.DAL
{
    public class ProductDisplayController : Controller
    {
        //
        // GET: /ProductDisplay/
       public  static List<ProductDisplayList> productlist;
        ECOMEntities3 db = new ECOMEntities3();
        public JsonResult ProductDisplay()
        {
            
             productlist = (from a in db.EC_Tbl_Products join b in db.EC_TbL_Categories on a.Catg_Code equals b.Catg_Code
                            join c in db.EC_Tbl_Product_Attributes on a.Product_Code equals c.Product_Code

                            select new ProductDisplayList { Catg_Code= a.Catg_Code,

                Catg_Name=b.Catg_Name,
                 Catg_Intr_Code=b.Catg_Intr_Code,
                 Description=a.Description,
                 Discount_Percentage=c.Discount_Percentage,
                  Parent_Id=b.Parent_Id,
                   Position=b.Position,
                    Product_Code=a.Product_Code,
                     Product_Intr_Code=a.Product_Intr_Code,
                     Product_Name=a.Product_Name,
                     Product_Price=c.Product_Price,
                     
                                Stock =c.Stock,
                            Id=c.Id,
                              ImagePath = a.ImagePath
                            }).Distinct().ToList();

            if(productlist.Count>0)
            {
            return Json(new { value = productlist }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { value = "Nodatafound" }, JsonRequestBehavior.AllowGet);
            }
        }
	}




}