namespace UserManagementAPI.POCOs
{
    public class Sale
    {
        public int id { get; set; } = 0;
        public AdhocTypeLookup? oAdhocType { get; set; }
        public CompanyLookup? oCompany { get; set; }

        public int? isInvoiced { get; set; } = 0;
        public DateTime? invoiceDate { get; set; } = DateTime.Now;
        public int createdBy { get; set; } = 1057;
        public int clientId { get; set; }
        public string? adhocDescrib { get; set; } = string.Empty;
        public DateTime? adhocDate { get; set; } = DateTime.Now;
        public int? lastModifiedBy { get; set; } = 1057;
        public bool isuploadtoSage { get; set; } = false;
        public decimal vat { get; set; } = 0m;
        public PaymentTermLookup? oPaymentTerm { get; set; }
        public string? orderNo { get; set; } = string.Empty;

        public SaleItem[] saleItems { get; set; } 
        public List<SalePayment> salePayments { get; set; }

    }

    public class SaleItem
    {
        public int id { get; set; }
        public Sale oSale { get; set; }
        public int? quantity { get; set; } = 0;
        public string? description { get; set; } = string.Empty;
        public AdhocTypeLookup? oAdhocType { get; set; }
        public decimal? unitPrice { get; set; } = 0m;
        public decimal? totalPrice { get; set; } = 0m;
        public int? addedBy { get; set; } = 1057;
        public DateTime? addedDate { get; set; } = DateTime.Now;

    }

    public class SalePayment
    {
        public int id { get; set; }
        public int? adhocRecordId { get; set; }
        public DateTime? paymentDate { get; set; } = DateTime.Now;
        public decimal paymentAmt { get; set; } = 0m;
        public PaymentMethod? payMethod { get; set; }
        public decimal outstandingAmt { get; set; } = 0m;
    }

}
