using System;
using System.Collections.Generic;

namespace UserManagementAPI.Models
{
    public partial class TShippingOrderStatus
    {
        /// <summary>
        /// primary key
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// description of the status
        /// </summary>
        public string? StatusDescription { get; set; }
    }
}
