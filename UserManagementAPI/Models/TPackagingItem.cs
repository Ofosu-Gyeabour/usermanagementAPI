using System;
using System.Collections.Generic;

namespace UserManagementAPI.Models
{
    public partial class TPackagingItem
    {
        public TPackagingItem()
        {
            TPackagingPrices = new HashSet<TPackagingPrice>();
        }

        /// <summary>
        /// primary key of the table
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// packaging Item
        /// </summary>
        public string? PackagingItem { get; set; }
        /// <summary>
        /// packaging description
        /// </summary>
        public string? PackagingDescription { get; set; }

        public virtual ICollection<TPackagingPrice> TPackagingPrices { get; set; }
    }
}
