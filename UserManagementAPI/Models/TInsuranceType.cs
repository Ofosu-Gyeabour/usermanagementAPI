using System;
using System.Collections.Generic;

namespace UserManagementAPI.Models
{
    public partial class TInsuranceType
    {
        public TInsuranceType()
        {
            TInsurances = new HashSet<TInsurance>();
        }

        /// <summary>
        /// primary key
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// type of insurance
        /// </summary>
        public string? InsuranceType { get; set; }

        public virtual ICollection<TInsurance> TInsurances { get; set; }
    }
}
