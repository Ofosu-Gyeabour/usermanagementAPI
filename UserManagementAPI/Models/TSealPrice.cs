using System;
using System.Collections.Generic;

namespace UserManagementAPI.Models
{
    public partial class TSealPrice
    {
        public int Id { get; set; }
        /// <summary>
        /// The Id of the company
        /// </summary>
        public int? CompanyId { get; set; }
        /// <summary>
        /// the type of seal
        /// </summary>
        public int? SealtypeId { get; set; }
        /// <summary>
        /// the price of the seal type
        /// </summary>
        public decimal? Price { get; set; }
        /// <summary>
        /// the selling or trading price of the seal
        /// </summary>
        public decimal? Sellingprice { get; set; }

        public virtual Tcompany? Company { get; set; }
        public virtual TSealType? Sealtype { get; set; }
    }
}
