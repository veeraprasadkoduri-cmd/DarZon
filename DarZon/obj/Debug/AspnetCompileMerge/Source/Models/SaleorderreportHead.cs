using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DarZon.Models
{
    public class SaleorderreportHead
    {

        public string Phone2 { get; set; }
        public string CardName { get; set; }
        public Nullable<int> DocNum { get; set; }
        public string CardCode { get; set; }
        public string Username { get; set; }
        public string Cellular { get; set; }
        public string Street { get; set; }
        public string Block { get; set; }
        public string Address2 { get; set; }
        public string Address { get; set; }
        public string County { get; set; }
        public Nullable<decimal> U_Advance { get; set; }
        public string ItmsGrpNam { get; set; }
        public Nullable<decimal> TotalBeforeDiscount { get; set; }
        public Nullable<decimal> DiscPrcnt { get; set; }
        public Nullable<decimal> DocTotal { get; set; }
        public Nullable<decimal> DiscSum { get; set; }
        public Nullable<decimal> VatSum { get; set; }
        public string U_DelDate { get; set; }
        public string DocDate { get; set; }
        public string U_WebSaleOrder { get; set; }
        public string DocDueDate { get; set; }
        public Nullable<decimal> U_Adv { get; set; }
        
    }
}