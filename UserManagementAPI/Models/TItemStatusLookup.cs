using System;
using System.Collections.Generic;

namespace UserManagementAPI.Models
{
    public partial class TItemStatusLookup
    {
        public TItemStatusLookup()
        {
            TShippingOrderItems = new HashSet<TShippingOrderItem>();
            TpackagingOrderItems = new HashSet<TpackagingOrderItem>();
        }

        /// <summary>
        /// primary key
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// status of item (ordered, approved, etc)
        /// </summary>
        public string? ItemStatusDescrib { get; set; }

        public virtual ICollection<TShippingOrderItem> TShippingOrderItems { get; set; }
        public virtual ICollection<TpackagingOrderItem> TpackagingOrderItems { get; set; }
    }
}
