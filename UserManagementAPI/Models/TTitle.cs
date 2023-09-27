using System;
using System.Collections.Generic;

namespace UserManagementAPI.Models
{
    public partial class TTitle
    {
        /// <summary>
        /// primary key
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// title name
        /// </summary>
        public string? Title { get; set; }
    }
}
