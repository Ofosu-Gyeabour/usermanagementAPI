using System;
using System.Collections.Generic;

namespace UserManagementAPI.Models
{
    public partial class TDepartment
    {
        public TDepartment()
        {
            Tusrs = new HashSet<Tusr>();
        }

        /// <summary>
        /// primary key
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// name of department
        /// </summary>
        public string? DepartmentName { get; set; }
        /// <summary>
        /// the Id of the company
        /// </summary>
        public int? CompanyId { get; set; }
        /// <summary>
        /// the description of the company
        /// </summary>
        public string? Describ { get; set; }

        public virtual Tcompany? Company { get; set; }
        public virtual ICollection<Tusr> Tusrs { get; set; }
    }
}
