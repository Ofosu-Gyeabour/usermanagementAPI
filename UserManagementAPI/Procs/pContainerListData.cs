namespace UserManagementAPI.Procs
{
    public class pContainerListData
    {
        public pContainerListData()
        {
            
        }

        public int Id { get; set; }
        public int? containerTypeId { get; set; }
        public string? containerType { get; set; }
        public string containerName { get; set; }
        public string bookingRef { get; set; }
        public decimal? freight { get; set; }
        public decimal? haulage { get; set; }
        public int? sealNo { get; set; }
        public string? receivingAgent { get; set; }
        public string? consignorAgent { get; set; }
        public decimal? containerVolume { get; set; }
        public int qty { get; set; }
        public DateTime dtecreated { get; set; }
        public string? containerLoadingStatus { get; set; }
        public string? vesselName { get; set; }
        public string? shippingLine { get; set; }
        public string? DeparturePort { get; set; }
        public string? ArrivalPort { get; set; }
        public DateTime? closingDate { get; set; }
        public DateTime? departureDate { get; set; }
        public DateTime? arrivalDate { get; set; }
        public string? containerRef { get; set; }
        public string? containerCode { get; set; }
        public string? arrivalCountry { get; set; }
    }
}
