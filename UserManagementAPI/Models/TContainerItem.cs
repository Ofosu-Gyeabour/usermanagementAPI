using System;
using System.Collections.Generic;

namespace UserManagementAPI.Models
{
    public partial class TContainerItem
    {
        /// <summary>
        /// primary key
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// container Id
        /// </summary>
        public int? ContainerId { get; set; }
        /// <summary>
        /// shipping order id
        /// </summary>
        public int? ShippingOrderId { get; set; }
        /// <summary>
        /// order number
        /// </summary>
        public string? OrderNo { get; set; }
        /// <summary>
        /// date of order
        /// </summary>
        public string? ItemBcode { get; set; }
        public DateTime? OrderDate { get; set; }
        /// <summary>
        /// name of consignee
        /// </summary>
        public string? Consignee { get; set; }
        /// <summary>
        /// weight of item
        /// </summary>
        public decimal? Weight { get; set; }
        /// <summary>
        /// volume of item
        /// </summary>
        public decimal? Cube { get; set; }
        /// <summary>
        /// mark of item
        /// </summary>
        public string? Mark { get; set; }
        /// <summary>
        /// quantity of item
        /// </summary>
        public int? Qty { get; set; }
        /// <summary>
        /// description of item
        /// </summary>
        public string? ItmDescrib { get; set; }
        /// <summary>
        /// delivery method for item
        /// </summary>
        public string? Method { get; set; }
        /// <summary>
        /// the name of the shipper
        /// </summary>
        public string? Shipper { get; set; }

        public virtual TLoadContainer? Container { get; set; }
    }
}
