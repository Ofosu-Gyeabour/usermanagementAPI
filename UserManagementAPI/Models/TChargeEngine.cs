using System;
using System.Collections.Generic;

namespace UserManagementAPI.Models
{
    public partial class TChargeEngine
    {
        /// <summary>
        /// primary key
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// the Id of the order type
        /// </summary>
        public int? OrdertypeId { get; set; }
        /// <summary>
        /// description of the order
        /// </summary>
        public int? ChargeId { get; set; }
        /// <summary>
        /// the rate applied
        /// </summary>
        public decimal? ChargeRate { get; set; }
        /// <summary>
        /// amount for threshold to kick in
        /// </summary>
        public decimal? ThresholdAmt { get; set; }
        /// <summary>
        /// threshold rate
        /// </summary>
        public decimal? ThresholdRate { get; set; }
        /// <summary>
        /// label or calc
        /// </summary>
        public int? IsLabel { get; set; }

        public virtual TChargeLookup? Charge { get; set; }
        public virtual TOrderType? Ordertype { get; set; }
    }
}
