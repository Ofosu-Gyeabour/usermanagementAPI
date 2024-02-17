namespace UserManagementAPI.Xero
{
    public class clsXeroInvoice
    {
        public string Type { get; set; } = @"ACCREC";
        public clsXeroContact? Contact { get; set; }

        public DateTime? Date { get; set; }
        public DateTime? DueDate { get; set; }
        public string? DateString { get; set; } = string.Empty;
        public string? DueDateString { get; set; } = string.Empty;
        public string? ExpectedPaymentDate { get; set; } = string.Empty;
        public string? InvoiceNumber { get; set; } = string.Empty;
        public string? Reference { get; set; } = string.Empty;
        public string? BrandingThemeID { get; set; } = string.Empty;
        public string? Url { get; set; } = string.Empty;
        public string? CurrencyCode { get; set; } = string.Empty;
        public decimal? SubTotal { get; set; } = 0m;
        public decimal? TotalTax { get; set; } = 0m;
        public decimal? Total { get; set; } = 0m;
        public string? LineAmountTypes { get; set; } = @"Exclusive";
        public List<clsLineItems> LineItems { get; set; }

        public string? Status { get; set; } = string.Empty;  //set to AUTHORISED to approve sales invoice
    }

    public class clsLineItems
    {
        public string? Description { get; set; } = string.Empty;
        public int? Quantity { get; set; } = 0;
        public decimal? UnitAmount { get; set; } = 0m;
        public string? TaxType { get; set; } = string.Empty;
        public decimal? TaxAmount { get; set; } = 0m;
        public decimal? LineAmount { get; set; } = 0m;

        public string? AccountCode { get; set; } = string.Empty;
        public clsInvoiceTracking? Tracking { get; set; } 
        public decimal? DiscountRate { get; set; } = 0m;
        public string? ItemCode { get; set; } = string.Empty; //used when price and account code of the item can be picked from xero itself and not the application posting the invoice


    }
    
    public class clsInvoiceTracking
    {
        public string? Name { get; set; } = string.Empty;
        public string? Option { get; set; } = string.Empty;
    }

    public class XeroAPIResponse
    {
        public string? Id { get; set; } = string.Empty;
        public string? Status { get; set; } = string.Empty;
        public string? Message { get; set; } = string.Empty;
        public string? ProviderName { get; set; } = string.Empty;
        public string? DateTimeUTC { get; set; } = string.Empty;

        public List<clsXeroInvoice> Invoices { get; set; }

    }

    public class XeroTokenAPIResponse
    {
        public string? id_token { get; set; } = string.Empty;
        public string? access_token { get; set; } = string.Empty;
        public int? expires_in { get; set; } = 0;
        public string? token_type { get; set; } = string.Empty;
        public string? refresh_token { get; set; } = string.Empty;
        public string? scope { get; set; } = string.Empty;
    }

    public class clsRefresh
    {
        public string grant_type { get; set; } = string.Empty;
        public string? refresh_token { get; set; } = string.Empty;
        public string? client_id { get; set; } = string.Empty;
        public string? client_secret { get; set; } = string.Empty;

    }

}
