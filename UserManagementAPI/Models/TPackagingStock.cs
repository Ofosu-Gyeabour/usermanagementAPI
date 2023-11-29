using System;
using System.Collections.Generic;

namespace UserManagementAPI.Models
{
    public partial class TPackagingStock
    {
        /// <summary>
        /// primary id of key
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// ID of packaging item
        /// </summary>
        public int? TpackagingItemId { get; set; }
        /// <summary>
        /// current stock level for packaging item
        /// </summary>
        public int? InStock { get; set; }
        /// <summary>
        /// floor threshold for packaging item. when breached, trigger a mail and alert responsible parties
        /// </summary>
        public int? FloorThreshold { get; set; }
        /// <summary>
        /// maximum stock to be kept for item. alert responsible parties when breached
        /// </summary>
        public int? CeilingThreshold { get; set; }
        /// <summary>
        /// reference to the company
        /// </summary>
        public int? CompanyId { get; set; }

        public virtual Tcompany? Company { get; set; }
        public virtual TPackagingItem? TpackagingItem { get; set; }
    }
}
