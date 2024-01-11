using System;
using System.Collections.Generic;

namespace UserManagementAPI.Models
{
    public partial class TParish
    {
        public int Id { get; set; }
        public int? ZoneId { get; set; }
        public string? ParishName { get; set; }

        public virtual TZone? Zone { get; set; }
    }
}
