using System;
using System.Collections.Generic;

namespace UserManagementAPI.Models
{
    public partial class TSla
    {
        /// <summary>
        /// primary key
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// reference to task table
        /// </summary>
        public int? TaskId { get; set; }

        public virtual TTask? Task { get; set; }
    }
}
