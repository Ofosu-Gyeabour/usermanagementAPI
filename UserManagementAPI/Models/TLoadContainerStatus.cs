using System;
using System.Collections.Generic;

namespace UserManagementAPI.Models
{
    public partial class TLoadContainerStatus
    {
        public TLoadContainerStatus()
        {
            TLoadContainers = new HashSet<TLoadContainer>();
        }

        public int Id { get; set; }
        public string? ContainerLoadingStatus { get; set; }

        public virtual ICollection<TLoadContainer> TLoadContainers { get; set; }
    }
}
