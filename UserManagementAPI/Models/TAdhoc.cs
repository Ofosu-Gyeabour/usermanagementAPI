using System;
using System.Collections.Generic;

namespace UserManagementAPI.Models
{
    public partial class TAdhoc
    {
        public int Id { get; set; }
        /// <summary>
        /// reference to the adhoc type table
        /// </summary>
        public int? AdhocTypeId { get; set; }
        /// <summary>
        /// reference to the company table
        /// </summary>
        public int? CompanyId { get; set; }
        /// <summary>
        /// flag determining if transaction has been invoiced
        /// </summary>
        public int? IsInvoiced { get; set; }
        /// <summary>
        /// date of invoicing
        /// </summary>
        public DateTime? InvoiceDate { get; set; }
        /// <summary>
        /// user creating transaction record
        /// </summary>
        public int? CreatedBy { get; set; }
        /// <summary>
        /// the Id of the client
        /// </summary>
        public int? ClientId { get; set; }
        /// <summary>
        /// description of adhoc txn
        /// </summary>
        public string? AdhocDescrib { get; set; }
        /// <summary>
        /// date for the adhoc txn
        /// </summary>
        public DateTime? AdhocDate { get; set; }
        /// <summary>
        /// user who last modified record
        /// </summary>
        public int? LastModifedBy { get; set; }
        /// <summary>
        /// flag determining if invoice has been uploaded to SAGE. set to automate this with SAGE 200
        /// </summary>
        public bool? IsuploadtoSage { get; set; }
        /// <summary>
        /// VAT component
        /// </summary>
        public decimal? Vat { get; set; }
        /// <summary>
        /// reference to payment terms table
        /// </summary>
        public int? PaymentTermsId { get; set; }

        public virtual TPaymentTerm? PaymentTerms { get; set; }
    }
}
