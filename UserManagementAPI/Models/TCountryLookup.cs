using System;
using System.Collections.Generic;

namespace UserManagementAPI.Models
{
    public partial class TCountryLookup
    {
        public TCountryLookup()
        {
            TAirports = new HashSet<TAirport>();
            TChargeLookups = new HashSet<TChargeLookup>();
            TDeliveryCharges = new HashSet<TDeliveryCharge>();
            TDeliveryZones = new HashSet<TDeliveryZone>();
            TDialCodes = new HashSet<TDialCode>();
            Tcompanies = new HashSet<Tcompany>();
            Tshippingports = new HashSet<Tshippingport>();
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
        public virtual ICollection<TAirport> TAirports { get; set; }
        public virtual ICollection<TChargeLookup> TChargeLookups { get; set; }
        public virtual ICollection<TDeliveryCharge> TDeliveryCharges { get; set; }
        public virtual ICollection<TDeliveryZone> TDeliveryZones { get; set; }
        public virtual ICollection<TDialCode> TDialCodes { get; set; }
        public virtual ICollection<Tcompany> Tcompanies { get; set; }
        public virtual ICollection<Tshippingport> Tshippingports { get; set; }
    }
}
