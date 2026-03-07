using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DarZon.Models
{
    public class Saleorderreportchild
    {
        //public string Dscription { get; set; }
    public List<string> newDscription { get; set; }
      
        public List<decimal> newlinetotal { get; set; }
        public string Dscription { get; set; } 
        public Nullable<decimal> Quantity { get; set; }
        public Nullable<decimal> Price { get; set; }
        public Nullable<decimal> LineTotal { get; set; }
        public Nullable<decimal> TotalBeforeDiscount { get; set; }
        public Nullable<decimal> DiscPrcnt { get; set; }
        public Nullable<decimal> DocTotal { get; set; }
        public Nullable<decimal> DiscSum { get; set; }
        public Nullable<decimal> VatSum { get; set; }
     
        public string DocDate { get; set; }
        public string U_DelDate { get; set; }
        public int DocEntry { get; set; }
        public int LineNum { get; set; }

        public string U_ItemG { get; set; }
        public string U_MPI { get; set; }
        



    }
}