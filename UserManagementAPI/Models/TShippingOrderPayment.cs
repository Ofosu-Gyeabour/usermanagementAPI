using System;
using System.Collections.Generic;

namespace UserManagementAPI.Models
{
    public partial class TShippingOrderPayment
    {
        /// <summary>
        /// primary key
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Id of shipping order
        /// </summary>
        public int? ShippingOrderId { get; set; }
        /// <summary>
        /// date of payment
        /// </summary>
        public DateTime? PayDate { get; set; }
        /// <summary>
        /// amount of payment
        /// </summary>
        public decimal? PayAmt { get; set; }
        /// <summary>
        /// method of payment (eg: CHEQUE, BANK TRANSFER, etc)
        /// </summary>
        public int? PayMethodId { get; set; }
        /// <summary>
        /// receipt number associated with payment
        /// </summary>
        public string? PayReceiptNo { get; set; }
        /// <summary>
        /// outstanding amount left for payment
        /// </summary>
        public decimal? OutstandingAmt { get; set; }
    }
}
