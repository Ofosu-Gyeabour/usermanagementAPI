using System;
using System.Collections.Generic;

namespace UserManagementAPI.Models
{
    public partial class TChargeLookup
    {
        public TChargeLookup()
        {
            TChargeEngines = new HashSet<TChargeEngine>();
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

        public virtual ICollection<TChargeEngine> TChargeEngines { get; set; }
        public virtual ICollection<TDeliveryCharge> TDeliveryCharges { get; set; }
    }
}
