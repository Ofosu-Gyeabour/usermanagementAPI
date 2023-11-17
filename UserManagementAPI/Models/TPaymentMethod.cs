using System;
using System.Collections.Generic;

namespace UserManagementAPI.Models
{
    public partial class TPaymentMethod
    {
        /// <summary>
        /// primary key
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// payment method
        /// </summary>
        public string? Method { get; set; }
        /// <summary>
        /// flag: find out more
        /// </summary>
        public bool? IsAccnt { get; set; }
    }
}
