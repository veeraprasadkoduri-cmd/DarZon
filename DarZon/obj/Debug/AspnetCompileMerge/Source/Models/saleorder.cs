using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DarZon.Models
{
    public class saleorder
    {
        public SaleOrderHeader objsaleorderHeader { get; set;}
        public List<SaleorderwithMeasurement> objlistsalDetails { get; set; }
        public string custPhoneNo { get; set; }

    }
}