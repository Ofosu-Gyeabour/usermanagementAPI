using System;
using System.Collections.Generic;

namespace UserManagementAPI.Models
{
    public partial class Tclientreferralsource
    {
        /// <summary>
        /// primary key
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// referral source (newspapers, radio, etc)
        /// </summary>
        public string? ReferralSource { get; set; }
    }
}
