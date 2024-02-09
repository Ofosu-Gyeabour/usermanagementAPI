#nullable disable

namespace UserManagementAPI.POCOs
{
    public class clsShippingOrderPayment
    {
        #region properties

        public DateTime? payDate { get; set; }
        public decimal? payAmt { get; set; } = 0m;
        public string? payMethod { get; set; } = string.Empty;
        public string? payReceiptNo { get; set; } = string.Empty;
        public decimal? outstandingAmt { get; set; } = 0m;

        #endregion

        #region methods

        public async Task<int?> getID()
        {
            //TODO: gets the id for the payment method
            int? pId = 0;

            try
            {
                var config = new swContext();
                using (config)
                {
                    var obj = await config.TPaymentMethods.Where(tp => tp.Method == payMethod).FirstOrDefaultAsync();
                    pId = obj != null ? obj.Id : 0;
                }

                return pId;
            }
            catch(Exception ex)
            {
                return pId;
            }
        }

        #endregion


    }
}
