using System;
using System.Collections.Generic;

namespace UserManagementAPI.Models
{
    public partial class TClientType
    {
        public TClientType()
        {
            TClients = new HashSet<TClient>();
        }

        /// <summary>
        /// primary key
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// description of client type
        /// </summary>
        public string? Describ { get; set; }

        public virtual ICollection<TClient> TClients { get; set; }
    }
}
