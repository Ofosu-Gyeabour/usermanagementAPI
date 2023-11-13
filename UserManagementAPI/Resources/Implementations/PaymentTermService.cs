#nullable disable
using UserManagementAPI.POCOs;
using UserManagementAPI.Response;
using UserManagementAPI.Resources.Interfaces;

namespace UserManagementAPI.Resources.Implementations
{
    public class PaymentTermService : IPaymentTermService
    {
        private swContext config;
        public PaymentTermService()
        {
            config = new swContext();
        }

        public async Task<DefaultAPIResponse> Get()
        {
            //gets all payment terms from the data source
            DefaultAPIResponse response;
            List<PaymentTermLookup> paymentTermList = null;

            try
            {
                var Query = (from pt in config.TPaymentTerms
                             select new
                             {
                                 id = pt.Id,
                                 description = pt.PaymentTermDescrib
                             });

                var pList = await Query.ToListAsync().ConfigureAwait(false);
                paymentTermList = pList.Select(q => new PaymentTermLookup
                {
                    id = q.id,
                    paymentTermDescrib = q.description
                }).ToList();

                response = new DefaultAPIResponse() { 
                    status = true, 
                    message = @"success",
                    data = paymentTermList
                };

                return response;
            }
            catch(Exception x)
            {
                return response = new DefaultAPIResponse() { 
                    status = false,
                    message = $"error: {x.Message}"
                };
            }
        }

    }
}
