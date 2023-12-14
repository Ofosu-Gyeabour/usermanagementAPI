using System;
using System.Collections.Generic;

namespace UserManagementAPI.Models
{
    public partial class TShippingCharge
    {
        /// <summary>
        /// primary key
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Id of the shipping order
        /// </summary>
        public int? ShippingId { get; set; }
        /// <summary>
        /// freight rate
        /// </summary>
        public decimal? FreightRate { get; set; }
        /// <summary>
        /// nominal value of freight charge
        /// </summary>
        public decimal? Freight { get; set; }
        /// <summary>
        /// the denomination of the freight
        /// </summary>
        public int? FreightDenom { get; set; }
        /// <summary>
        /// hazmat charge
        /// </summary>
        public decimal? Hazmat { get; set; }
        /// <summary>
        /// docs charge
        /// </summary>
        public decimal? Docs { get; set; }
        /// <summary>
        /// customs charge
        /// </summary>
        public decimal? Customs { get; set; }
        /// <summary>
        /// terminal handling charge (thc)
        /// </summary>
        public decimal? TerminalHandlingCharge { get; set; }
        /// <summary>
        /// haulage charge
        /// </summary>
        public decimal? Haulage { get; set; }
        /// <summary>
        /// import license charge
        /// </summary>
        public decimal? ImportLic { get; set; }
        /// <summary>
        /// verified gross mass (VGM)
        /// </summary>
        public decimal? VerifiedGrossMass { get; set; }
        /// <summary>
        /// Id of the insurance
        /// </summary>
        public int? InsuranceId { get; set; }
        /// <summary>
        /// the nominal value of the insurance
        /// </summary>
        public decimal? InsuranceValue { get; set; }
        /// <summary>
        /// terminal haulage charge for the destination
        /// </summary>
        public decimal? DestinationThc { get; set; }
        /// <summary>
        /// C&amp;D
        /// </summary>
        public decimal? CnD { get; set; }
        /// <summary>
        /// duties
        /// </summary>
        public decimal? Duties { get; set; }
        /// <summary>
        /// packing charge
        /// </summary>
        public decimal? Packing { get; set; }
        /// <summary>
        /// wrapping charge
        /// </summary>
        public decimal? Wrapping { get; set; }
        /// <summary>
        /// DMTL Car
        /// </summary>
        public decimal? Dmtlcar { get; set; }
        /// <summary>
        /// CS charge
        /// </summary>
        public decimal? Cs { get; set; }
    }
}
