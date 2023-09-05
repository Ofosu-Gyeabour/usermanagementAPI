namespace UserManagementAPI.POCOs
{
    public class Log
    {
        public int id { get; set; }
        public int? eventId { get; set; }
        public string? actor { get; set; } = string.Empty;
        public string? entity { get; set; } = string.Empty;
        public string? entityValue { get; set; } = string.Empty;
        public int? companyId { get; set; } = 0;
        public DateTime? logDate { get; set; }
    }
}
