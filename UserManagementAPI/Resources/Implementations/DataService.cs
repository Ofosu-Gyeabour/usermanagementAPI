#nullable disable

using UserManagementAPI.Resources;
using UserManagementAPI.Resources.Interfaces;
using UserManagementAPI.POCOs;
using UserManagementAPI.Response;
using UserManagementAPI.Procs;
using UserManagementAPI.utils;

namespace UserManagementAPI.Resources.Implementations
{
    public class DataService : IDataService
    {
        private swContext config;

        public DataService()
        {
            config = new swContext();
        }
        

        public async Task<DefaultAPIResponse> getCustomerShippingOrder(int customerID, int page, int pageSize, DateTime df, DateTime dt)
        {
            //TODO: gets shipping order data from the data store for presentation
            DefaultAPIResponse response = null;

            try
            {
                View360 v360 = await new View360() { customerId = customerID, dateFrom = df, dateTo = dt }.get360View();
                //dtStore dtstore = new dtStore();
                //var storedProcData = await dtstore.GetShippingOrders(customerID);

                //var totalCount = storedProcData.Count();
                //var totalPages = (int)Math.Ceiling(totalCount / (decimal)pageSize);


                response = new DefaultAPIResponse() { 
                    status = v360.procShippings.Count() > 0 ? true: false,
                    message = v360.procShippings.Count() > 0 ? $"{v360.procShippings.Count()} shipping records, {v360.procPackagings.Count()} packaging records" : @"No data fetched",
                    data = v360
                    //data = storedProcData
                    //        .Skip((page - 1) * pageSize)
                    //        .Take(pageSize)
                    //        .ToList()
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
