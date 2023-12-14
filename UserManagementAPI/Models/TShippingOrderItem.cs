using System;
using System.Collections.Generic;

namespace UserManagementAPI.Models
{
    public partial class TShippingOrderItem
    {
        /// <summary>
        /// primary key
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// shipping order Id
        /// </summary>
        public int? ShippingorderId { get; set; }
        /// <summary>
        /// the Item Id
        /// </summary>
        public int? ItemId { get; set; }
        /// <summary>
        /// the quantity of the items
        /// </summary>
        public int? Qty { get; set; }
        /// <summary>
        /// description of the item
        /// </summary>
        public string? ItemDescription { get; set; }
        /// <summary>
        /// weight of the item
        /// </summary>
        public decimal? ItemWeight { get; set; }
        /// <summary>
        /// volume of the item
        /// </summary>
        public decimal? ItemVolume { get; set; }
        /// <summary>
        /// unitprice of the item
        /// </summary>
        public decimal? UnitPrice { get; set; }
        /// <summary>
        /// marks
        /// </summary>
        public string? Marks { get; set; }
        /// <summary>
        /// hs code
        /// </summary>
        public string? Hscode { get; set; }
        /// <summary>
        /// lpId
        /// </summary>
        public int? LpId { get; set; }
        /// <summary>
        /// path to uploaded reference image or picture
        /// </summary>
        public string? ItemPicPath { get; set; }

        public virtual TShippingItem? Item { get; set; }
    }
}
