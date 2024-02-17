using System;
using System.Collections.Generic;

namespace UserManagementAPI.Models
{
    public partial class TFx
    {
        /// <summary>
        /// primary key
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// USDGBP pair
        /// </summary>
        public decimal? Usdgbp { get; set; }
        /// <summary>
        /// USDEUR pair
        /// </summary>
        public decimal? Usdeur { get; set; }
        /// <summary>
        /// valid date for fx
        /// </summary>
        public DateTime? FxDate { get; set; }
    }
}
