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

        public int createdBy { get; set; }
        public int modifiedBy { get; set; }
        public bool shipper { get; set; }

        public string clientPassword { get; set; } = string.Empty;
        public bool canLogin { get; set; } = true;
    }


    public class IndividualCustomerLookup : CorporateCustomerLookup
    {
        public string surname { get; set; } = string.Empty;
        public string firstname { get; set; } = string.Empty;
        public string middlenames { get; set; } = string.Empty;

        public async Task<List<OrderSummaryDetails>> getOrderSummaryAsync(OrderSummaryParameter param)
        {
            //creates order summary
            List<OrderSummaryDetails> result = null;
            OrderSummaryDetails paidTotal;

            try
            {
                var total = new OrderSummaryDetails() { key = @"ITEMS TOTAL", value = param.total };
                var vRate = new OrderSummaryDetails() { key = @"VAT RATE", value = param.vatRate };
                var vAmt = new OrderSummaryDetails() { key = @"VAT AMOUNT", value = (param.total * param.vatRate) };
                var grandTotal = new OrderSummaryDetails() { key = @"TOTAL", value = (total.value + vAmt.value) };

                if (param.paid > 0)
                {
                    paidTotal = new OrderSummaryDetails() { key = @"PAYMENTS TOTAL", value = (param.paid) };
                }
                else
                {
                    paidTotal = new OrderSummaryDetails() { key = @"PAYMENTS TOTAL", value = (grandTotal.value - grandTotal.value) };
                }
                
                var amountDue = new OrderSummaryDetails() { key = @"AMOUNT DUE", value = (grandTotal.value - paidTotal.value) };

                result = new List<OrderSummaryDetails>()
                {
                    total,
                    vRate,
                    vAmt,
                    grandTotal,
                    paidTotal,
                    amountDue
                };

                return result;
            }
            catch(Exception x)
            {
                return result;
            }
            
        }
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
