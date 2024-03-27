using System;
using System.Collections.Generic;

namespace UserManagementAPI.Models
{
    public partial class TItemStatusLookup
    {
        /// <summary>
        /// primary key
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// status of item (ordered, approved, etc)
        /// </summary>
        public string? ItemStatusDescrib { get; set; }
    }
}
