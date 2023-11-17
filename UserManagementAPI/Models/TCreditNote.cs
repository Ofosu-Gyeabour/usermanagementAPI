using System;
using System.Collections.Generic;

namespace UserManagementAPI.Models
{
    public partial class TCreditNote
    {
        /// <summary>
        /// primary key
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// the order id: foreign key to the order table (adhoc/sales, packaging or shipping order)
        /// </summary>
        public int? OrderId { get; set; }
        /// <summary>
        /// quantity
        /// </summary>
        public int? Qty { get; set; }
        /// <summary>
        /// description given in credit note
        /// </summary>
        public string? NoteDescrib { get; set; }
        /// <summary>
        /// the line amount for the credit note
        /// </summary>
        public decimal? Lineamount { get; set; }
        /// <summary>
        /// date for the credit note
        /// </summary>
        public DateTime? CreditNoteDate { get; set; }
        /// <summary>
        /// the user creating the credit note
        /// </summary>
        public int? CreatedBy { get; set; }
        /// <summary>
        /// the user approving the credit note
        /// </summary>
        public int? ApprovedBy { get; set; }
        /// <summary>
        /// flag determining if credit note has been uploaded to SAGE
        /// </summary>
        public bool? UploadedToSage { get; set; }
    }
}
