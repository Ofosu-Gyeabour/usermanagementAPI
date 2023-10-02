using System;
using System.Collections.Generic;

namespace UserManagementAPI.Models
{
    public partial class TAdhocType
    {
        /// <summary>
        /// primary key
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// name of adhoc type
        /// </summary>
        public string? AdhocName { get; set; }
        /// <summary>
        /// nom code of adhoc type
        /// </summary>
        public string? Nomcode { get; set; }
    }
}
