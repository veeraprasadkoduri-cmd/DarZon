using DarZon.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace DarZon.DAL
{
    public abstract class UserMaster : DALSignUpBaseController
    {
      public VEGAEntities db = new VEGAEntities();
        public  abstract void createuser(SellerUser objsignupdetails);

        protected void forgotpassword(SignupData objsignupdata)
        {

        }
        protected void ChangePassword(SignupData objsignupdata)
        {


        }

        //public JsonResult CreateUser(Tbl_Usermaster newUser, string productId,string quantity,string status)
        //{
        //    try
        //    {
        //        var newCode = "";
                
        //        string role;
        //        if (newUser.User_Type == "B")
        //        {
        //            newCode = getUniqueno("BC");
        //            role = "Buyer";
        //            newUser.Roles = "Buyer";
        //        }
        //        else
        //        {
        //            newCode = getUniqueno("SC");
        //            role = "Seller"; 
        //        }

        //        if (newUser != null)
        //        {
        //            EC_Tbl_User_Data newEntery = new EC_Tbl_User_Data();


        //            newEntery.User_Intr_Code = newUser.User_Intr_Code;
        //            newEntery.City = newUser.City;
        //            newEntery.Country = newUser.Country;
        //            newEntery.CreatedDate = DateTime.Now;
        //            newEntery.CurrentVisited = newUser.CurrentVisited;
        //            newEntery.Last_Name = newUser.Last_Name;
        //            newEntery.LastVisited = newUser.LastVisited;
        //            newEntery.LOGGED_IN = newUser.LOGGED_IN;
        //            newEntery.State = newUser.State;
        //            newEntery.Street_Address = newUser.Street_Address;
        //            newEntery.VisitCount = newUser.VisitCount;
        //            newEntery.Zip_Code = newUser.Zip_Code;
        //            newEntery.Mobile_No = newUser.Mobile_No;
        //            newEntery.Email_ID = newUser.Email_ID;
        //            newEntery.Password = newUser.Password;
        //            newEntery.OTP_No = newUser.OTP_No;
        //            newEntery.First_Name = newUser.First_Name;
        //            newEntery.User_Code = newCode;
        //            newEntery.Roles = newUser.Roles;
        //            newEntery.User_Type = newUser.User_Type;
        //             newEntery.Active = "A";
        //            newEntery.EmailActive = "N";
        //            newEntery.CreatedDate = DateTime.Now;
        //            db.EC_Tbl_User_Data.Add(newEntery);
        //            db.SaveChanges();
        //            FormsAuthentication.SetAuthCookie(newCode, false);
        //           System.Web.HttpContext.Current.Session["Usercode"] = newCode;
        //           System.Web.HttpContext.Current.Session["UserName"] = newUser.First_Name;
        //            var authTicket = new FormsAuthenticationTicket(1, newUser.Email_ID, DateTime.Now, DateTime.Now.AddMinutes(20), false, newUser.Roles);
        //            string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
        //            var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
        //            System.Web.HttpContext.Current.Response.Cookies.Add(authCookie);
                  
        //            if (status == "C" )
        //            {
        //                AddToCart(productId, quantity);
        //            }
        //            else if(status=="W")
        //            {
        //                CartListController cart = new CartListController();
        //                cart.AddToWList(productId, newCode);
        //            }
        //            //MC_TBL_Temp_OTP temOTP = db.MC_TBL_Temp_OTP.Where(w => w.Mobile_No == newUser.Mobile_No).FirstOrDefault();

        //            //if (temOTP != null)
        //            //{
        //            //    db.MC_TBL_Temp_OTP.Remove(temOTP);
        //            //    db.SaveChanges();
        //            //}

        //            //var addresDetails = db.MC_TBL_AppConfig.FirstOrDefault();

        //            //string email, subject, fromAddress, mailPWWD, mailServer;

        //            //if (addresDetails.SendEmailFlag == "Y")
        //            //{
        //            //    fromAddress = addresDetails.FromMail;
        //            //    mailServer = addresDetails.SMTPmailserver;
        //            //    mailPWWD = addresDetails.Frommailpwd;
        //            //    email = newUser.Email_ID;
        //            //    subject = "Medsy Activation mail";

        //            //    body = new StringBuilder();

        //            //    body.Append("<<div class='mail-cont f style='font-style:bold'><p><b>Dear User</b></p> ");
        //            //    body.Append("<p>Please activate your account to complete the signup process with Medsy.</b></p> ");
        //            //    body.Append("<p><a href='http://medsy.in/ActiveUser/Active?userCode=" + newCode + "'> &nbsp;&nbsp;click here to activate. </b></div>");

        //            //    EmailService.SendMail(subject, body, email);

        //            //    string message1 = "Please activate your account to complete the signup process with Medsy http://medsy.in/ActiveUser/Active?userCode";

        //            //    // var phoneno = temOTP.Mobile_No;
        //            //    //string message1 = "Login process with Medsy is";
        //            //    var phoneno = newEntery.Mobile_No;

        //            //    SmsService.SendSMS(message1, phoneno);
        //            //}
        //            return Json(new { value = "Success" }, JsonRequestBehavior.AllowGet);
        //           // return "Success";
        //        }
        //        else
        //        {
        //            return Json(new { value = "Fail" }, JsonRequestBehavior.AllowGet);
        //        }

        //    }
        //    catch (DbEntityValidationException dbEx)
        //    {
        //        foreach (var validationErrors in dbEx.EntityValidationErrors)
        //        {
        //            foreach (var validationError in validationErrors.ValidationErrors)
        //            {
        //                System.Console.WriteLine("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
        //            }
        //        }
                
        //        return Json(new { value = "Error in Entity!" }, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { value = "Fail" }, JsonRequestBehavior.AllowGet);
        //    }
        //}

    
        //public string AddToCart( string productId,string quantity)
        //{

        //    try
        //    {
        //        int ptid = int.Parse(productId);
        //        var Productprice = (from  a in db.EC_Tbl_Product_Attributes join b in db.EC_Tbl_Products on a.Product_Code equals b.Product_Code where a.Id == ptid select new { a.Product_Price,a.Discount_Percentage,b.Product_Name ,b.ImagePath}).FirstOrDefault();

        //        EC_Tbl_Cart_Products NewItem = new EC_Tbl_Cart_Products();
               
        //        NewItem.User_Code = System.Web.HttpContext.Current.Session["Usercode"].ToString();
        //        NewItem.Product_Code = productId;
        //        NewItem.ImagePath = Productprice.ImagePath;
        //        NewItem.ProductName = Productprice.Product_Name;
        //        NewItem.Quantity = int.Parse(quantity);
        //        NewItem.Date_Added = DateTime.Now;
        //        NewItem.Product_Price = Productprice.Product_Price*(100-Productprice.Discount_Percentage)/100;
        //        NewItem.TotalAmount = Productprice.Product_Price * (100 - Productprice.Discount_Percentage) / 100 * int.Parse(quantity);
        //        db.EC_Tbl_Cart_Products.Add(NewItem);
        //        db.SaveChanges();
        //        return "Success";
        //    }
        //    catch (Exception ex)
        //    {
        //        return "Fail";
        //    }
        //}
    }

 public class DALSignUpBaseController:Controller
 {
    VEGAEntities db = new VEGAEntities();
    //public JsonResult CheckDublicateMobileNumber(string mobileNumber, string SrvType)
    // {
        
    //     try
    //     {
    //         var checkMB = db.Tbl_Usermaster.Where(w => w.Mobile_No == mobileNumber && w.User_Type == SrvType).FirstOrDefault();

    //         if (checkMB != null)
    //         {
    //                return Json(new { value = "Success" }, JsonRequestBehavior.AllowGet);
    //            }
    //         else
    //         {
    //                return Json(new { value = "Fail" }, JsonRequestBehavior.AllowGet);
    //            }

             
    //        }
    //     catch (Exception ex)
    //     {
    //            return Json(new { value = "Fail" }, JsonRequestBehavior.AllowGet);
    //        }
    // }
        public JsonResult Islogin()
        {
            if (Session["Usercode"] == null)
            {
                return Json(new { value = "Fail" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { value = "Success" }, JsonRequestBehavior.AllowGet);
            }

        }
     //   public JsonResult CheckDublicateEmailId(string emailId, string ServType)
     //{
     //    string message;
     //    try
     //    {
     //        var checkEmail = db.EC_Tbl_User_Data.Where(w => w.Email_ID == emailId && w.User_Type == ServType).FirstOrDefault();

     //        if (checkEmail != null)
     //        {
     //            message = "Success";
     //        }
     //        else
     //        {
     //            message = "Fail";
     //        }
     //           return Json(new { value = message }, JsonRequestBehavior.AllowGet);
     //       }
     //    catch (Exception ex)
     //    {
     //           return Json(new { value = "Fail" }, JsonRequestBehavior.AllowGet);
     //       }
     //}
     //public JsonResult NotRegisterdEmailId(string emailId)
     //{
     //    string message;
     //    try
     //    {
     //        var checkEmail = (from user in db.EC_Tbl_User_Data where (user.Email_ID == emailId || user.Mobile_No == emailId) select new { name = user.User_Type }).ToList().Distinct();

     //        if (checkEmail.Count() == 0)
     //        {
     //           message = "Success";
     //               return Json(new { value = message }, JsonRequestBehavior.AllowGet);
     //           }
     //        else
     //        {
     //             return Json(new { value = "Fail"}, JsonRequestBehavior.AllowGet);
     //        }

           
            
     //    }
     //    catch (Exception ex)
     //    {
     //        return Json(new { value = "Fail" }, JsonRequestBehavior.AllowGet);
         
     //    }
     //}
     //public string ValidedOPT(string otp, string emailId)
     //{
     //    try
     //    {
     //        if (otp != "")
     //        {
     //            var userDetails = (from user in db.EC_Tbl_User_Data where user.Email_ID == emailId || user.Mobile_No == emailId select user).FirstOrDefault();

     //            if (userDetails.OTP_No == otp)
     //            {
     //                return  "Success" ;
     //            }
     //            else
     //            {
     //                return "Fail";
     //            }
     //        }
     //        else
     //        {
     //            return "Fail" ;
     //        }
     //    }
     //    catch (Exception ex)
     //    {
     //        return "Catch Error";
     //    }
     //}
     //public string SendOPTToUser(string mobile, string emailId)
     //{
     //    try
     //    {
     //        string massage;

     //        string email, subject, otp;

     //        if (emailId != null)
     //        {
     //            otp = EmailService.CreateOTP();

     //            MC_TBL_Temp_OTP userDetails = new MC_TBL_Temp_OTP();

     //            userDetails.Email_ID = emailId;
     //            userDetails.Mobile_No = mobile;
     //            userDetails.OTP_No = Convert.ToInt32(otp);

     //            Repository.db.MC_TBL_Temp_OTP.Add(userDetails);
     //            Repository.db.SaveChanges();

     //            email = emailId;
     //            subject = "Medsy Signup OTP";

     //            StringBuilder body = new StringBuilder();

     //            body.Append("<div class='mail-cont f style='font-style:bold'><p><b>Dear User,</b></p> ");
     //            body.Append("<p>One time password (OTP) for your signup process with Medsy is " + otp + ".</b></p> ");
     //            body.Append("<p>This Password is valid only for this transaction.</b></p>");
     //            body.Append("<p>Do not share it with any one.</b></div>");

     //            EmailService.SendMail(subject, body, email);

     //            string message1 = "" + otp + " for signup process with Medsy";
     //            var phoneno = userDetails.Mobile_No;
     //            SmsService.SendSMS(message1, phoneno);
     //            massage = "Success";
     //        }
     //        else
     //        {
     //            massage = "Fail";
     //        }

     //        //return (new { Value = massage });
     //        return massage;
     //    }
     //    catch (DbEntityValidationException dbEx)
     //    {
     //        foreach (var validationErrors in dbEx.EntityValidationErrors)
     //        {
     //            foreach (var validationError in validationErrors.ValidationErrors)
     //            {
     //                System.Console.WriteLine("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
     //            }
     //        }
     //        return ("Error in Entity!");
     //    }
     //    catch (Exception ex)
     //    {
     //      //  return Json("Catch Error", JsonRequestBehavior.AllowGet);
     //        return "Catch Error";
     //    }
     //}
     //public string ResendOTP(string mobile)
     //{
     //    try
     //    {
     //        string message;

     //        string email = "", subject, otp;

     //        if (mobile != null)
     //        {
     //            otp = EmailService.CreateOTP();

     //            var userDetails = db.MC_TBL_Temp_OTP.Where(t => t.Mobile_No == mobile).FirstOrDefault();

     //            if (userDetails != null)
     //            {
     //                userDetails.OTP_No = Convert.ToInt32(otp);
     //                Repository.db.SaveChanges();


     //                subject = "Medsy Signup OTP";
     //                email = userDetails.Email_ID;

     //                StringBuilder body = new StringBuilder();

     //                body.Append("<div class='mail-cont f style='font-style:bold'><p><b>Dear User,</b></p> ");
     //                body.Append("<p>One time password (OTP) for your signup process with Medsy is " + otp + ".</b></p> ");
     //                body.Append("<p>This Password is valid only for this transaction.</b></p>");
     //                body.Append("<p>Do not share it with any one.</b></div>");

     //                EmailService.SendMail(subject, body, email);

     //                string message1 = "" + otp + " for signup process with Medsy";
     //                var phoneno = userDetails.Mobile_No;
     //                SmsService.SendSMS(message1, phoneno);
     //                message = "Success";
     //            }
     //            else
     //            {
     //                message = "Fail";
     //            }
     //        }
     //        else
     //        {
     //            message = "Fail";
     //        }

     //       // return Json(new { Value = message }, JsonRequestBehavior.AllowGet);
     //        return message;
     //    }
     //    catch (Exception ex)
     //    {
     //        //return Json(new { Value = "fail" }, JsonRequestBehavior.AllowGet);
     //        return "Fail";
     //    }
     //}
     //public string ValidedOPTToUser(string otp, string emailId, string mobileNo)
     //{
     //    try
     //    {
     //        if (otp != "")
     //        {
     //            var userDetails = db.MC_TBL_Temp_OTP.Where(e => e.Email_ID == emailId).FirstOrDefault();

     //            if (userDetails.OTP_No == Convert.ToInt32(otp))
     //            {
     //                //return Json(new { Value = "Success" }, JsonRequestBehavior.AllowGet);
     //                return "Success";
     //            }
     //            else
     //            {
     //               // return Json(new { Value = "Fail" }, JsonRequestBehavior.AllowGet);
     //                return "Fail";
     //            }
     //        }
     //        else
     //        {
     //            //return Json(new { Value = "Fail" }, JsonRequestBehavior.AllowGet);
     //            return "Fail";
     //        }
     //    }
     //    catch (Exception ex)
     //    {
     //       // return Json("Catch Error", JsonRequestBehavior.AllowGet);
     //        return "Catch Error";
     //    }
     //}
     public void ErrorLog(string Message, string innerexcep)
     {
         //Session.Abandon();
         StreamWriter sw = new StreamWriter(Server.MapPath("~") + "\\ErrorDetails_Log\\ErrorLog.txt", true);

         string Controller = System.Web.HttpContext.Current.Request.RequestContext.RouteData.GetRequiredString("controller");

         string action = System.Web.HttpContext.Current.Request.RequestContext.RouteData.GetRequiredString("action");
         string errorocu = "\n\n" + "Error Occured on" + DateTime.Now.ToString() + "\n";
         string design = "===========================================================" + "\n";
         string occured = "\n" + "Details-- Controller :" + Controller + "\n" + " Action name:" + action + "\n";
         sw.WriteLine(errorocu + design + occured + " Error Details :" + Message + "Inner Exception" + innerexcep);

         sw.Flush();
         sw.Close();

     }
     public string getUniqueno(string ReqUniqueType)
     {
         try
         {
             ObjectParameter Unique_RNo = new ObjectParameter("uniqueno", typeof(String));
             var UniqueNo = Repository.db.gen_unique_no(ReqUniqueType, Unique_RNo);
             return (Unique_RNo.Value.ToString());
         }
         catch (Exception ex)
         {
             return (null);

         }
     }
     


    }



}