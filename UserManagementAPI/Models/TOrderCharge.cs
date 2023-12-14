using System;
using System.Collections.Generic;

namespace UserManagementAPI.Models
{
    public partial class TOrderCharge
    {
        public int Id { get; set; }
        /// <summary>
        /// order id...for all orders
        /// </summary>
        public int? OrderId { get; set; }
        /// <summary>
        /// the Id of the charge
        /// </summary>
        public int? ChargeId { get; set; }
        /// <summary>
        /// the rate of the charge
        /// </summary>
        public decimal? ChargeRate { get; set; }
        /// <summary>
        /// the nominal value upon computation
        /// </summary>
        public decimal? ChargeValue { get; set; }

        public virtual TChargeLookup? Charge { get; set; }
        public virtual TShipping? Order { get; set; }
    }
}
