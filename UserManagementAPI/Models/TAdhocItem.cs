using System;
using System.Collections.Generic;

namespace UserManagementAPI.Models
{
    public partial class TAdhocItem
    {
        /// <summary>
        /// primary key
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// foreign key to the adhoc table
        /// </summary>
        public int? AdhocId { get; set; }
        /// <summary>
        /// quantity or unit of items
        /// </summary>
        public int? Qty { get; set; }
        /// <summary>
        /// description of sale items
        /// </summary>
        public string? Describ { get; set; }
        /// <summary>
        /// nom code
        /// </summary>
        public string? NCode { get; set; }
        /// <summary>
        /// nom code description
        /// </summary>
        public string? NCodeDescrib { get; set; }
        /// <summary>
        /// unit price of item
        /// </summary>
        public decimal? UnitPrice { get; set; }
        /// <summary>
        /// total price
        /// </summary>
        public decimal? TotalPrice { get; set; }
        /// <summary>
        /// user adding item. important should the order be modified in the future
        /// </summary>
        public int? AddedBy { get; set; }
        /// <summary>
        /// date item was added
        /// </summary>
        public DateTime? AddedDate { get; set; }

        public virtual TAdhoc? Adhoc { get; set; }
    }
}
