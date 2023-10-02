using System;
using System.Collections.Generic;

namespace UserManagementAPI.Models
{
    public partial class TSailingSchedule
    {
        /// <summary>
        /// primary key
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// reference to vessel table
        /// </summary>
        public int? VesselId { get; set; }
        /// <summary>
        /// reference to shipping port table. port of departure
        /// </summary>
        public int? PortOfDepartureId { get; set; }
        /// <summary>
        /// reference to shipping port table. port of arrival
        /// </summary>
        public int? PortOfArrivalId { get; set; }
        /// <summary>
        /// closing date
        /// </summary>
        public DateTime? ClosingDate { get; set; }
        /// <summary>
        /// departure date
        /// </summary>
        public DateTime? DepartureDate { get; set; }
        /// <summary>
        /// arrival date
        /// </summary>
        public DateTime? ArrivalDate { get; set; }

        public virtual TVessel? Vessel { get; set; }
    }
}
