using System;
using System.Collections.Generic;

namespace UserManagementAPI.Models
{
    public partial class TCongestionCharge
    {
        /// <summary>
        /// primary key
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// postal code
        /// </summary>
        public string? PostCode { get; set; }
        /// <summary>
        /// charge associated with the congestion
        /// </summary>
        public decimal? Charge { get; set; }
    }
}
