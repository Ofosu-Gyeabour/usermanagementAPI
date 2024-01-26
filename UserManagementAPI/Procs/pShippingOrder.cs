#nullable disable
namespace UserManagementAPI.Procs
{
    public class pShippingOrder
    {
        public int Id { get; set; }
        public string? bolNo { get; set; } = string.Empty;
        public string? destination { get; set; } = string.Empty;
        public string? clientName { get; set; } = string.Empty;
        public string? businessName { get; set; } = string.Empty;
        public int? total_items { get; set; } = 0;
        public decimal? freight_cost { get; set; } = 0m;
        public decimal? other_costs { get; set; } = 0m;
        public decimal? total_value { get; set; } = 0m;
    }

    public class pPackagingOrder
    {
        public int Id { get; set; }
        public string? orderNo { get; set; }
        public DateTime? deliveryDate { get; set; }
        public string? status { get; set; }
    }

    public class pSalesOrder
    {
        public int Id { get; set; }
        public string? orderNo { get; set; } = string.Empty;
        public DateTime? adhocDate { get; set; } = DateTime.Now;
        public string? adhocDescrib { get; set; } = string.Empty;
        public int? item_count { get; set; } = 0;
        public decimal? total_cost { get; set; } = 0m;
        public string? sageStatus { get; set; } = string.Empty;
    }

}
