using System;
using System.Collections.Generic;

namespace UserManagementAPI.Models
{
    public partial class TEvent
    {
        /// <summary>
        /// primary key
        /// </summary>
        public int Id { get; set; }
        public string? EventDescription { get; set; }
    }
}
