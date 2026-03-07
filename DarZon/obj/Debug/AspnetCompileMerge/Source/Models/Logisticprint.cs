using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DarZon.Models
{
    public class Logisticprint
    {

    public string DocNum { get; set; }
       
        public Nullable<decimal> AddonQty { get; set; }
        public string LabelNo { get; set; }
        public string MainlabelNo { get; set; }
        
        public Nullable<decimal> MainQty { get; set; }

        public string Lbno { get; set; }
        public string WebSO { get; set; }
       
        public Nullable<decimal> Fabric { get; set; }
        public int Id { get; set; }

    }
}