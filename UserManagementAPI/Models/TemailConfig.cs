using System;
using System.Collections.Generic;

namespace UserManagementAPI.Models
{
    public partial class TemailConfig
    {
        public int Id { get; set; }
        /// <summary>
        /// the Id of the company
        /// </summary>
        public int? CompanyId { get; set; }
        /// <summary>
        /// mail originator
        /// </summary>
        public string? Mfrom { get; set; }
        /// <summary>
        /// mail host
        /// </summary>
        public string? Host { get; set; }
        /// <summary>
        /// mail port
        /// </summary>
        public int? Port { get; set; }
        /// <summary>
        /// user credentail for mail
        /// </summary>
        public string? UsrCredential { get; set; }
        /// <summary>
        /// user password for mail
        /// </summary>
        public string? UsrPassword { get; set; }
        /// <summary>
        /// logo for the URL
        /// </summary>
        public string? LogoUrl { get; set; }
        /// <summary>
        /// bifa URL
        /// </summary>
        public string? BifaUrl { get; set; }
        /// <summary>
        /// receipient of mail
        /// </summary>
        public string? MTo { get; set; }
        public string? SubjectColor { get; set; }
        /// <summary>
        /// mail copied to
        /// </summary>
        public string? MCc { get; set; }
        /// <summary>
        /// mail blind copied to
        /// </summary>
        public string? MBcc { get; set; }
        /// <summary>
        /// signature of BIFA
        /// </summary>
        public string? BifaSign { get; set; }
        /// <summary>
        /// flag determining if mail account is active
        /// </summary>
        public int? IsActive { get; set; }

        public virtual Tcompany? Company { get; set; }
    }
}
