﻿using System;
using System.Collections.Generic;

namespace UserManagementAPI.Models
{
    public partial class Tcompany
    {
        public Tcompany()
        {
            TClients = new HashSet<TClient>();
            TDepartments = new HashSet<TDepartment>();
            TPackagingPrices = new HashSet<TPackagingPrice>();
            TPackagingStocks = new HashSet<TPackagingStock>();
            TSealPrices = new HashSet<TSealPrice>();
            Tbranches = new HashSet<Tbranch>();
            TemailConfigs = new HashSet<TemailConfig>();
            Tusrs = new HashSet<Tusr>();
        }

        /// <summary>
        /// primary key
        /// </summary>
        public int CompanyId { get; set; }
        /// <summary>
        /// name of the company
        /// </summary>
        public string? Company { get; set; }
        /// <summary>
        /// address of the company
        /// </summary>
        public string? CompanyAddress { get; set; }
        public int? CompanyTownId { get; set; }
        /// <summary>
        /// region where company is located
        /// </summary>
        public int? CompanyCountryId { get; set; }
        /// <summary>
        /// logo of company
        /// </summary>
        public string? CompanyLogo { get; set; }
        /// <summary>
        /// date of incorporation
        /// </summary>
        public DateTime? IncorporationDate { get; set; }

        public virtual TCountryLookup? CompanyCountry { get; set; }
        public virtual ICollection<TClient> TClients { get; set; }
        public virtual ICollection<TDepartment> TDepartments { get; set; }
        public virtual ICollection<TPackagingPrice> TPackagingPrices { get; set; }
        public virtual ICollection<TPackagingStock> TPackagingStocks { get; set; }
        public virtual ICollection<TSealPrice> TSealPrices { get; set; }
        public virtual ICollection<Tbranch> Tbranches { get; set; }
        public virtual ICollection<TemailConfig> TemailConfigs { get; set; }
        public virtual ICollection<Tusr> Tusrs { get; set; }
    }
}
