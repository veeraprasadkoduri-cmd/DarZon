using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DarZon.Models
{
    public class Orderlistmaster
    {

        //public string Docnum { get; set; }
        //public string CustomerName { get; set; }
        //public string DocDueDate { get; set; }
        public Orderheader objsaleorderHeader { get; set; }
        public List<Orderlistchild> objchild { get; set; }
        //public List<Transportaionchild> objchilddata { get; set; }
        //public List<Transportaionhead> objheaddata { get; set; }

    }
} 