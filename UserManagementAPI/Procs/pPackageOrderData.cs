#nullable disable
namespace UserManagementAPI.Procs
{
    public class pPackageOrderData
    {
        public pPackageOrder pPackageRecord { get; set; }
        public List<pPackageOrderItem> pPackageItems { get; set; }
        public List<pPackageOrderCharge> pPackageCharges { get; set; }
        public List<pPackageOrderPayment> pPackageOrderPayments { get; set; }

        public object pPackager { get; set; } //customer on whose behalf packaging is being done

    }

    public class pPackageOrder
    {
        public int? Id { get; set; }
        public int? clientId { get; set; }
        public int? isinvoiced { get; set; }
        public DateTime? invoiceDate { get; set; }
        public int? companyId { get; set; } = 1;
        public string? company { get; set; }
        public int? saletypeid { get; set; }
        public string? saletypedescrib { get; set; } = string.Empty;
        public string? driverName { get; set; } = string.Empty;
        public DateTime? deliveryDate { get; set; } = DateTime.Now;
        public int? deliveryTimeId { get; set; }
        public string? deliverytimeDescrib { get; set; } = string.Empty;
        public string? contact { get; set; } = string.Empty;
        public string? whatsapp { get; set; } = string.Empty;
        public string? addr1 { get; set; } = string.Empty;
        public string? addr2 { get; set; } = string.Empty;
        public string? addr3 { get; set; } = string.Empty;
        public string? orderNo { get; set; } = string.Empty;
        public int? statusId { get; set; } = 1;
        public string? describ { get; set; } = string.Empty;
    }

    public class pPackageOrderItem
    {
        public int? Id { get; set; }
        public int? itemId { get; set; }
        public string? packagingitem { get; set; } = string.Empty;
        public int? qty { get; set; }
        public string? itemDescription { get; set; } = string.Empty;
        public decimal? itemPrice { get; set; } = 0m;
        public string? nomcode { get; set; } = string.Empty;

    }

    public class pPackageOrderCharge
    {
        public int? Id { get; set; }
        public int? chargeId { get; set; }
        public string? chargeDescription { get; set; } = string.Empty;
        public decimal? chargeAmt { get; set; } = 0m;
    }

    public class pPackageOrderPayment
    {
        public int? Id { get; set; }
        public DateTime? payDate { get; set; }
        public decimal? payAmt { get; set; }
        public int? payMethodId { get; set; }
        public string? payMethod { get; set; } = string.Empty;
        public string? receiptNo { get; set; } = string.Empty;
        public decimal? outstandingAmt { get; set; } = 0m;
    }
}
