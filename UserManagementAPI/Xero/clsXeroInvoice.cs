namespace UserManagementAPI.Xero
{
    #region Invoicing classes
    public class clsXeroInvoice
    {
        public string Type { get; set; } = @"ACCREC";  //required
        public clsXeroContact? Contact { get; set; }  //required
        public List<clsLineItems> LineItems { get; set; }  //required

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

    #endregion

    #region xero return types
    public class XeroAPIResponse
    {
        public string? Id { get; set; } = string.Empty;
        public string? Status { get; set; } = string.Empty;
        public string? Message { get; set; } = string.Empty;
        public string? ProviderName { get; set; } = string.Empty;
        public string? DateTimeUTC { get; set; } = string.Empty;

        public List<clsXeroInvoice> Invoices { get; set; }
        public object data { get; set; }

    }

    public class XeroConnections
    {
        public string id { get; set; } = string.Empty;
        public string authEventId { get; set; } = string.Empty;
        public string tenantId { get; set; } = string.Empty;
        public string tenantType { get; set; } = string.Empty;
        public string tenantName { get; set; } = string.Empty;
        public DateTime createdDateUtc { get; set; }
        public DateTime updatedDateUtc { get; set; }
    }
    
    public class XeroAPIConnectionResponse
    {
        public List<XeroConnections> Connections { get; set; }
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

    #endregion

    #region refresh-token
    public class clsRefresh
    {
        public string id_token { get; set; } = string.Empty;
        public string access_token { get; set; } = string.Empty;
        public string expires_in { get; set; } = string.Empty;
        public string token_type { get; set; } = string.Empty;
        public string scope { get; set; } = string.Empty;
        public string? refresh_token { get; set; } = string.Empty;

    }

    #endregion



}
