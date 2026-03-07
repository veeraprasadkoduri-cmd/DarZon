using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DarZon.Models
{
    public class InvoiceHead
    {
        public string DeliveryNo { get; set; }

        public string InvoiceNo { get; set; }
        public string UserId { get; set; }
        public string DeliveryDate { get; set; }
        public string AltDate { get; set; }
        public string CustomerNo { get; set; }
        public string CustomerName { get; set; }

        public string AltDetails { get; set; }

        public decimal TotalBefDis { get; set; }

        public decimal Tax { get; set; }

        public decimal DocTotal { get; set; }
        public string Invoicedate { get; set; }
        public string invoicedetails { get; set; }

        public Nullable<decimal> cash { get; set; }
        public Nullable<decimal> discount { get; set; }
        public Nullable<decimal> card { get; set; }
        public Nullable<decimal> OtherPayments { get; set; }
        public string carddetails { get; set; }
        public string OthpayDetails { get; set; }
        public Nullable<decimal> Advance { get; set; }
        public Nullable<decimal> totalBeorerDiscount { get; set; }
        public Nullable<decimal> totalAfterDiscount { get; set; }
        public Nullable<decimal> BalanceAmount { get; set; }
        public Nullable<decimal> CurrentAdvance { get; set; }
        public string WHCODE { get; set; }
        public string websaleorder { get; set; }

    }
}