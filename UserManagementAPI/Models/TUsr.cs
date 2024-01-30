using System;
using System.Collections.Generic;

namespace UserManagementAPI.Models
{
    public partial class Tusr
    {
        public Tusr()
        {
            TConsolOrders = new HashSet<TConsolOrder>();
            TConsolUsrs = new HashSet<TConsolUsr>();
            TUsrDetails = new HashSet<TUsrDetail>();
        }

        /// <summary>
        /// primary key
        /// </summary>
        public int UsrId { get; set; }
        /// <summary>
        /// surname of the user
        /// </summary>
        public string? Surname { get; set; }
        /// <summary>
        /// first name of the user
        /// </summary>
        public string? Firstname { get; set; }
        /// <summary>
        /// other names of the user
        /// </summary>
        public string? Othernames { get; set; }
        /// <summary>
        /// user name 
        /// </summary>
        public string? Usrname { get; set; }
        /// <summary>
        /// password for user name
        /// </summary>
        public string? Usrpassword { get; set; }
        /// <summary>
        /// Id of company user belongs to
        /// </summary>
        public int? CompanyId { get; set; }
        /// <summary>
        /// the department the user belongs to
        /// </summary>
        public int? DepartmentId { get; set; }
        /// <summary>
        /// flag determining if user is and Administration
        /// </summary>
        public int? IsAdmin { get; set; }
        /// <summary>
        /// flag determining if user is logged
        /// </summary>
        public int? IsLogged { get; set; }
        /// <summary>
        /// determines if user is active or has been de-activated
        /// </summary>
        public int? IsActive { get; set; }
        /// <summary>
        /// the Id of the profile
        /// </summary>
        public int? ProfileId { get; set; }
        public int? Lockattempt { get; set; }
        public int? Invalidattempt { get; set; }

        public virtual Tcompany? Company { get; set; }
        public virtual TDepartment? Department { get; set; }
        public virtual TProfile? Profile { get; set; }
        public virtual ICollection<TConsolOrder> TConsolOrders { get; set; }
        public virtual ICollection<TConsolUsr> TConsolUsrs { get; set; }
        public virtual ICollection<TUsrDetail> TUsrDetails { get; set; }
    }
}
