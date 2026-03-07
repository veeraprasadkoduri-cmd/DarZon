using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DarZon.Models
{
    public class Orderlistchild
    {

        public string Orderno { get; set; }
        public string Custname { get; set; }
        public int Qty { get; set; }
        public decimal Addonqty { get; set; }
        public string Remarks { get; set; }
        public string EstimatedEfforts { get; set; }
        public string Schedulefordelivery { get; set; }
        public string Reschedule { get; set; }
        public string websaleorder { get; set; }

    }
}