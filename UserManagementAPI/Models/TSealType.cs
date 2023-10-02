using System;
using System.Collections.Generic;

namespace UserManagementAPI.Models
{
    public partial class TSealType
    {
        public TSealType()
        {
            TSealPrices = new HashSet<TSealPrice>();
        }

        /// <summary>
        /// primary key
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// description of seal
        /// </summary>
        public string? SealTypeDescription { get; set; }

        public virtual ICollection<TSealPrice> TSealPrices { get; set; }
    }
}
