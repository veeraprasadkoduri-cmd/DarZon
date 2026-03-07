using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DarZon.Models
{
    public class SapSaleorder
    {
        public SaleOrderHeader objsaleorder { get; set; }
        public List<SaleOrderDetail> objsaleorderdetails { get; set; }

    }
}