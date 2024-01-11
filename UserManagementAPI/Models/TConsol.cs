using System;
using System.Collections.Generic;

namespace UserManagementAPI.Models
{
    public partial class TConsol
    {
        public int Id { get; set; }
        /// <summary>
        /// surname of client
        /// </summary>
        public string? Sname { get; set; }
        /// <summary>
        /// first name of client
        /// </summary>
        public string? Fname { get; set; }
        /// <summary>
        /// middle names of client
        /// </summary>
        public string? Middlenames { get; set; }
        /// <summary>
        /// business Name of client
        /// </summary>
        public string? ClientBusinessName { get; set; }
        /// <summary>
        /// post code of client
        /// </summary>
        public string? ClientPostCode { get; set; }
        /// <summary>
        /// address1 of client
        /// </summary>
        public string? ClientAddress1 { get; set; }
        /// <summary>
        /// address2 of client
        /// </summary>
        public string? ClientAddress2 { get; set; }
        /// <summary>
        /// third address of client
        /// </summary>
        public string? ClientAddress3 { get; set; }
        /// <summary>
        /// mobile number of the client
        /// </summary>
        public string? MobileNo { get; set; }
        /// <summary>
        /// whatsapp number
        /// </summary>
        public string? WhatsappNo { get; set; }
        /// <summary>
        /// primary email address of the consolidator&apos;s client
        /// </summary>
        public string? ClientEmailAddr { get; set; }
        /// <summary>
        /// secondary email address of the client
        /// </summary>
        public string? ClientEmailAddr2 { get; set; }
        /// <summary>
        /// Id of the country
        /// </summary>
        public int? ClientCountryId { get; set; }
        /// <summary>
        /// Id of the city
        /// </summary>
        public int? ClientCityId { get; set; }
        /// <summary>
        /// Id of the consolidator
        /// </summary>
        public int? ConsolId { get; set; }
        /// <summary>
        /// checks if the address is a UK - domiciled location
        /// </summary>
        public bool? IsUk { get; set; }
        public int? Inputter { get; set; }

        public virtual TConsolUsr? InputterNavigation { get; set; }
    }
}
