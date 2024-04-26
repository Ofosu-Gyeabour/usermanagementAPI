using System;
using System.Collections.Generic;

namespace UserManagementAPI.Models
{
    public partial class TContainerStatisticsLookup
    {
        public TContainerStatisticsLookup()
        {
            TLoadContainerStatistics = new HashSet<TLoadContainerStatistic>();
        }

        /// <summary>
        /// primary key
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// statistics key
        /// </summary>
        public string? IdKey { get; set; }

        public virtual ICollection<TLoadContainerStatistic> TLoadContainerStatistics { get; set; }
    }
}
