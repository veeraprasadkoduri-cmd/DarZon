using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DarZon.Models
{
    public class SaleorderwithMeasurement: SaleOrderDetail
    {
        
        public List<SaleOrderDetail> addonlist { get; set; }

        public List<Mesurement> objmesurementlist { get; set; }
        public string CusPhoneNo { get; set; }

    }
}