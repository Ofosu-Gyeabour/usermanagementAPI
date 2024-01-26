using System;
using System.Collections.Generic;

namespace UserManagementAPI.Models
{
    public partial class TpackagingOrderCharge
    {
        public int Id { get; set; }
        public int? PackageOrderId { get; set; }
        public int? ChargeId { get; set; }
        public decimal? ChargeAmt { get; set; }
        public string? ChargeDescription { get; set; }
        public int? CurrencyId { get; set; }

        public virtual TpackagingOrder? PackageOrder { get; set; }
    }
}
