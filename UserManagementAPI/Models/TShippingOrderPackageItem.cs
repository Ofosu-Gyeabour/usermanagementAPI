using System;
using System.Collections.Generic;

namespace UserManagementAPI.Models
{
    public partial class TShippingOrderPackageItem
    {
        /// <summary>
        /// primary key
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Id of the shipping order
        /// </summary>
        public int? ShippingOrderId { get; set; }
        /// <summary>
        /// Id of item being packaged
        /// </summary>
        public int? ItemId { get; set; }
        /// <summary>
        /// quantity of item being packed
        /// </summary>
        public int? Qty { get; set; }
        /// <summary>
        /// description given
        /// </summary>
        public string? Describ { get; set; }
        /// <summary>
        /// price of item being packed (usually retail price)
        /// </summary>
        public decimal? ItemPrice { get; set; }
        /// <summary>
        /// nomcode of item being packed
        /// </summary>
        public string? NomCode { get; set; }
        /// <summary>
        /// Id of user adding package item
        /// </summary>
        public int? AddedBy { get; set; }
        /// <summary>
        /// the date the package item was added
        /// </summary>
        public DateTime? AddedDate { get; set; }
    }
}
