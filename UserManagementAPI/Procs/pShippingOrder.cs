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

    public class pClient
    {
        public int uniqueID { get; set; }
        public int? clientTypeId { get; set; } = 0;
        public string? clientType { get; set; } = string.Empty;
        public string? associatedCompany { get; set; } = string.Empty;
        public int? associatedCompanyId { get; set; } = 0;
        public string? channelType { get; set; } = string.Empty;
        public int? channelTypeId { get; set; } = 0;
        public string? firstname { get; set; } = string.Empty;
        public string? middlenames { get; set; } = string.Empty;
        public string? surname { get; set; } = string.Empty;
        public string? clientBusiness { get; set; } = string.Empty;
        public string? mobileNo { get; set; } = string.Empty;
        public string? whatsappNo { get; set; } = string.Empty;
        public string? homeTel { get; set; } = string.Empty;
        public string? workTel { get; set; } = string.Empty;
        public string? emailAddr { get; set; } = string.Empty;
        public string? emailAddr2 { get; set; } = string.Empty;
        public string? accNo { get; set; } = string.Empty;
        public int? cityId { get; set; } = 10000;
        public string? city { get; set; } = string.Empty;
        public int? countryId { get; set; } = 10000;
        public string? nameOfcountry { get; set; } = string.Empty;
        public string? postCode { get; set; } = string.Empty;
        public int? referralId { get; set; } = 0;
        public string? referral { get; set; } = string.Empty;
        public string? collectionInstruction { get; set; } = string.Empty;
        
    }

    

}
