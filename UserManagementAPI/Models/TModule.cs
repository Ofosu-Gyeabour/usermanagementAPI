using System;
using System.Collections.Generic;

namespace UserManagementAPI.Models
{
    public partial class TModule
    {
        /// <summary>
        /// primary key of the module table
        /// </summary>
        public int ModuleId { get; set; }
        /// <summary>
        /// name of the module
        /// </summary>
        public string? SysName { get; set; }
        /// <summary>
        /// description of the module
        /// </summary>
        public string? PublicName { get; set; }
        /// <summary>
        /// flag indicating if module is in use
        /// </summary>
        public int? InUse { get; set; }
        public DateTime? DteAdded { get; set; }
    }
}
