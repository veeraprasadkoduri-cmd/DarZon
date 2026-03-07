using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DarZon.Models
{
    public class DeliveryHead
    {
        public string SaleOrderNo { get; set; }
        public string websaleorder { get; set; }
        public string UserId { get; set; }
        public string AltNo { get; set; }
        public string AltDate { get; set; }
        public string CustomerNo { get; set; }
        public string CustomerName { get; set; }

        public string AltDetails { get; set; }
        public string DelDate { get; set; }

        public decimal TotalBefDis { get; set; }

        public decimal DiscountP { get; set; }

        public decimal Tax { get; set; }

        public decimal DocTotal { get; set; }

        public string PhoneNo { get; set; }

        public string AltPhoneNo { get; set; }
        public string saleorderdate { get; set; }
        public string delveryNo { get; set; }
        public string Pickupuser { get; set; }

        public string WHCODE { get; set; }
    }
}