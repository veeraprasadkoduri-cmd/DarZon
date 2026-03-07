using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DarZon.Models
{
    public class deliveryModel
    {
        public DeliveryHead delheqad { get; set; }
        public List<SaleOrderDetail> objsaleorder { get; set; }
    }
}