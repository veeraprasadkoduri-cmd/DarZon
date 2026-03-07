using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DarZon.Models
{
    public class Orderheader
    {
        public int Tno { get; set; }
        public string Tstatus { get; set; }
        public string pickupuser { get; set; }
        public string Tdate { get; set; }
        public string UserID { get; set; }
        public  string  Outletfrom{ get; set; }
        public string outletto { get; set; }
        public string orderno { get; set; }
        public string MeasurmentDeatails { get; set; }
    }
}