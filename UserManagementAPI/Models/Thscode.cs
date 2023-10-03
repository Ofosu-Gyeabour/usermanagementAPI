using System;
using System.Collections.Generic;

namespace UserManagementAPI.Models
{
    public partial class Thscode
    {
        /// <summary>
        /// primary key
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// hs code
        /// </summary>
        public string? Hscode { get; set; }
        /// <summary>
        /// description of code
        /// </summary>
        public string? Description { get; set; }
    }
}
