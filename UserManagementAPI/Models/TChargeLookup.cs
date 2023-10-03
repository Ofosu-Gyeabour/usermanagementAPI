using System;
using System.Collections.Generic;

namespace UserManagementAPI.Models
{
    public partial class TChargeLookup
    {
        public TChargeLookup()
        {
            TDeliveryCharges = new HashSet<TDeliveryCharge>();
        }

        /// <summary>
        /// primary key
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// charge
        /// </summary>
        public string? Charge { get; set; }
        /// <summary>
        /// charge per unit of item being transported
        /// </summary>
        public decimal? Unitcharge { get; set; }
        /// <summary>
        /// rate of increase of charge for quantity of items &gt; 1
        /// </summary>
        public decimal? Cumchargerate { get; set; }
        /// <summary>
        /// the country in which the charges apply
        /// </summary>
        public int? CountryId { get; set; }

        public virtual TCountryLookup? Country { get; set; }
        public virtual ICollection<TDeliveryCharge> TDeliveryCharges { get; set; }
    }
}
