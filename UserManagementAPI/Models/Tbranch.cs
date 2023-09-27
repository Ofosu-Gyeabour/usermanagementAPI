using System;
using System.Collections.Generic;

namespace UserManagementAPI.Models
{
    public partial class Tbranch
    {
        public int Id { get; set; }
        /// <summary>
        /// name of the branch
        /// </summary>
        public string? BranchName { get; set; }
        /// <summary>
        /// Id of the company
        /// </summary>
        public int? CompanyId { get; set; }

        public virtual Tcompany? Company { get; set; }
    }
}
