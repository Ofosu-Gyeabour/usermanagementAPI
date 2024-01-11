using System;
using System.Collections.Generic;

namespace UserManagementAPI.Models
{
    public partial class TShippingOrderDriver
    {
        /// <summary>
        /// primary key
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// shipping order id
        /// </summary>
        public int? ShippingOrderId { get; set; }
        /// <summary>
        /// user Id of the driver
        /// </summary>
        public int? DriverId { get; set; }
        /// <summary>
        /// date of delivery or collection
        /// </summary>
        public DateTime? Dte { get; set; }
        /// <summary>
        /// note meant for driver to read
        /// </summary>
        public string? DriverNote { get; set; }
    }
}
