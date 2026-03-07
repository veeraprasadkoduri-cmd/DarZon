using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DarZon.Models;
namespace DarZon.Controllers
{
    public class OTPCreationController : Controller
    {
        // GET: OTPCreation
        DARZANTESTEntities db = new DARZANTESTEntities();
      
        public JsonResult createOTP(string saleorder,string mobileno,string message)
        {
            try
            {
              

                 string username = System.Configuration.ConfigurationManager.AppSettings["username"].ToString() ;
                string passsword = System.Configuration.ConfigurationManager.AppSettings["password"].ToString();
                string alphabets = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                string small_alphabets = "abcdefghijklmnopqrstuvwxyz";
                string numbers = "1234567890";
                string characters = numbers;
                int length = 4;
                string otp = string.Empty;
                for (int i = 0; i < length; i++)
                {
                    string character = string.Empty;
                    do
                    {
                        int index = new Random().Next(0, characters.Length);
                        character = characters.ToCharArray()[index].ToString();
                    } while (otp.IndexOf(character) != -1);
                    otp += character;
                }
               
                message = otp + " is your OTP from DARZAN for confirming the collection of all the Goods in PL Store Warehouse from Main Warehouse to deliver at Hyderabad";
                message.Replace("XXXX", otp);
                SMSCAPI objsms = new SMSCAPI();
                objsms.SendSMS(username, passsword, mobileno, message);

                Session["OTP"] = otp;
                return Json(new { value = otp }, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                return Json(new { value = "Fail" }, JsonRequestBehavior.AllowGet);
            }


        }
        public JsonResult ResendOTP(string saleorder,string mobileno,string message)
        {
            
                return createOTP(saleorder, mobileno, message);
       
           
        }
        public JsonResult checkOPT(string saleorder,string OTP)
        {
            string opt = "";
            var check = (from a in db.SaleOrderHeaders where a.DocEntry == saleorder && a.OTP == OTP select a).FirstOrDefault();
            if (Session["OTP"].ToString()== OTP)
            {
                return Json(new { value = "success" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { value = "Fail" }, JsonRequestBehavior.AllowGet);
            }
            }
        public PartialViewResult OTP(string salorder)
        {
            OTPEntity objotp = new OTPEntity();

            objotp.saleorderno = salorder;

            return PartialView(objotp);


        }

    }
}