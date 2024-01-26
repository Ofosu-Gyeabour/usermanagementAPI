using System;
using System.Collections.Generic;

namespace UserManagementAPI.Models
{
    public partial class TSaleTypeLookup
    {
        public TSaleTypeLookup()
        {
            TpackagingOrders = new HashSet<TpackagingOrder>();
        }

        public int Id { get; set; }
        /// <summary>
        /// sale type
        /// </summary>
        public string? SaleTypeDescrib { get; set; }

        public virtual ICollection<TpackagingOrder> TpackagingOrders { get; set; }
    }
}
