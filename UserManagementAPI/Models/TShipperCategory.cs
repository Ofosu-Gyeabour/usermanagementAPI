using System;
using System.Collections.Generic;

namespace UserManagementAPI.Models
{
    public partial class TShipperCategory
    {
        /// <summary>
        /// primary key
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// category of shipping
        /// </summary>
        public string? Description { get; set; }
    }
}
