using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DarZon.Models
{
    public class Activity
    {
        public string Customercode { get; set; }
        public string Customername { get; set; }
        public string mobileno { get; set; }
        public string Activitytype { get; set; }
        public int Actvityno { get; set; }
        public string Assignempid { get; set; }
        public String Assignby { get; set; }
        public string Startdate { get; set; }
        public string enddate { get; set; }
        public string saleorder { get; set; }
        public string starttimeto { get; set; }
        public string Endtime { get; set; }
        public string status { get; set; }
        public string custcomments { get; set; }
        public string operatorcomments { get; set; }
        public string websaleorder { get; set; }

    }
}