using System;
using System.Collections.Generic;

namespace UserManagementAPI.Models
{
    public partial class TCurrencyLookup
    {
        /// <summary>
        /// primary key
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// the acronym or code of the currency
        /// </summary>
        public string? CurrencyCode { get; set; }
        /// <summary>
        /// the description (or friendly name) of the currency
        /// </summary>
        public string? CurrencyDescription { get; set; }
    }
}
