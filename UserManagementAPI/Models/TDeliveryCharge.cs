using System;
using System.Collections.Generic;

namespace UserManagementAPI.Models
{
    public partial class TDeliveryCharge
    {
        /// <summary>
        /// primary key
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// the name of the charge
        /// </summary>
        public int? ChargeId { get; set; }
        /// <summary>
        /// the charge amount
        /// </summary>
        public decimal? ChargeAmt { get; set; }
        /// <summary>
        /// the country Id
        /// </summary>
        public int? CountryId { get; set; }
        public decimal? QtyCummulativeRate { get; set; }

        public virtual TChargeLookup? Charge { get; set; }
        public virtual TCountryLookup? Country { get; set; }
    }
}
