using System;
using System.Collections.Generic;

namespace UserManagementAPI.Models
{
    public partial class TShippingMethod
    {
        /// <summary>
        /// primary key
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// shipping method
        /// </summary>
        public string? Method { get; set; }
        /// <summary>
        /// route to use (Air, Land, Sea, etc)
        /// </summary>
        public string? Route { get; set; }
    }
}
