using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DarZon.Models
{
    public class Invoicechild
    {

        public string ItemCode { get; set; }
        public string description { get; set; }
        public Decimal qty { get; set; }
        public Decimal Rate { get; set; }

        public string Tax { get; set; }

        public Decimal Total { get; set; }
        public string Remarks { get; set; }

        public string Line { get; set; }
        public string websaleorder { get; set; }
    }
}