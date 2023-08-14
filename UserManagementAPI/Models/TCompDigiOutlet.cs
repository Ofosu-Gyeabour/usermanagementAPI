using System;
using System.Collections.Generic;

namespace UserManagementAPI.Models
{
    public partial class TCompDigiOutlet
    {
        /// <summary>
        /// primary key of the digital channel table
        /// </summary>
        public int ChannelId { get; set; }
        /// <summary>
        /// id of the company
        /// </summary>
        public int? CompanyId { get; set; }
        /// <summary>
        /// Id of the digital channel type (i.e. fb, twitter, tiktok, etc)
        /// </summary>
        public int? ChannelTypeId { get; set; }
        /// <summary>
        /// digital handle for the company
        /// </summary>
        public string? CompanyHandle { get; set; }

        public virtual TChannelType? ChannelType { get; set; }
    }
}
