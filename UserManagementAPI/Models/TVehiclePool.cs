using System;
using System.Collections.Generic;

namespace UserManagementAPI.Models
{
    public partial class TVehiclePool
    {
        public TVehiclePool()
        {
            TDriverAssignments = new HashSet<TDriverAssignment>();
        }

        /// <summary>
        /// primary key
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// registration number of vehicle
        /// </summary>
        public string? RegNo { get; set; }
        /// <summary>
        /// vehicle make/model for the vehicle
        /// </summary>
        public string? VehicleMake { get; set; }
        /// <summary>
        /// flag determining if vehicle was hired
        /// </summary>
        public bool? IsHired { get; set; }
        /// <summary>
        /// the hiring of company
        /// </summary>
        public string HiredCompany { get; set; } = null!;
        /// <summary>
        /// the date of hiring
        /// </summary>
        public DateTime? HiredDate { get; set; }
        /// <summary>
        /// flag determining if vehicle is currently in use
        /// </summary>
        public bool? InUse { get; set; }
        public bool? IsAssigned { get; set; }

        public virtual ICollection<TDriverAssignment> TDriverAssignments { get; set; }
    }
}
