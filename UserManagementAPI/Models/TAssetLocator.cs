using System;
using System.Collections.Generic;

namespace UserManagementAPI.Models
{
    public partial class TAssetLocator
    {
        public int Id { get; set; }
        /// <summary>
        /// item bar code
        /// </summary>
        public string? Itembcode { get; set; }
        /// <summary>
        /// warehouse section
        /// </summary>
        public int? WarehouseSectionId { get; set; }
        public DateTime? LastLocationUpdate { get; set; }

        public virtual TwhouseSection? WarehouseSection { get; set; }
    }
}
