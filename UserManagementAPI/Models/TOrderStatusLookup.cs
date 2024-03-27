using System;
using System.Collections.Generic;

namespace UserManagementAPI.Models
{
    public partial class TOrderStatusLookup
    {
        public TOrderStatusLookup()
        {
            TOrderStatuses = new HashSet<TOrderStatus>();
            TShippings = new HashSet<TShipping>();
            TpackagingOrders = new HashSet<TpackagingOrder>();
        }

        /// <summary>
        /// primary key
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// lookup value: APPROVED , CANCELLED
        /// </summary>
        public string? Describ { get; set; }

        public virtual ICollection<TOrderStatus> TOrderStatuses { get; set; }
        public virtual ICollection<TShipping> TShippings { get; set; }
        public virtual ICollection<TpackagingOrder> TpackagingOrders { get; set; }
    }
}
