using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DarZon.Models
{
    public class CustomerMaster
    {
        public string customercode { get; set; }
        public string CustomerName { get; set; }
        public string PhoneNumber { get; set; }

        public string DoorNo { get; set; }
        public string Street { get; set; }
        public string Block { get; set; }
        public string city { get; set; }
        public string Area { get; set; }
        public string Landmark { get; set; }
        public string Pincode { get; set; }
        public string State { get; set; }
        public string Emailid { get; set; }
        public string AltrphoneNo { get; set; }
        public string CustTYpe { get; set; }
        public string Anniversery { get; set; }
        public string DBO { set; get; }
    }
}