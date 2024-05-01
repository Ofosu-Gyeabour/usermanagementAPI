#nullable disable
using Swashbuckle.AspNetCore.SwaggerUI;
using UserManagementAPI.POCOs;

namespace UserManagementAPI.website
{
    public class clsCustomer 
    {
        private swContext config;
        public clsCustomer()
        {
            
        }

        #region properties
        public int id { get; set; } = 0;
        public string surname { get; set; } = string.Empty;
        public string firstname { get; set; } = string.Empty;
        public string middlenames { get; set; } = string.Empty;


        #endregion
        #region corporate-properties

        public int clienttypeId { get; set; } = 1;  //individual: 2 = corporate
        public string accountNo { get; set; } = string.Empty;
        public int? cityId { get; set; } = 10000; //default to SYSTEM
        public int? countryId { get; set; } = 1000;  //default to SYSTEM
        public int? channeltypeId { get; set; } = 1; //default to email notifications
        //public CityLookup? oCity { get; set; }
        //public CountryLookup? oCountry { get; set; }
        //public CompanyLookup? oCompany { get; set; }
        public int? associatedCompany { get; set; } = 1; //default to WIF
        //public ChannelTypeLookup? oChannelType { get; set; }
        //public ReferralLookup? oReferral { get; set; }
        public int? referralId { get; set; } = 2; //defaulted to GOOGLE
        public string clientBusiness { get; set; } = string.Empty;
        public string? postCode { get; set; } = string.Empty;
        public string mobileNo { get; set; } = string.Empty;
        public string whatsappNo { get; set; } = string.Empty;
        public string? homeTelephone { get; set; } = string.Empty;
        public string workTelephone { get; set; } = string.Empty;
        public string? clientEmail { get; set; } = string.Empty;
        public string? clientEmail2 { get; set; } = string.Empty;
        public string verificationLink { get; set; } = string.Empty;
        public string pwd { get; set; } = string.Empty;
        public AddressLookup? oAddress { get; set; }

        //public string collectionInstruction { get; set; } = string.Empty;
        //public int createdBy { get; set; }
        //public int modifiedBy { get; set; }
        //public bool shipper { get; set; }

        //public string clientPassword { get; set; } = string.Empty;
        //public bool canLogin { get; set; } = true;

        //public int consolidator { get; set; } = 1;
        #endregion

        #region private method

        public async Task<string> generateClientPassword()
        {
            //generates 10 letters as default password for client
            //5 letters
            //5 numbers

            //string characters = @"abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ!@£$%^&*()#";
            string characters = UserManagementAPI.utils.ConfigObject.CHARS;

            string result = string.Empty;

            string numberString = string.Empty;
            var rand = new Random();
            for(int i = 0; i < 5; i++)
            {
                int no = rand.Next(0, 9);
                numberString += no.ToString();
            }

            string randomString = string.Empty;

            for (int k =0; k < 5; k++)
            {
                int alpha = rand.Next(63);
                randomString += characters.ElementAt(alpha).ToString();
            }

            result = $"{numberString}{randomString}";
            return result;
        }

        #endregion

    }
}
