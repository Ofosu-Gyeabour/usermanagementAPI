using System;
using System.Collections.Generic;

namespace UserManagementAPI.Models
{
    public partial class TBarCodeGenerator
    {
        /// <summary>
        /// primary key
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// fk
        /// </summary>
        public int? BarcodeId { get; set; }
        /// <summary>
        /// generated bar code
        /// </summary>
        public string? Genbarcode { get; set; }
        /// <summary>
        /// date generated
        /// </summary>
        public DateTime? Dte { get; set; }

        public virtual TBarCodeOp? Barcode { get; set; }
    }
}
