using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace DarZon.Models
{
    public class  RDRheader
    {
        public ORDR oRDR { get; set; }
        public List<RDR1> orderlistchild { get; set; }
        public List<Transportaionchild> transchild { get; set; }
    }
}