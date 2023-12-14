using System;
using System.Collections.Generic;

namespace UserManagementAPI.Models
{
    public partial class TShipping
    {
        public TShipping()
        {
            TOrderCharges = new HashSet<TOrderCharge>();
            TOrderStatuses = new HashSet<TOrderStatus>();
        }

        /// <summary>
        /// primary key
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// company id
        /// </summary>
        public int? CompanyId { get; set; }
        /// <summary>
        /// flag to determine if order is consolidated
        /// </summary>
        public bool? IsConsolidated { get; set; }
        /// <summary>
        /// consolidator description
        /// </summary>
        public string? ConsolidatorDescrib { get; set; }
        /// <summary>
        /// flag determining if order has been invoiced
        /// </summary>
        public bool? IsInvoiced { get; set; }
        /// <summary>
        /// date of invoicing
        /// </summary>
        public DateTime? InvoiceDate { get; set; }
        /// <summary>
        /// system user creating order
        /// </summary>
        public int? CreatedBy { get; set; }
        /// <summary>
        /// customer placing order
        /// </summary>
        public int? CustomerId { get; set; }
        /// <summary>
        /// Id of the consignee
        /// </summary>
        public int? ConsignorId { get; set; }
        /// <summary>
        /// Id of the receipient
        /// </summary>
        public int? ReceipientId { get; set; }
        /// <summary>
        /// Id of the notify party
        /// </summary>
        public int? NotifyPartyId { get; set; }
        /// <summary>
        /// seal quantity
        /// </summary>
        public int? SealQty { get; set; }
        /// <summary>
        /// price of seal
        /// </summary>
        public decimal? SealPrice { get; set; }
        /// <summary>
        /// route of shipment
        /// </summary>
        public int? RoutingId { get; set; }
        /// <summary>
        /// id of the delivery method
        /// </summary>
        public int? DelMethodId { get; set; }
        /// <summary>
        /// Id of the payment method
        /// </summary>
        public int? PayMethodId { get; set; }
        /// <summary>
        /// id of the arrival port
        /// </summary>
        public int? ArrivalPortId { get; set; }

        public virtual ICollection<TOrderCharge> TOrderCharges { get; set; }
        public virtual ICollection<TOrderStatus> TOrderStatuses { get; set; }
    }
}
