namespace UserManagementAPI.Xero
{
    public class clsXeroContact
    {
        public string Name { get; set; } = string.Empty; //required field
        public string? ContactID { get; set; } = "1e4b296e-7089-42f4-b59d-ca5015f8ef5f"; //defaulted to string.Empty
        public string? ContactNumber { get; set; } = string.Empty;
        public string? AccountNumber { get; set; } = string.Empty;
        public string? BusinessName { get; set; } = string.Empty;
        public string? FirstName { get; set; } = string.Empty;
        public string? Middlename { get; set; } = string.Empty;
        public string? LastName { get; set; } = string.Empty;
        public string? EmailAddress { get; set; } = string.Empty;
        public List<contactPerson>? ContactPersons { get; set; }
        public string? AddressLine1 { get; set; } = string.Empty;
        public string? AddressLine2 { get; set; } = string.Empty;
        public string? AddressLine3 { get; set; } = string.Empty;
        public string? AddressLine4 { get; set; } = string.Empty;
        public string? City { get; set; } = string.Empty;
        public string? Country { get; set; } = string.Empty;
        public string? PostalCode { get; set; } = string.Empty;
        public string? ContactStatus { get; set; } = string.Empty;

        public string? BankAccountDetails { get; set; } = string.Empty;
        public string? TaxNumber { get; set; } = string.Empty;
        public string? AccountsReceivableTaxType { get; set; } = string.Empty;
        public string? AccountsPayableTaxType { get; set; } = string.Empty;
        public string? DefaultCurrency { get; set; } = @"GBP";

        public string? mobileNo { get; set; } = string.Empty;
        public string? whatsappNo { get; set; } = string.Empty;
        public string? workTelephone { get; set; } = string.Empty;
    }

    public class contactPerson
    {
        public string? FirstName { get; set; } = string.Empty;
        public string? LastName { get; set; } = string.Empty;
        public string? EmailAddress { get; set; } = string.Empty;
        public string? IncludeInEmails { get; set; } = @"true";
        public List<Telephone>? Telephones { get; set; } = null;
    }

    public class Telephone
    {
        public string? PhoneNumber { get; set; } = string.Empty;
        public string? PhoneAreaCode { get; set; } = string.Empty;
        public string? PhoneCountryCode { get; set; } = string.Empty;
    }
}
