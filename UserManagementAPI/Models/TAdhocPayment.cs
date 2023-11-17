using System;
using System.Collections.Generic;

namespace UserManagementAPI.Models
{
    public partial class TAdhocPayment
    {
        /// <summary>
        /// primary key
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// adhoc Id: foreign key to the adhoc table
        /// </summary>
        public int? AdhocId { get; set; }
        /// <summary>
        /// date of payment
        /// </summary>
        public DateTime? PayDate { get; set; }
        /// <summary>
        /// amount of payment
        /// </summary>
        public decimal? PayAmt { get; set; }
        /// <summary>
        /// method of payment
        /// </summary>
        public int? PayMethodId { get; set; }
        /// <summary>
        /// amount left outstanding
        /// </summary>
        public decimal? OutstandingAmt { get; set; }

        public virtual TAdhoc? Adhoc { get; set; }
    }
}
