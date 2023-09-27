using System;
using System.Collections.Generic;

namespace UserManagementAPI.Models
{
    public partial class TDialCode
    {
        /// <summary>
        /// primary key for the table
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// the dialling code
        /// </summary>
        public string? Code { get; set; }
        /// <summary>
        /// the associated country for the dialling code
        /// </summary>
        public int? CountryId { get; set; }

        public virtual Tcompany? Country { get; set; }
    }
}
