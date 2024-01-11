using System;
using System.Collections.Generic;

namespace UserManagementAPI.Models
{
    public partial class TShippingOrderCharge
    {
        /// <summary>
        /// primary key
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// shipping order Id
        /// </summary>
        public int? ShippingOrderId { get; set; }
        /// <summary>
        /// the Id of the charge being applied
        /// </summary>
        public int? ChargeId { get; set; }
        /// <summary>
        /// the amount being applied
        /// </summary>
        public decimal? ChargeAmt { get; set; }
        /// <summary>
        /// the description of the charge
        /// </summary>
        public string? ChargeDescription { get; set; }
        /// <summary>
        /// the Id of the currency of the charges..from the currency lookup
        /// </summary>
        public int? CurrencyId { get; set; }
    }
}
