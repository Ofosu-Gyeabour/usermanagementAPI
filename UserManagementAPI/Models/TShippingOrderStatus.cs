using System;
using System.Collections.Generic;

namespace UserManagementAPI.Models
{
    public partial class TShippingOrderStatus
    {
        public TShippingOrderStatus()
        {
            TShippings = new HashSet<TShipping>();
        }

        /// <summary>
        /// primary key
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// description of the status
        /// </summary>
        public string? StatusDescription { get; set; }

        public virtual ICollection<TShipping> TShippings { get; set; }
    }
}
