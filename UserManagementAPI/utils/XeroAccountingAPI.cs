#nullable disable
namespace UserManagementAPI.utils
{
    public class XeroAccountingAPI
    { 
        public string Accept { get; set; } = string.Empty;
        public string ContentType { get; set; } = string.Empty;
        public string contacts { get; set; } = string.Empty;
        public string invoices { get; set; } = string.Empty;
        public string refresh { get; set; } = string.Empty;
    }

    public class XeroConfigObject
    {
        public static string ACCEPT { get; set; }
        public static string CONTENT_TYPE { get; set; }
        public static string CONTACT { get; set; }
        public static string INVOICE { get; set; }
        public static string REFRESH_T { get; set; }
    }
}
