using System;
using System.Collections.Generic;

namespace UserManagementAPI.Models
{
    public partial class TjtsRoe
    {
        /// <summary>
        /// primary key
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// month of the year
        /// </summary>
        public string? Mth { get; set; }
        /// <summary>
        /// full year (i.e. 2023, 2024)
        /// </summary>
        public int? Yr { get; set; }
        /// <summary>
        /// rate of exchange to use in computation
        /// </summary>
        public decimal? Roe { get; set; }
        /// <summary>
        /// jamaican dollar
        /// </summary>
        public decimal? Jam { get; set; }
        /// <summary>
        /// computed forex to use in shipment report
        /// </summary>
        public decimal? Fx { get; set; }
    }
}
