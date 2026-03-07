using SolarPrime.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SolarPrime.DAL
{
    public class CartListController : Controller
    {
        ECOMEntities3 db = new ECOMEntities3();
        // GET: CartList
        [HttpPost]
        public JsonResult getcartlist(string useremail)
        {

            var cartlist = (from a in db.EC_Tbl_Cart_Products where a.User_Code == useremail select a).ToList();

            return Json(cartlist, JsonRequestBehavior.AllowGet);
        }
        public JsonResult addtocary(int ID,int quantity)
        {

           if(Session["Usercode"]!=null)
            {
                string UserId = (string)Session["Usercode"];
                EC_Tbl_Cart_Products cartitem = new EC_Tbl_Cart_Products();

            var actItem = (from a in db.EC_Tbl_Product_Attributes
                           join b in db.EC_Tbl_Products on a.Product_Code equals b.Product_Code
                           where a.Id == ID select new { a.Product_Price,a.Discount_Percentage,b.Product_Name,b.Product_Code,b.ImagePath}).FirstOrDefault();

            cartitem.Product_Code = actItem.Product_Code;
            cartitem.Product_Price = actItem.Product_Price * (99 / 100) * actItem.Discount_Percentage;
            cartitem.User_Code = UserId;
            cartitem.Quantity = quantity;
                cartitem.ImagePath = actItem.ImagePath;
            cartitem.Date_Added = DateTime.Now;
            db.EC_Tbl_Cart_Products.Add(cartitem);
            db.SaveChanges();
            return Json(new { value = "Success" }, JsonRequestBehavior.AllowGet);
            }
           else
            {
                return Json(new { value = "fail" }, JsonRequestBehavior.AllowGet);

            }


        }

        public JsonResult UpdateQty(int Qty, string ProductId,string Price,string status)
        {
            try
            {
                if (Session["Usercode"] != null)
                {
                    string[] arrProductId = ProductId.Split('_');
                   
                    int ptid =int.Parse(arrProductId[1]);
                    EC_Tbl_Cart_Products cartitem = (from d in db.EC_Tbl_Cart_Products where d.Id == ptid select d).FirstOrDefault();
                    cartitem.Quantity = Qty;
                    cartitem.TotalAmount = decimal.Parse(Price) * Qty;

                    db.SaveChanges();

                    return Json(new { value = "Success" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { value = "Fail" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return Json(new { value = "Fail" }, JsonRequestBehavior.AllowGet);
            }

        }
        public JsonResult DeleteCart(int ProductCode)

        {
            try
            {

                //  string user_code = (string)Session["Usercode"];
                EC_Tbl_Cart_Products cartitem = (from d in db.EC_Tbl_Cart_Products where d.Id == ProductCode select d).FirstOrDefault();
                if (cartitem != null)
                {
                    db.EC_Tbl_Cart_Products.Remove(cartitem);
                    db.SaveChanges();
                    return Json(new { value = "Success" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { value = "Fail" }, JsonRequestBehavior.AllowGet);

                }
            }
            catch (Exception ex)
            {
                return Json(new { value = "Fail" }, JsonRequestBehavior.AllowGet);
            }

        }

        public JsonResult DeleteWlist(int ProductCode)

        {
            try
            {

                //  string user_code = (string)Session["Usercode"];
                EC_Tbl_User_Wishlist cartitem = (from d in db.EC_Tbl_User_Wishlist where d.Id == ProductCode select d).FirstOrDefault();
                if (cartitem != null)
                {
                    db.EC_Tbl_User_Wishlist.Remove(cartitem);
                    db.SaveChanges();
                    return Json(new { value = "Success" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { value = "Fail" }, JsonRequestBehavior.AllowGet);

                }
            }
            catch (Exception ex)
            {
                return Json(new { value = "Fail" }, JsonRequestBehavior.AllowGet);
            }

        }

        public JsonResult AddToWList(string ProductId,string Usercode)
        {
            try
            {
                //string UserId = (string)Session["Usercode"];
                EC_Tbl_User_Wishlist NewItem = new EC_Tbl_User_Wishlist();
                int ptid = int.Parse(ProductId);
                NewItem.User_Code = Usercode;
                NewItem.Product_Code = ProductId;
                var actItem = (from a in db.EC_Tbl_Product_Attributes
                               join b in db.EC_Tbl_Products on a.Product_Code equals b.Product_Code
                               where a.Id == ptid
                               select new { a.Product_Price, a.Discount_Percentage, b.Product_Name, b.Product_Code,b.ImagePath }).FirstOrDefault();


                NewItem.ProductName = actItem.Product_Name;
                NewItem.ImagePath = actItem.ImagePath;
                NewItem.Date_Added = DateTime.Now;
                NewItem.Date_Added_Price = actItem.Product_Price * (99 / 100) * actItem.Discount_Percentage;
                db.EC_Tbl_User_Wishlist.Add(NewItem);
                db.SaveChanges();
                return Json(new { value = "Success" }, JsonRequestBehavior.AllowGet);
            }
            catch(Exception e)
            {
                return Json(new { value = "Fail" }, JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult AddAddress(AddressViewModel model)
        {
            try
            {
                if (model.Id == 0)
                {
                    EC_Tbl_Addresses add = new EC_Tbl_Addresses();
                    add.Name = model.Name;
                    add.Mobile_No = model.Mobile_No;
                    add.Landmark = model.Landmark;
                    add.Pincode = model.Pincode;
                    add.State = model.State.Trim();
                    add.Street = model.Street;
                    add.User_Code = System.Web.HttpContext.Current.Session["Usercode"].ToString();
                    add.Address_Type = model.Address_Type;
                    add.City = model.City;
                    add.Country = model.Country.Trim();
                    add.Door_No = model.Door_No;
                    db.EC_Tbl_Addresses.Add(add);
                    db.SaveChanges();
                }
                else
                {
                    EC_Tbl_Addresses address = (from d in db.EC_Tbl_Addresses where d.Id == model.Id select d).FirstOrDefault();
                    address.Name = model.Name;
                    address.Mobile_No = model.Mobile_No;
                    address.Landmark = model.Landmark;
                    address.Pincode = model.Pincode;
                    address.State = model.State.Trim();
                    address.Street = model.Street;
                    address.Address_Type = model.Address_Type;
                    address.City = model.City;
                    address.Country = model.Country.Trim();
                    address.Door_No = model.Door_No;
                    db.SaveChanges();
                }

                return Json(new { value = "Success" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { value = "Fail" }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}