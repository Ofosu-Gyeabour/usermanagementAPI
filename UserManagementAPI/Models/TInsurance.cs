using System;
using System.Collections.Generic;

namespace UserManagementAPI.Models
{
    public partial class TInsurance
    {
        public int Id { get; set; }
        /// <summary>
        /// type of insurance
        /// </summary>
        public int? InsuranceTypeId { get; set; }
        /// <summary>
        /// description of insurance
        /// </summary>
        public string? Description { get; set; }
        /// <summary>
        /// unit price of insurance
        /// </summary>
        public decimal? UnitPrice { get; set; }

        public virtual TInsuranceType? InsuranceType { get; set; }
    }
}
