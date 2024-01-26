using System;
using System.Collections.Generic;

namespace UserManagementAPI.Models
{
    public partial class TpackagingOrderItem
    {
        public int Id { get; set; }
        /// <summary>
        /// foreign key to packaging order table
        /// </summary>
        public int? PackageOrderId { get; set; }
        public int? ItemId { get; set; }
        public int? Qty { get; set; }
        public string? ItemDescription { get; set; }
        public decimal? ItemPrice { get; set; }
        public string? NomCode { get; set; }

        public virtual TpackagingOrder? PackageOrder { get; set; }
    }
}
