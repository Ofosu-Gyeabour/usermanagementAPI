using System;
using System.Collections.Generic;

namespace UserManagementAPI.Models
{
    public partial class TShippingConsigneeItem
    {
        /// <summary>
        /// primary key
        /// </summary>
        public int Id { get; set; }
        public int? ShippingOrderId { get; set; }
        /// <summary>
        /// value of item
        /// </summary>
        public decimal? ItemValue { get; set; }
        /// <summary>
        /// seal number
        /// </summary>
        public string? SealNo { get; set; }
        /// <summary>
        /// reference of the customer
        /// </summary>
        public string? CustomerRef { get; set; }
        /// <summary>
        /// input date
        /// </summary>
        public DateTime? InputDate { get; set; }
        /// <summary>
        /// ship by date. The date by which shipment should have been done
        /// </summary>
        public DateTime? LatestshippingDate { get; set; }
        /// <summary>
        /// bill of laden freight amount
        /// </summary>
        public decimal? Blfreight { get; set; }
        /// <summary>
        /// Id determining freight payable status
        /// </summary>
        public int? FreightPayableId { get; set; }

        public virtual TShipping? ShippingOrder { get; set; }
    }
}
