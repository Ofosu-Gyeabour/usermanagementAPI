﻿using System;
using System.Collections.Generic;

namespace UserManagementAPI.Models
{
    public partial class TcontainerType
    {
        public TcontainerType()
        {
            TLoadContainers = new HashSet<TLoadContainer>();
        }

        /// <summary>
        /// primary key
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// container type
        /// </summary>
        public string? Ctype { get; set; }
        /// <summary>
        /// container volume
        /// </summary>
        public decimal? Cvolume { get; set; }

        public virtual ICollection<TLoadContainer> TLoadContainers { get; set; }
    }
}
