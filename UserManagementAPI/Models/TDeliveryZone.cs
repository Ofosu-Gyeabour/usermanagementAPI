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
        /// zone Description
        /// </summary>
        public string? Description { get; set; }
        /// <summary>
        /// reference to country
        /// </summary>
        public int? CountryId { get; set; }

        public virtual TCountryLookup? Country { get; set; }
    }
}
