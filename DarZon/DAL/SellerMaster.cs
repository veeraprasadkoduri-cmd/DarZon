using DarZon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace SolarPrime.DAL
{
    public class SellerMaster : UserMaster
    {
        public override void createuser(SellerUser objseller)
        {
            objseller.user.Roles = "Seller";
            objseller.user.User_Type = "S";
            objseller.user.User_Code = objseller.selleruser.Supplier_Code;
            objseller.user.First_Name = objseller.selleruser.Company_Name;
            objseller.user.Mobile_No = objseller.selleruser.Phone;
            objseller.user.Email_ID = objseller.selleruser.Email_Id;
            CreateUser(objseller.user, "0", "0", "0");

            db.EC_Tbl_Suppliers.Add(objseller.selleruser);
            db.SaveChanges();



        }
    }
}