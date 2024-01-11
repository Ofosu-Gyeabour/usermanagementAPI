using System;
using System.Collections.Generic;

namespace UserManagementAPI.Models
{
    public partial class TShippingOrderInsurance
    {
        /// <summary>
        /// primary key
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Id of shipping order
        /// </summary>
        public int? ShippingOrderId { get; set; }
        /// <summary>
        /// Id of insurance type
        /// </summary>
        public int? InsuranceTypeId { get; set; }
        /// <summary>
        /// insurance amount
        /// </summary>
        public decimal? InsuranceAmt { get; set; }
    }
}
