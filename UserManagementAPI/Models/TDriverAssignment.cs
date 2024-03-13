using System;
using System.Collections.Generic;

namespace UserManagementAPI.Models
{
    public partial class TDriverAssignment
    {
        /// <summary>
        /// primary key
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// the name of the driver
        /// </summary>
        public string? DriverName { get; set; }
        /// <summary>
        /// the Id of the driver
        /// </summary>
        public int? DriverId { get; set; }
        /// <summary>
        /// the Id of the vehicle
        /// </summary>
        public int? VehicleId { get; set; }
        public bool? IsAssigned { get; set; }
        /// <summary>
        /// the date of assignment
        /// </summary>
        public DateTime? AssignmentDate { get; set; }
        /// <summary>
        /// the date on which the vehicle was returned to pool
        /// </summary>
        public DateTime? ReturnedToPool { get; set; }

        public virtual TVehiclePool? Vehicle { get; set; }
    }
}
