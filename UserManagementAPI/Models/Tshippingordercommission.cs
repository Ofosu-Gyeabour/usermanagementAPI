using System;
using System.Collections.Generic;

namespace UserManagementAPI.Models
{
    public partial class Tshippingordercommission
    {
        public int Id { get; set; }
        public int Orderid { get; set; }
        public decimal? Wifduties { get; set; }
        public decimal? Jtsduties { get; set; }
        public decimal? Earningsonduties { get; set; }
        public decimal? Wifcd { get; set; }
        public decimal? Jtscd { get; set; }
        public decimal? Earningsoncd { get; set; }
        public decimal? Cbm { get; set; }
    }
}
