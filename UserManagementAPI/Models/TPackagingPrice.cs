using System;
using System.Collections.Generic;

namespace UserManagementAPI.Models
{
    public partial class TPackagingPrice
    {
        /// <summary>
        /// primary key
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// ID of the packaging item
        /// </summary>
        public int? PackagingItemId { get; set; }
        /// <summary>
        /// unit price for packaging item
        /// </summary>
        public decimal? UnitPrice { get; set; }
        /// <summary>
        /// wholesale price for packaging item
        /// </summary>
        public decimal? WholesalePrice { get; set; }
        /// <summary>
        /// recommended retail price for packaging item
        /// </summary>
        public decimal? RetailPrice { get; set; }
        /// <summary>
        /// the company selling these packaging items
        /// </summary>
        public int? CompanyId { get; set; }

        public virtual Tcompany? Company { get; set; }
        public virtual TPackagingItem? PackagingItem { get; set; }
    }
}
