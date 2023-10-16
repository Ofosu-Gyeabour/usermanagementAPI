using System;
using System.Collections.Generic;

namespace UserManagementAPI.Models
{
    public partial class TClientAddress
    {
        /// <summary>
        /// primary key
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// the Id of the client...reference from tClient
        /// </summary>
        public int? ClientId { get; set; }
        /// <summary>
        /// first address part
        /// </summary>
        public string? ClientAddr1 { get; set; }
        /// <summary>
        /// second address part
        /// </summary>
        public string? ClientAddr2 { get; set; }
        /// <summary>
        /// third address part
        /// </summary>
        public string? ClientAddr3 { get; set; }
        /// <summary>
        /// fourth address part
        /// </summary>
        public string? ClientAddr4 { get; set; }
        /// <summary>
        /// bit signalling if the address is a UK address
        /// </summary>
        public bool? IsUk { get; set; }

        public virtual TClient? Client { get; set; }
    }
}
