using System;
using System.Collections.Generic;

namespace UserManagementAPI.Models
{
    public partial class TDeliveryMethod
    {
        public TDeliveryMethod()
        {
            TDeliveryZones = new HashSet<TDeliveryZone>();
        }

        public int Id { get; set; }
        /// <summary>
        /// delivery method
        /// </summary>
        public string? Method { get; set; }
        /// <summary>
        /// delivery method description
        /// </summary>
        public string? Description { get; set; }

        public virtual ICollection<TDeliveryZone> TDeliveryZones { get; set; }
    }
}
