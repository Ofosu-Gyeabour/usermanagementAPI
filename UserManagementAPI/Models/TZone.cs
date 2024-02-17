using System;
using System.Collections.Generic;

namespace UserManagementAPI.Models
{
    public partial class TZone
    {
        public TZone()
        {
            TD2djamaicaDeliveries = new HashSet<TD2djamaicaDelivery>();
            TD2dukDeliveries = new HashSet<TD2dukDelivery>();
            TParishes = new HashSet<TParish>();
        }

        public int Id { get; set; }
        public string? ZoneName { get; set; }
        public int? CountryId { get; set; }

        public virtual TCountryLookup? Country { get; set; }
        public virtual ICollection<TD2djamaicaDelivery> TD2djamaicaDeliveries { get; set; }
        public virtual ICollection<TD2dukDelivery> TD2dukDeliveries { get; set; }
        public virtual ICollection<TParish> TParishes { get; set; }
    }
}
