#nullable disable
namespace UserManagementAPI.Procs
{
    public class pSalesOrderData
    {
        public psaleRecord saleRecord { get; set; }
        public List<pSaleItem> saleItems { get; set; }
        public List<pSalePayment> salePayments { get; set; }
    }

    public class psaleRecord
    {
        public int? id { get; set; } = 0;
        public string? adhocname { get; set; } = string.Empty;
        public string? nomcode { get; set; } = string.Empty;
        public int? companyid { get; set; } = 1;
        public string? company { get; set; } = string.Empty;
        public DateTime? adhocdate { get; set; } = DateTime.Now;
        public decimal? vat { get; set; } = 0m;
        public int? isinvoiced { get; set; } = 0;
        public DateTime? invoiceDate { get; set; } = DateTime.Now;
        public string? surname { get; set; } = string.Empty;
        public string? firstname { get; set; } = string.Empty;
        public string? middlenames { get; set; } = string.Empty;
        public string? clientbusinessname { get; set; } = string.Empty;
        public int? paymenttermsId { get; set; } = 0;
        public string? paymentTermDescrib { get; set; } = string.Empty;
    }

    public class pSaleItem
    {
        //adhoc item is the same as sales item
        public int? Id { get; set; } = 0;
        public int? qty { get; set; } = 0;
        public string? describ { get; set; } = string.Empty;
        public string? nCode { get; set; } = string.Empty;
        public string? nCodeDescrib { get; set; } = string.Empty;
        public decimal? unitPrice { get; set; } = 0m;
        public decimal? totalPrice { get; set; } = 0m;
        public int? addedBy { get; set; } = 10000;
        public string? usrname { get; set; } = string.Empty;
        public DateTime? addedDate { get; set; } = DateTime.Now;
    }

    public class pSalePayment {
        public pSalePayment()
        {
            
        }
        public int? Id { get; set; } = 0;
        public DateTime? payDate { get; set; } = DateTime.Now;
        public decimal? payAmt { get; set; } = 0m;
        public string? method { get; set; } = string.Empty;
        public int? payMethodId { get; set; } = 0;
        public decimal? outstandingAmt { get; set; } = 0m;
    }


}
