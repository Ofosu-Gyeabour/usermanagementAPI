using System;
using System.Collections.Generic;

namespace UserManagementAPI.Models
{
    public partial class TCountryLookup
    {
        public TCountryLookup()
        {
            Tcompanies = new HashSet<Tcompany>();
        }

        /// <summary>
        /// primary key of country table
        /// </summary>
        public int CountryId { get; set; }
        /// <summary>
        /// foreign key to region table
        /// </summary>
        public int? RegionId { get; set; }
        /// <summary>
        /// the name of the country
        /// </summary>
        public string? CountryName { get; set; }
        /// <summary>
        /// country code for country
        /// </summary>
        public string? CountryCode { get; set; }

        public virtual TRegionLookup? Region { get; set; }
        public virtual ICollection<Tcompany> Tcompanies { get; set; }
    }
}
