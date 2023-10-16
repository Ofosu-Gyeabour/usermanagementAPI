using System;
using System.Collections.Generic;

namespace UserManagementAPI.Models
{
    public partial class TPaymentTerm
    {
        public TPaymentTerm()
        {
            TAdhocs = new HashSet<TAdhoc>();
        }

        /// <summary>
        /// primary key
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// payment term description (WEEKLY, MONTHLY, QUARTERLY, SEMI-ANNUALLY)
        /// </summary>
        public string? PaymentTermDescrib { get; set; }

        public virtual ICollection<TAdhoc> TAdhocs { get; set; }
    }
}
