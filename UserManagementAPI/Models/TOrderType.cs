using System;
using System.Collections.Generic;

namespace UserManagementAPI.Models
{
    public partial class TOrderType
    {
        public TOrderType()
        {
            TOrderStatuses = new HashSet<TOrderStatus>();
        }

        /// <summary>
        /// primary key
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// description of order type: Sales Order, Packaging Order, Shipping Order, etc
        /// </summary>
        public string? Describ { get; set; }

        public virtual ICollection<TOrderStatus> TOrderStatuses { get; set; }
    }
}
