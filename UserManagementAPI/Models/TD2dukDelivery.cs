using System;
using System.Collections.Generic;

namespace UserManagementAPI.Models
{
    public partial class TD2dukDelivery
    {
        public int Id { get; set; }
        public decimal? B1 { get; set; }
        public decimal? B2 { get; set; }
        public decimal? B3 { get; set; }
        public decimal? B4 { get; set; }
        public decimal? AdditionalB { get; set; }
        public int? DeliveryMethodId { get; set; }
        public int? ZoneId { get; set; }
        public decimal? Duty { get; set; }

        public virtual TDeliveryMethod? DeliveryMethod { get; set; }
        public virtual TZone? Zone { get; set; }
    }
}
