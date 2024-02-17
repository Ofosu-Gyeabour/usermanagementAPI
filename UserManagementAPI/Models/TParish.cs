using System;
using System.Collections.Generic;

namespace UserManagementAPI.Models
{
    public partial class TParish
    {
        public TParish()
        {
            TD2djamaicaDeliveries = new HashSet<TD2djamaicaDelivery>();
            TD2dukDeliveries = new HashSet<TD2dukDelivery>();
        }

        public int Id { get; set; }
        public int? ZoneId { get; set; }
        public string? ParishName { get; set; }

        public virtual TZone? Zone { get; set; }
        public virtual ICollection<TD2djamaicaDelivery> TD2djamaicaDeliveries { get; set; }
        public virtual ICollection<TD2dukDelivery> TD2dukDeliveries { get; set; }
    }
}
