using System;
using System.Collections.Generic;

namespace UserManagementAPI.Models
{
    public partial class TConsolOrderItem
    {
        /// <summary>
        /// primary key
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Id of the consolidated order
        /// </summary>
        public int? ConsolOrderId { get; set; }
        /// <summary>
        /// quantity of item
        /// </summary>
        public int? Qty { get; set; }
        /// <summary>
        /// Id of the item
        /// </summary>
        public int? ItemId { get; set; }
        /// <summary>
        /// description of the item and it&apos;s contents
        /// </summary>
        public string? Describ { get; set; }
        /// <summary>
        /// weight of the item
        /// </summary>
        public decimal? ItemWgt { get; set; }
        /// <summary>
        /// volume of the item
        /// </summary>
        public decimal? ItemVol { get; set; }
        /// <summary>
        /// marks for the item
        /// </summary>
        public string? Marks { get; set; }
        /// <summary>
        /// the hscode for the item
        /// </summary>
        public string? Hscode { get; set; }

        public virtual TConsolOrder? ConsolOrder { get; set; }
    }
}
