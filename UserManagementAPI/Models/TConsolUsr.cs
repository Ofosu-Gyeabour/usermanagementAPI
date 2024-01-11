using System;
using System.Collections.Generic;

namespace UserManagementAPI.Models
{
    public partial class TConsolUsr
    {
        public TConsolUsr()
        {
            TConsolOrders = new HashSet<TConsolOrder>();
            TConsols = new HashSet<TConsol>();
        }

        /// <summary>
        /// primary key
        /// </summary>
        public int Id { get; set; }
        public string? Sname { get; set; }
        public string? Fname { get; set; }
        public string? Onames { get; set; }
        public string? ClientBusinessName { get; set; }
        /// <summary>
        /// user name
        /// </summary>
        public string? Usrname { get; set; }
        /// <summary>
        /// encrypted password
        /// </summary>
        public string? Usrpwd { get; set; }
        /// <summary>
        /// identification number of consolidator
        /// </summary>
        public int? ConsolId { get; set; }
        /// <summary>
        /// admin flag
        /// </summary>
        public int? IsAdmin { get; set; }
        /// <summary>
        /// logged flag
        /// </summary>
        public int? IsLogged { get; set; }
        /// <summary>
        /// active flag
        /// </summary>
        public int? IsActive { get; set; }
        /// <summary>
        /// system-enabled log attempt
        /// </summary>
        public int? LogAttempt { get; set; }
        /// <summary>
        /// failed attempts
        /// </summary>
        public int? FailedAttempt { get; set; }
        /// <summary>
        /// user creating account
        /// </summary>
        public int? CreatedBy { get; set; }

        public virtual TClient? Consol { get; set; }
        public virtual Tusr? CreatedByNavigation { get; set; }
        public virtual ICollection<TConsolOrder> TConsolOrders { get; set; }
        public virtual ICollection<TConsol> TConsols { get; set; }
    }
}
