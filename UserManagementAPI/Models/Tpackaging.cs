using System;
using System.Collections.Generic;

namespace UserManagementAPI.Models
{
    public partial class Tpackaging
    {
        /// <summary>
        /// primary key
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// packaging item
        /// </summary>
        public string? Packagingitem { get; set; }
        /// <summary>
        /// brief description of packaging item
        /// </summary>
        public string? Itemdescription { get; set; }
        /// <summary>
        /// dimensions of packaging item
        /// </summary>
        public string? Dimension { get; set; }
        /// <summary>
        /// unit price for packaging item
        /// </summary>
        public decimal? Unitprice { get; set; }
        /// <summary>
        /// wholesale price for packaging item
        /// </summary>
        public decimal? Wholesaleprice { get; set; }
        /// <summary>
        /// rrp of packaging item. Find out what rrp means
        /// </summary>
        public decimal? Rrp { get; set; }
        /// <summary>
        /// unit of items in stock. gets updated as items gets used
        /// </summary>
        public int? Instock { get; set; }
        /// <summary>
        /// code of the item: to differentiate it when two companies have the same items
        /// </summary>
        public string? Itemcode { get; set; }
        /// <summary>
        /// the Id of the company having the packaging item
        /// </summary>
        public int? CompanyId { get; set; }
        /// <summary>
        /// the threshold at which a notification should be sent to management
        /// </summary>
        public int? Stockthreshold { get; set; }
        public string? PluralName { get; set; }
    }
}
