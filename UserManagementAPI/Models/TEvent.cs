using System;
using System.Collections.Generic;

namespace UserManagementAPI.Models
{
    public partial class TEvent
    {
        public TEvent()
        {
            TLoggers = new HashSet<TLogger>();
        }

        /// <summary>
        /// primary key
        /// </summary>
        public int Id { get; set; }
        public string? EventDescription { get; set; }

        public virtual ICollection<TLogger> TLoggers { get; set; }
    }
}
