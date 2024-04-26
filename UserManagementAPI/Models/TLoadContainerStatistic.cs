using System;
using System.Collections.Generic;

namespace UserManagementAPI.Models
{
    public partial class TLoadContainerStatistic
    {
        /// <summary>
        /// primary key
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Id of the container
        /// </summary>
        public int? ContainerId { get; set; }
        /// <summary>
        /// statistics Id from the dbo.tContainerStatisticsLookup table
        /// </summary>
        public int? StatId { get; set; }
        /// <summary>
        /// value of the statistic for the container in question
        /// </summary>
        public decimal? StatValue { get; set; }

        public virtual TLoadContainer? Container { get; set; }
        public virtual TContainerStatisticsLookup? Stat { get; set; }
    }
}
