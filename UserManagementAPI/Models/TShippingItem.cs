using System;
using System.Collections.Generic;

namespace UserManagementAPI.Models
{
    public partial class TShippingItem
    {
        public TShippingItem()
        {
            TShippingOrderItems = new HashSet<TShippingOrderItem>();
        }

        public int Id { get; set; }
        public string? ItemName { get; set; }
        public string? ItemDescription { get; set; }
        public int? CountryId { get; set; }
        public decimal? ItemPrice { get; set; }
        public string? PluralName { get; set; }

        public virtual TCountryLookup? Country { get; set; }
        public virtual ICollection<TShippingOrderItem> TShippingOrderItems { get; set; }
    }
}
