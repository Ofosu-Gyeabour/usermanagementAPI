#nullable disable

using System.Security;

namespace UserManagementAPI.POCOs
{
    public class clsCongestion
    {
        private swContext config;
        public clsCongestion()
        {
            config = new swContext();
        }

        public int id { get; set; }
        public string? postCode { get; set; }
        public decimal? congestionCharge { get; set; }

        public async Task<clsCongestion> getCongestionChargeAsync()
        {
            //TODO: determine how much is charged as congestion fee for the post code in question
            clsCongestion obj = null;

            try
            {
                var d = await config.TCongestionCharges.Where(c => c.PostCode == this.postCode).FirstOrDefaultAsync();

                return d == null ? obj : obj = new clsCongestion() { 
                    id = d.Id,
                    postCode = d.PostCode.Trim(),
                    congestionCharge = d.Charge
                };
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

    }
}
