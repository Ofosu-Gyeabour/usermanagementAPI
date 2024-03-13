using System;
using System.Collections.Generic;

namespace UserManagementAPI.Models
{
    public partial class TXeroConfig
    {
        public int ConfigId { get; set; }
        public string ClientId { get; set; } = null!;
        public string? ClientSecret { get; set; }
        public string? RefreshToken { get; set; }
        public string? AccessToken { get; set; }
        public string? IdToken { get; set; }
        public string? TokenType { get; set; }
        public string? XeroTenantId { get; set; }
        public string? ReDirectUri { get; set; }
        public string? Scopes { get; set; }
        public int? State { get; set; }
    }
}
