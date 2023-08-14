using System;
using System.Collections.Generic;

namespace UserManagementAPI.Models
{
    public partial class TCity
    {
        /// <summary>
        /// primary key
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// name of city
        /// </summary>
        public string? CityName { get; set; }
        /// <summary>
        /// Id of country
        /// </summary>
        public int? CountryId { get; set; }
    }
}
