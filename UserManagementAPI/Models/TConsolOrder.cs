using System;
using System.Collections.Generic;

namespace UserManagementAPI.Models
{
    public partial class TConsolOrder
    {
        public TConsolOrder()
        {
            TConsolOrderItems = new HashSet<TConsolOrderItem>();
        }

        /// <summary>
        /// primary key
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// consolidator id
        /// </summary>
        public int? ConsolId { get; set; }
        /// <summary>
        /// country of arrival
        /// </summary>
        public int? ArrivalcountryId { get; set; }
        /// <summary>
        /// port of arrival
        /// </summary>
        public int? ArrivalPortId { get; set; }
        /// <summary>
        /// the customer receiving the order at the destination
        /// </summary>
        public int? RecipientId { get; set; }
        /// <summary>
        /// system user creating order
        /// </summary>
        public int? OrderInputBy { get; set; }
        /// <summary>
        /// the date order was created or inputted
        /// </summary>
        public DateTime? OrderInputDate { get; set; }
        /// <summary>
        /// the notes coming with order
        /// </summary>
        public string? OrderNote { get; set; }
        /// <summary>
        /// the current status of the order
        /// </summary>
        public int? StatusId { get; set; }
        /// <summary>
        /// system user converting order in the main shipping module
        /// </summary>
        public int? OrderconvertedBy { get; set; }
        /// <summary>
        /// the date the order was converted in the main shipping module
        /// </summary>
        public DateTime? OrderconvertedDate { get; set; }
        /// <summary>
        /// order number (format: PCO-0000001)
        /// </summary>
        public string? ConsolOrderNo { get; set; }

        public virtual TConsolUsr? OrderInputByNavigation { get; set; }
        public virtual Tusr? OrderconvertedByNavigation { get; set; }
        public virtual TConsolStatus? Status { get; set; }
        public virtual ICollection<TConsolOrderItem> TConsolOrderItems { get; set; }
    }
}
