using System;
using System.Collections.Generic;

namespace UserManagementAPI.Models
{
    public partial class TLoadContainer
    {
        public TLoadContainer()
        {
            TContainerItems = new HashSet<TContainerItem>();
            TLoadContainerDocValues = new HashSet<TLoadContainerDocValue>();
            TLoadContainerStatistics = new HashSet<TLoadContainerStatistic>();
        }

        /// <summary>
        /// primary key
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// the type of container
        /// </summary>
        public int? ContainerTypeId { get; set; }
        /// <summary>
        /// the name of the container
        /// </summary>
        public string? ContainerName { get; set; }
        /// <summary>
        /// the date it was created
        /// </summary>
        public DateTime? DteCreated { get; set; }
        /// <summary>
        /// the agent receiving the shipping
        /// </summary>
        public int? ReceivingAgentId { get; set; }
        /// <summary>
        /// the agent in the UK
        /// </summary>
        public int? ConsignorAgentId { get; set; }
        /// <summary>
        /// booking reference
        /// </summary>
        public string? BookingRef { get; set; }
        /// <summary>
        /// freight
        /// </summary>
        public decimal? Freight { get; set; }
        /// <summary>
        /// haulage
        /// </summary>
        public decimal? Haulage { get; set; }
        /// <summary>
        /// the volume of the container
        /// </summary>
        public decimal? ContainerVolume { get; set; }
        /// <summary>
        /// the seal number (i.e. 34433)
        /// </summary>
        public int? SealNo { get; set; }
        /// <summary>
        /// the shipping line used in booking the container
        /// </summary>
        public int? BookedwithShippingLineId { get; set; }
        /// <summary>
        /// the quantity
        /// </summary>
        public int? Qty { get; set; }
        /// <summary>
        /// the status of the container (i.e. NOT COMPLETED, COMPLETED)
        /// </summary>
        public int? ContainerStatusId { get; set; }
        /// <summary>
        /// system-generated container reference number
        /// </summary>
        public string? ContainerRef { get; set; }
        /// <summary>
        /// bar code representation (in 0s and 1s for container)
        /// </summary>
        public string? ContainerCode { get; set; }
        public DateTime? Linebillapprovaldte { get; set; }
        public DateTime? Dtdocssentagent { get; set; }
        public decimal? Shippinginvoicetotal { get; set; }
        public string? Shippinglineinvoiceno { get; set; }
        public decimal? Transtoportcost { get; set; }
        public string? Transportinvoiceno { get; set; }
        public decimal? Agentcost { get; set; }
        public decimal? Gpvalueoncontbb { get; set; }
        public string? Notevessel { get; set; }
        public bool? Sentdocs { get; set; }
        public DateTime? Sentdocsdte { get; set; }
        public int? Sentby { get; set; }
        public string? Othercostdescrib { get; set; }
        public decimal? Othercost { get; set; }
        /// <summary>
        /// Id for the sailing schedule
        /// </summary>
        public int? ScheduleId { get; set; }

        public virtual Tcompany? ConsignorAgent { get; set; }
        public virtual TLoadContainerStatus? ContainerStatus { get; set; }
        public virtual TcontainerType? ContainerType { get; set; }
        public virtual Tcompany? ReceivingAgent { get; set; }
        public virtual TSailingSchedule? Schedule { get; set; }
        public virtual ICollection<TContainerItem> TContainerItems { get; set; }
        public virtual ICollection<TLoadContainerDocValue> TLoadContainerDocValues { get; set; }
        public virtual ICollection<TLoadContainerStatistic> TLoadContainerStatistics { get; set; }
    }
}
