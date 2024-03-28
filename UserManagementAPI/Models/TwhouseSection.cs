﻿using System;
using System.Collections.Generic;

namespace UserManagementAPI.Models
{
    public partial class TwhouseSection
    {
        public TwhouseSection()
        {
            TAssetLocators = new HashSet<TAssetLocator>();
        }

        /// <summary>
        /// primary key
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// section of the warehouse
        /// </summary>
        public string? WhouseSection { get; set; }
        /// <summary>
        /// associated barcode
        /// </summary>
        public string? AssocBarcode { get; set; }

        public virtual ICollection<TAssetLocator> TAssetLocators { get; set; }
    }
}