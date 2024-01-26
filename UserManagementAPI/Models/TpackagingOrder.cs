using System;
using System.Collections.Generic;

namespace UserManagementAPI.Models
{
    public partial class TpackagingOrder
    {
        public TpackagingOrder()
        {
            TpackagingOrderCharges = new HashSet<TpackagingOrderCharge>();
            TpackagingOrderItems = new HashSet<TpackagingOrderItem>();
            TpackagingOrderPayments = new HashSet<TpackagingOrderPayment>();
        }

        public int Id { get; set; }
        public int? ClientId { get; set; }
        public int? Isinvoiced { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public int? CreatedBy { get; set; }
        public string? DeliveryNote { get; set; }
        public int? CompanyId { get; set; }
        public int? SaletypeId { get; set; }
        public string? DriverName { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public int? DeliveryTimeId { get; set; }
        public string? Contact { get; set; }
        public string? Whatsapp { get; set; }
        public string? Addr1 { get; set; }
        public string? Addr2 { get; set; }
        public string? Addr3 { get; set; }
        public string? OrderNo { get; set; }
        public int? StatusId { get; set; }

        public virtual TSaleTypeLookup? Saletype { get; set; }
        public virtual ICollection<TpackagingOrderCharge> TpackagingOrderCharges { get; set; }
        public virtual ICollection<TpackagingOrderItem> TpackagingOrderItems { get; set; }
        public virtual ICollection<TpackagingOrderPayment> TpackagingOrderPayments { get; set; }
    }
}
