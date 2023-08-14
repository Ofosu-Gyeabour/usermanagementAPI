using System;
using System.Collections.Generic;

namespace UserManagementAPI.Models
{
    public partial class Tusr
    {
        public Tusr()
        {
            TUsrDetails = new HashSet<TUsrDetail>();
        }

        /// <summary>
        /// primary key
        /// </summary>
        public int UsrId { get; set; }
        public string? Surname { get; set; }
        public string? Firstname { get; set; }
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

        public virtual Tcompany? Company { get; set; }
        public virtual TProfile? Profile { get; set; }
        public virtual ICollection<TUsrDetail> TUsrDetails { get; set; }
    }
}
