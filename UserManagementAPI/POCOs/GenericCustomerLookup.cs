namespace UserManagementAPI.POCOs
{
    public class ClientTypeLookup
    {
        public int id { get; set; } = 0;
        public string clientTypeDescrib { get; set; } = string.Empty;
    }

    public class ChannelTypeLookup
    {
        public int id { get; set; } = 0;
        public string nameOfchannel { get; set; } = string.Empty;
        public string channelTypeDescrib { get; set; } = string.Empty;
    }

    public class AddressLookup
    {
        public int id { get; set; } = 0;
        public int clientId { get; set; } = 0;
        public string? address1 { get; set; } = string.Empty;
        public string? address2 { get; set; } = string.Empty;
        public string? address3 { get; set; } = string.Empty;
        public string? address4 { get; set; } = string.Empty;
    }

    public class CorporateCustomerLookup
    {
        public int id { get; set; } = 0;
        public ClientTypeLookup? oClientType { get; set; }
        public string accountNo { get; set; } = string.Empty;
        public CityLookup? oCity { get; set; }
        public CountryLookup? oCountry { get; set; }
        public CompanyLookup? oCompany { get; set; }
        public ChannelTypeLookup? oChannelType { get; set; }
        public ReferralLookup? oReferral { get; set; }
        public AddressLookup? oAddress { get; set; }
        public string? clientBusiness { get; set; } = string.Empty;
        public string postCode { get; set; } = string.Empty;
        public string mobileNo { get; set; } = string.Empty;
        public string whatsappNo { get; set; } = string.Empty;
        public string homeTelephone { get; set; } = string.Empty;
        public string workTelephone { get; set; } = string.Empty;
        public string clientEmail { get; set; } = string.Empty;
        public string clientEmail2 { get; set; } = string.Empty;
        public string collectionInstruction { get; set; } = string.Empty;
    }


    public class IndividualCustomerLookup : CorporateCustomerLookup
    {
        public string surname { get; set; } = string.Empty;
        public string firstname { get; set; } = string.Empty;
        public string middlenames { get; set; } = string.Empty;
    }

    public class GenericCustomerLookup
    {
        public int id { get; set; } = 0;
        public string? accountNo { get; set; } = string.Empty;
        public ClientTypeLookup? oClientType { get; set; }
        public string nameOrcompany { get; set; } = string.Empty;
        public string postCode { get; set; } = string.Empty;
        public string address { get; set; } = string.Empty;
        public string mobileNo { get; set; } = string.Empty;
    }

}
