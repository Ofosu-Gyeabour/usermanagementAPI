#nullable disable
namespace UserManagementAPI.POCOs
{
    public class clsConsigneeItem
    {
        public decimal? itemValue { get; set; }
        public string? sealNumber { get; set; } = string.Empty;
        public string? customerRef { get; set; } = string.Empty;
        public DateTime? inputDate { get; set; }
        public DateTime? shipByDate { get; set; }
        public decimal? BLFreight { get; set; } = 0m;
        public int? freightPayable { get; set; } = 0;
    }
}
