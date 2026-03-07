using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DarZon.Models
{
    public class Tailorassignment
    { 
       public Tailorassignmentdates Objdate { get; set; }
       
        public ICollection<Tailorchilddata> objchilddata { get; set; }

    }
}