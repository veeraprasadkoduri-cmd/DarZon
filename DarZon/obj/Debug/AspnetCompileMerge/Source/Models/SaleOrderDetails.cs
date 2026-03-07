using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DarZon.Models
{
    public class SaleOrderDetails : ItemMaster
    {

        public int SNO { get; set; }

        public string COGSCostingCode { get; set; }
        public string TaxCode { get; set; }
        public string Deliverydate { get; set; }
        public double Advance { get; set; }
        public double balance { get; set; }
        public double TotalAmount { get; set; }
        public string Remarks { get; set; }
        public int quantity { get; set; }
        public string cardcoe { get; set; }
        public string curstatus { get; set; }



    }
}