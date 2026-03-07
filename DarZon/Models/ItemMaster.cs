using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DarZon.Models
{
    public class ItemMaster
    {
        public string ItemCode { get; set; }
        public string ItemDescription { get; set; }

        public Nullable<decimal> UnitPrice  { get; set; }
        public int deldays { get; set; }
        public int predelchar { get; set; }
    }
}