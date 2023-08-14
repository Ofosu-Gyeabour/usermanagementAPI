using System;
using System.Collections.Generic;

namespace UserManagementAPI.Models
{
    public partial class TProfile
    {
        public TProfile()
        {
            Tusrs = new HashSet<Tusr>();
        }

        /// <summary>
        /// Id of the profile
        /// </summary>
        public int ProfileId { get; set; }
        /// <summary>
        /// profile string
        /// </summary>
        public string? ProfileString { get; set; }
        /// <summary>
        /// the company using the profile
        /// </summary>
        public int? CompanyId { get; set; }
        /// <summary>
        /// flag indicating whether profile is in use or not
        /// </summary>
        public int? InUse { get; set; }
        /// <summary>
        /// date profile was added
        /// </summary>
        public DateTime? DteAdded { get; set; }

        public virtual ICollection<Tusr> Tusrs { get; set; }
    }
}
