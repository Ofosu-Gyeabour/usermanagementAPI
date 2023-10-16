using System;
using System.Collections.Generic;

namespace UserManagementAPI.Models
{
    public partial class Tclientreferralsource
    {
        public Tclientreferralsource()
        {
            TClients = new HashSet<TClient>();
        }

        /// <summary>
        /// primary key
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// referral source (newspapers, radio, etc)
        /// </summary>
        public string? ReferralSource { get; set; }

        public virtual ICollection<TClient> TClients { get; set; }
    }
}
