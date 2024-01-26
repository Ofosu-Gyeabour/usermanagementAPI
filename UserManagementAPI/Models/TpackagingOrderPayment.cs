using System;
using System.Collections.Generic;

namespace UserManagementAPI.Models
{
    public partial class TpackagingOrderPayment
    {
        public int Id { get; set; }
        public int? PackageOrderId { get; set; }
        public DateTime? PayDate { get; set; }
        public decimal? PayAmt { get; set; }
        public int? PayMethodId { get; set; }
        public string? PayReceiptNo { get; set; }
        public decimal? OutstandingAmt { get; set; }

        public virtual TpackagingOrder? PackageOrder { get; set; }
    }
}
