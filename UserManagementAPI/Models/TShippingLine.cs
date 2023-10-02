using System;
using System.Collections.Generic;

namespace UserManagementAPI.Models
{
    public partial class TShippingLine
    {
        public TShippingLine()
        {
            TVessels = new HashSet<TVessel>();
        }

        /// <summary>
        /// primary key
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// name of shipping line
        /// </summary>
        public string? ShippingLine { get; set; }

        public virtual ICollection<TVessel> TVessels { get; set; }
    }
}
