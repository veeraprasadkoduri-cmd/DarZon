using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DarZon.Models
{
    public class Invoicemodel
    {
        public InvoiceHead invhead { get; set; }
        public List<Invoicechild> objinvlist { get; set; }
    }
}