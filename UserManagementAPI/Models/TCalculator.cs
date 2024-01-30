using System;
using System.Collections.Generic;

namespace UserManagementAPI.Models
{
    public partial class TCalculator
    {
        public int Id { get; set; }
        public int? PId { get; set; }
        public int? ZId { get; set; }
        public decimal? Frgtbar1 { get; set; }
        public decimal? Frgtbar2 { get; set; }
        public decimal? Frgtbar3 { get; set; }
        public decimal? Frgtbar4 { get; set; }
        public decimal? Frgtbar5 { get; set; }
        public decimal? Minimum { get; set; }
        public decimal? Duty { get; set; }
    }
}
