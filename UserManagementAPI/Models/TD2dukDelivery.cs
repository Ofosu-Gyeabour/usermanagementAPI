using System;
using System.Collections.Generic;

namespace UserManagementAPI.Models
{
    public partial class TD2dukDelivery
    {
        public int Id { get; set; }
        public int? PId { get; set; }
        public decimal? Fb1 { get; set; }
        public decimal? Fb2 { get; set; }
        public decimal? Fb3 { get; set; }
        public decimal? Fb4 { get; set; }
        public decimal? Additionalfb { get; set; }
        public int? DeliveryMethodId { get; set; }
        public int? ZoneId { get; set; }
        public int? Type { get; set; }
        public decimal? Minimum { get; set; }
        public decimal? Duty { get; set; }
        public decimal? M1 { get; set; }
        public decimal? M2 { get; set; }
        public decimal? M3 { get; set; }
        public decimal? M4 { get; set; }
        public decimal? M5 { get; set; }

        public virtual TDeliveryMethod? DeliveryMethod { get; set; }
        public virtual TParish? PIdNavigation { get; set; }
        public virtual TZone? Zone { get; set; }
    }
}
