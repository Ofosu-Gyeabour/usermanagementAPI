using System;
using System.Collections.Generic;

namespace UserManagementAPI.Models
{
    public partial class TDeliveryZone
    {
        public int Id { get; set; }
        /// <summary>
        /// name of zone
        /// </summary>
        public string? Zone { get; set; }
        /// <summary>
        /// reference to delivery method
        /// </summary>
        public int? DeliverymethodId { get; set; }
        /// <summary>
        /// reference to country
        /// </summary>
        public int? CountryId { get; set; }

        public virtual TCountryLookup? Country { get; set; }
        public virtual TDeliveryMethod? Deliverymethod { get; set; }
    }
}
