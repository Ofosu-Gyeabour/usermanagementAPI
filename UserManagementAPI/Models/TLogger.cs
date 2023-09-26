using System;
using System.Collections.Generic;

namespace UserManagementAPI.Models
{
    public partial class TLogger
    {
        /// <summary>
        /// id of the log table
        /// </summary>
        public int LogId { get; set; }
        /// <summary>
        /// the type of event being logged (for the purpose of charging)
        /// </summary>
        public int? LogEvent { get; set; }
        /// <summary>
        /// the user performing whose actions are being logged
        /// </summary>
        public string? LogActor { get; set; }
        /// <summary>
        /// the entity being persisted or modified
        /// </summary>
        public string? LogEntity { get; set; }
        /// <summary>
        /// the serialized data of the entity being persisted
        /// </summary>
        public string? LogEntityValue { get; set; }
        /// <summary>
        /// the company the user belongs to
        /// </summary>
        public int? CompanyId { get; set; }
        /// <summary>
        /// the date user&apos;s action is taken place
        /// </summary>
        public DateTime? LogDate { get; set; }

        public virtual TEvent? LogEventNavigation { get; set; }
    }
}
