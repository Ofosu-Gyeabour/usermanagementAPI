using System;
using System.Collections.Generic;

namespace UserManagementAPI.Models
{
    public partial class Tshippingport
    {
        /// <summary>
        /// primary key
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// name of port
        /// </summary>
        public string? NameOfport { get; set; }
        /// <summary>
        /// country Id
        /// </summary>
        public int? CountryId { get; set; }
        /// <summary>
        /// port code
        /// </summary>
        public string? Portcode { get; set; }
        /// <summary>
        /// days it will take to travel
        /// </summary>
        public int? TraveltimeInDays { get; set; }
    }
}
