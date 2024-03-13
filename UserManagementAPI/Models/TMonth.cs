using System;
using System.Collections.Generic;

namespace UserManagementAPI.Models
{
    public partial class TMonth
    {
        /// <summary>
        /// primary key of month
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// name of month
        /// </summary>
        public string? Month { get; set; }
    }
}
