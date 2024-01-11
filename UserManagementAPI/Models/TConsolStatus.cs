using System;
using System.Collections.Generic;

namespace UserManagementAPI.Models
{
    public partial class TConsolStatus
    {
        public TConsolStatus()
        {
            TConsolOrders = new HashSet<TConsolOrder>();
        }

        public int Id { get; set; }
        public string? Describ { get; set; }

        public virtual ICollection<TConsolOrder> TConsolOrders { get; set; }
    }
}
