using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DarZon.Models
{
    public class Ordermaster
    {
        public int Docnum { get; set; }
        public string CustomerName { get; set; }
        public Nullable<System.DateTime> DocDueDate { get; set; }
    }
}