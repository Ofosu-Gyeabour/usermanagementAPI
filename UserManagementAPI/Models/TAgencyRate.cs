using System;
using System.Collections.Generic;

namespace UserManagementAPI.Models
{
    public partial class TAgencyRate
    {
        public int Id { get; set; }
        public int? PortId { get; set; }
        public decimal? Trade { get; set; }
        public decimal? Agency { get; set; }
        public decimal? Retail { get; set; }
        public decimal? Frgt1 { get; set; }
        public decimal? Frgt2 { get; set; }
        public decimal? Frgt3 { get; set; }
        public decimal? Frgt4 { get; set; }
        public decimal? B1 { get; set; }
        public decimal? B2 { get; set; }
        public decimal? B3 { get; set; }
        public decimal? Surcharge { get; set; }
        public decimal? Minimum { get; set; }
        public int? RtypeId { get; set; }
        public int? AgentId { get; set; }

        public virtual Tshippingport? Port { get; set; }
    }
}
