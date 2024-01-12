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
            TShippingOrderCharges = new HashSet<TShippingOrderCharge>();
            TShippingOrderInsurances = new HashSet<TShippingOrderInsurance>();
            TShippingOrderItems = new HashSet<TShippingOrderItem>();
            TShippingOrderPackageItems = new HashSet<TShippingOrderPackageItem>();
            TShippingOrderPayments = new HashSet<TShippingOrderPayment>();
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
        /// <summary>
        /// contact instruction for the order
        /// </summary>
        public string? ContactInstr { get; set; }
        /// <summary>
        /// note pertaining to the order (commonly called order note)
        /// </summary>
        public string? OrderNote { get; set; }
        /// <summary>
        /// cargo description
        /// </summary>
        public string? CargoDescr { get; set; }
        /// <summary>
        /// the date on which the order was created
        /// </summary>
        public DateTime? OrderCreationDate { get; set; }
        /// <summary>
        /// status of the shipping order. foreign key to the dbo.tshippingorderstatus table
        /// </summary>
        public int? OrderStatusId { get; set; }
        public string? BolNo { get; set; }

        public virtual Tshippingport? ArrivalPort { get; set; }
        public virtual TShippingOrderStatus? OrderStatus { get; set; }
        public virtual ICollection<TOrderCharge> TOrderCharges { get; set; }
        public virtual ICollection<TOrderStatus> TOrderStatuses { get; set; }
        public virtual ICollection<TShippingOrderCharge> TShippingOrderCharges { get; set; }
        public virtual ICollection<TShippingOrderInsurance> TShippingOrderInsurances { get; set; }
        public virtual ICollection<TShippingOrderItem> TShippingOrderItems { get; set; }
        public virtual ICollection<TShippingOrderPackageItem> TShippingOrderPackageItems { get; set; }
        public virtual ICollection<TShippingOrderPayment> TShippingOrderPayments { get; set; }
    }
}
