using System;
using System.Collections.Generic;

namespace UserManagementAPI.Models
{
    public partial class TLoadContainerDocLookup
    {
        public TLoadContainerDocLookup()
        {
            TLoadContainerDocValues = new HashSet<TLoadContainerDocValue>();
        }

        public int Id { get; set; }
        /// <summary>
        /// AGENT, AGENT EMAIL, VESSEL REF, etc
        /// </summary>
        public string? DocType { get; set; }

        public virtual ICollection<TLoadContainerDocValue> TLoadContainerDocValues { get; set; }
    }
}
