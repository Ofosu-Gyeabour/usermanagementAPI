using System;
using System.Collections.Generic;

namespace UserManagementAPI.Models
{
    public partial class TLoadContainerDocValue
    {
        public int Id { get; set; }
        /// <summary>
        /// Id of the container
        /// </summary>
        public int? ContainerId { get; set; }
        /// <summary>
        /// lookup Id for the documentation on the container
        /// </summary>
        public int? TcontainerDocLookupId { get; set; }
        /// <summary>
        /// the value for the documentation value
        /// </summary>
        public string? TcontainerDocValue { get; set; }

        public virtual TLoadContainer? Container { get; set; }
        public virtual TLoadContainerDocLookup? TcontainerDocLookup { get; set; }
    }
}
