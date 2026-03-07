using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DarZon.Models
{
    public class Statusdabrdmain
    {
        public Tailorassignmentdates Objdate { get; set; }

        public ICollection<StatusDashboard> objchilddata { get; set; }
    }
}