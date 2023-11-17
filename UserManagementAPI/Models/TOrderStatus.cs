using System;
using System.Collections.Generic;

namespace UserManagementAPI.Models
{
    public partial class TOrderStatus
    {
        /// <summary>
        /// primary key
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// the type of order. foreign key to ordertype table
        /// </summary>
        public int? OrderTypeId { get; set; }
        /// <summary>
        /// Id of order. foreign key to adhoc, packaging or shipping table
        /// </summary>
        public int? OrderId { get; set; }
        /// <summary>
        /// the Id of the status (APPROVED, CANCELLED, etc)
        /// </summary>
        public int? OrderStatus { get; set; }
        /// <summary>
        /// the reason for the order status
        /// </summary>
        public string? Reason { get; set; }
        /// <summary>
        /// the date of the action
        /// </summary>
        public DateTime? ActionedDate { get; set; }
        /// <summary>
        /// user performing action
        /// </summary>
        public int? ActionedBy { get; set; }

        public virtual TOrderStatusLookup? OrderStatusNavigation { get; set; }
        public virtual TOrderType? OrderType { get; set; }
    }
}
