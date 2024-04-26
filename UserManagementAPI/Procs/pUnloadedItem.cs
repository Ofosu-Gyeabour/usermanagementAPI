namespace UserManagementAPI.Procs
{
    public class pUnloadedItem
    {
        public pUnloadedItem()
        {
            
        }

        #region properties
        public int itemId { get; set; }
        public int recordid { get; set; }
        public string? bolNo { get; set; } = string.Empty;
        public DateTime? OrderCreationDate { get; set; }
        public string? ConsigneeName { get; set; } = string.Empty;
        public string? Consignee { get; set; } = string.Empty;
        public string? ShipperName { get; set; } = string.Empty;
        public string? Shipper { get; set; } = string.Empty;
        public decimal? itemWeight { get; set; } = 0m;
        public decimal? itemVolume { get; set; } = 0m;
        public string? marks { get; set; } = string.Empty;
        public int? qty { get; set; } = 0;
        public string? itembcode { get; set; } = string.Empty;
        public string? itemDescription { get; set; } = string.Empty;
        public int? itemstatusid { get; set; } = 0;
        public string? OrderItemstatus { get; set; } = string.Empty;
        public int? delMethodId { get; set; } = 0;
        public string? method { get; set; } = string.Empty;
        public int? OrderStatusId { get; set; } = 0;
        public string? ShippingOrderStatus { get; set; } = string.Empty;

        #endregion

    }

    public class LoadedItem
    {

        #region properties
        public int itemId { get; set; }
        public int recordid { get; set; }
        public string? bolNo { get; set; } = string.Empty;
        public DateTime? OrderCreationDate { get; set; }
        public string? ConsigneeName { get; set; } = string.Empty;
        public string? Consignee { get; set; } = string.Empty;
        public string? ShipperName { get; set; } = string.Empty;
        public string? Shipper { get; set; } = string.Empty;
        public decimal? itemWeight { get; set; } = 0m;
        public decimal? itemVolume { get; set; } = 0m;
        public string? marks { get; set; } = string.Empty;
        public int? qty { get; set; } = 0;
        public string? itembcode { get; set; } = string.Empty;
        public string? itemDescription { get; set; } = string.Empty;
        public int? itemstatusid { get; set; } = 0;
        public string? OrderItemstatus { get; set; } = string.Empty;
        public int? delMethodId { get; set; } = 0;
        public string? method { get; set; } = string.Empty;
        public int? OrderStatusId { get; set; } = 0;
        public string? ShippingOrderStatus { get; set; } = string.Empty;

        public string? containerCode { get; set; } = string.Empty;
        public string? containerName { get; set; } = string.Empty;
        #endregion

    }
}
