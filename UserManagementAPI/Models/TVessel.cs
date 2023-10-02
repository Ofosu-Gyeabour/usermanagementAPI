using System;
using System.Collections.Generic;

namespace UserManagementAPI.Models
{
    public partial class TVessel
    {
        public TVessel()
        {
            TSailingSchedules = new HashSet<TSailingSchedule>();
        }

        /// <summary>
        /// primary key
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// reference to shipping Line table
        /// </summary>
        public int? ShippingLineId { get; set; }
        /// <summary>
        /// the name of the vessel
        /// </summary>
        public string? VesselName { get; set; }
        /// <summary>
        /// the flag of the vessel
        /// </summary>
        public string? VesselFlag { get; set; }

        public virtual TShippingLine? ShippingLine { get; set; }
        public virtual ICollection<TSailingSchedule> TSailingSchedules { get; set; }
    }
}
