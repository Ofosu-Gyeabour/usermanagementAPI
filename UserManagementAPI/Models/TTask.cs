using System;
using System.Collections.Generic;

namespace UserManagementAPI.Models
{
    public partial class TTask
    {
        public TTask()
        {
            TSlas = new HashSet<TSla>();
        }

        /// <summary>
        /// primary key
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// task
        /// </summary>
        public string? TaskName { get; set; }
        public string? TaskDescription { get; set; }

        public virtual ICollection<TSla> TSlas { get; set; }
    }
}
