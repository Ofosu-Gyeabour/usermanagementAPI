using System;
using System.Collections.Generic;

namespace UserManagementAPI.Models
{
    public partial class TDeliveryMethod
    {
        public TDeliveryMethod()
        {
            TD2djamaicaDeliveries = new HashSet<TD2djamaicaDelivery>();
            TD2dukDeliveries = new HashSet<TD2dukDelivery>();
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

        public virtual ICollection<TD2djamaicaDelivery> TD2djamaicaDeliveries { get; set; }
        public virtual ICollection<TD2dukDelivery> TD2dukDeliveries { get; set; }
    }
}
