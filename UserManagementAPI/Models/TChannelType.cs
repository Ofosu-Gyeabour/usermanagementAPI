using System;
using System.Collections.Generic;

namespace UserManagementAPI.Models
{
    public partial class TChannelType
    {
        public TChannelType()
        {
            TClients = new HashSet<TClient>();
            TCompDigiOutlets = new HashSet<TCompDigiOutlet>();
        }

        /// <summary>
        /// primary key of table
        /// </summary>
        public int ChannelTypeId { get; set; }
        /// <summary>
        /// the type of channel (i.e. fb, twitter, tiktok, instagram, etc)
        /// </summary>
        public string? Channel { get; set; }
        /// <summary>
        /// brief description of channel type
        /// </summary>
        public string? Describ { get; set; }

        public virtual ICollection<TClient> TClients { get; set; }
        public virtual ICollection<TCompDigiOutlet> TCompDigiOutlets { get; set; }
    }
}
