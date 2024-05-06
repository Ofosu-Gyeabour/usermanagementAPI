#nullable disable
using UserManagementAPI.Response;
using UserManagementAPI.Resources.Interfaces;
using UserManagementAPI.POCOs;
using System.Diagnostics;
using UserManagementAPI.utils;

namespace UserManagementAPI.Resources.Implementations
{
    public class ConsolidatorService : IConsolidatorService
    {
        private swContext config;
        public ConsolidatorService()
        {
            config = new swContext();
        }

        public async Task<DefaultAPIResponse> AuthenticatorConsolidatorAsync(UserInfo payLoad)
        {
            //TODO: validate the user credential of a consolidator
            DefaultAPIResponse response = null;

            try
            {
                clsConsolidator conso = new clsConsolidator();
                var conso_credentials = await conso.validateConsolidatorCredentialsAsync(payLoad);

                response = new DefaultAPIResponse() { 
                    status = true,
                    message = $"consolidator {payLoad.username} validated",
                    data = conso_credentials
                };

                return response;
            }
            catch(Exception x)
            {
                return response = new DefaultAPIResponse()
                {
                    status = false,
                    message = $"error: {x.Message}"
                };
            }
        }

        public async Task<DefaultAPIResponse> CreateConsolidatorUserAccountAsync(consolUserRecord payLoad)
        {
            //TODO: create a consolidator user account record
            DefaultAPIResponse response = null;

            try
            {
                var cls = new clsConsolidator();
                var operation_status = await cls.createUserAccountAsync(payLoad);

                return response = new DefaultAPIResponse() { 
                    status = operation_status,
                    message = operation_status ? $"{payLoad.userCredentials.username} created successfully" : $"{payLoad.userCredentials.username} could not be created. Please see Administrator",
                    data = payLoad
                };
            }
            catch(Exception x)
            {
                return response = new DefaultAPIResponse()
                {
                    status = false,
                    message = $"error: {x.Message}"
                };
            }
        }

        public async Task<DefaultAPIResponse> ResetUserAccountAsync(consolUserRecord payLoad)
        {
            //TODO: reset user account
            DefaultAPIResponse response = null;

            try
            {
                var obj = new clsConsolidator();
                var opStatus = await obj.resetUserAccountAsync(payLoad);

                return response = new DefaultAPIResponse()
                {
                    status = opStatus,
                    message = opStatus ? $"{payLoad.userCredentials.username} reset successfully" : $"{payLoad.userCredentials.username} could not be reset. Please see Administrator",
                    data = payLoad
                };
            }
            catch(Exception x)
            {
                return response = new DefaultAPIResponse()
                {
                    status = false,
                    message = $"error: {x.Message}"
                };
            }
        }

        public async Task<DefaultAPIResponse> ListConsolidatorUsersAsync(consolUserRecord payLoad)
        {
            //TODO: gets a list of users belonging to a consolidator
            DefaultAPIResponse response = null;

            try
            {
                clsConsolidator obj = new clsConsolidator();
                var consolUserList = await obj.getConsolidatorUserListAsync(payLoad.consolID);

                return response = new DefaultAPIResponse()
                {
                    status = consolUserList.Count() > 0 ? true: false,
                    message = consolUserList.Count() > 0? $"{consolUserList.Count()} records fetched successfully" : @"No data",
                    data = consolUserList
                };
            }
            catch(Exception x)
            {
                return response = new DefaultAPIResponse() { 
                    status = false,
                    message = $"error: {x.Message}"
                };
            }
        }

        public async Task<DefaultAPIResponse> GetCustomersOfClientAsync(consolUserRecord payLoad, string flag = @"*")
        {
            //TODO: gets the customers belonging to main clients / agents /consolidators in the data store
            DefaultAPIResponse response = null;

            try
            {
                clsConsolidator obj = new clsConsolidator();
                var records = await obj.getConsolidatorCustomersAsync(payLoad.consolID);

                return response = new DefaultAPIResponse()
                {
                    status = records.Count() > 0? true: false,
                    message = records.Count() > 0? $"{records.Count()} records fetched": @"No data",
                    data = records
                };
            }
            catch(Exception x)
            {
                return response = new DefaultAPIResponse()
                {
                    status = false,
                    message = $"error: {x.Message}"
                };
            }
        }

        public async Task<DefaultAPIResponse> CreateIndividualCustomerAsync(IndividualConsolidatorClient payLoad)
        {
            DefaultAPIResponse response = null;

            try
            {
                var obj = new clsConsolidator();
                var feedBack = await obj.createIndividualConsolidatorClientAsync(payLoad);

                return response = new DefaultAPIResponse() { 
                    status = feedBack,
                    message = feedBack == true ? $"{payLoad.surname} {payLoad.firstname} created successfully as a consolidator's client": @"An error occured. Please contact Administrator",
                    data = payLoad
                };
            }
            catch(Exception x)
            {
                return response = new DefaultAPIResponse()
                {
                    status = false,
                    message = $"error: {x.Message}"
                };
            }
        }

        public async Task<DefaultAPIResponse> CreateCorporateCustomerAsync(CorporateConsolidatorClient payLoad)
        {
            DefaultAPIResponse response = null;

            try
            {
                clsConsolidator obj = new clsConsolidator();
                var createFeedbackResponse = await obj.createCorporateConsolidatorClientAsync(payLoad);

                return response = new DefaultAPIResponse()
                {
                    status = createFeedbackResponse,
                    message = createFeedbackResponse == true? $"{payLoad.businessName} created successfully as a consolidator's client": @"An error occured. Please contact Administrator",
                    data = payLoad
                };
            }
            catch(Exception x)
            {
                return response = new DefaultAPIResponse()
                {
                    status = false,
                    message = $"error: {x.Message}"
                };
            }
        }

        public async Task<DefaultAPIResponse> CreateConsolidatorOrderAsync(clsConsolidatorOrder payLoad)
        {
            //TODO: creates consolidator order 
            DefaultAPIResponse response = null;

            try
            {
                clsConsolidator obj = new clsConsolidator();

                //delete any existing pending order first
                var deletePendingfeedBack = await obj.DeleteConsolidatedOrderAsync(payLoad);
                var feedBack = await obj.CreateOrderAsync(payLoad);

                return response = new DefaultAPIResponse()
                {
                    status = feedBack,
                    message = feedBack == true ? $"Items successfully added" : @"An error occured. Please contact Administrator",
                    data = payLoad
                };
            }
            catch(Exception x)
            {
                return response = new DefaultAPIResponse() { 
                    status = false,
                    message = $"error: {x.Message}"
                };
            }
        }

        public async Task<DefaultAPIResponse> PostConsolidatedOrderAsync(clsConsolidatorOrder payLoad)
        {
            //TODO: post a finished consolidated order
            DefaultAPIResponse rsp = null;

            try
            {
                clsConsolidator obj = new clsConsolidator();
                var postOrderStatus = await obj.PostConsolidatedOrderAsync(payLoad);

                rsp = new DefaultAPIResponse() { 
                    status = postOrderStatus ? true: false,
                    message = postOrderStatus? $"Consolidated items posted to WIF with intermediary order number: <b>{payLoad.orderNo}</b>": @"Operation failed. Please contact your Administrator",
                    data = payLoad
                };

                return rsp;
            }
            catch(Exception x)
            {
                return rsp = new DefaultAPIResponse()
                {
                    status = false,
                    message = $"error: {x.Message}"
                };
            }
        }
        public async Task<DefaultAPIResponse> GetPendingConsolidatorOrderAsync(int ConsolidatorID)
        {
            //TODO: return a pending consolidator order and its order items
            DefaultAPIResponse rsp = null;

            try
            {
                clsConsolidator obj = new clsConsolidator();
                var pendingConsolOrder = await obj.getPendingConsolidatorOrder(ConsolidatorID);

                rsp = new DefaultAPIResponse() { 
                    status = pendingConsolOrder != null ? true: false,
                    message = pendingConsolOrder != null ? $"{pendingConsolOrder.items.Length} items in the pending order": @"failed",
                    data = pendingConsolOrder
                };

                return rsp;
            }
            catch(Exception x)
            {
                return rsp = new DefaultAPIResponse() { 
                    status = false,
                    message = $"error: {x.Message}"
                };
            }
        }
        public async Task<DefaultAPIResponse> DeletePendingConsolidatedItemAsync(int ID, string itemName)
        {
            DefaultAPIResponse rsp = null;

            try
            {
                clsConsolidator obj = new clsConsolidator();
                var bln = await obj.deletePendingConsolidatedItemAsync(ID);

                rsp = new DefaultAPIResponse()
                {
                    status = bln ? true : false,
                    message = bln ? $"Item {itemName} deleted from consolidated items list": @"failed. Contact Administrator",
                    data = new GenericLookup() { id = ID, idValue = itemName }
                };

                return rsp;
            }
            catch(Exception x)
            {
                return rsp = new DefaultAPIResponse()
                {
                    status = false,
                    message = $"error: {x.Message}"
                };
            }
        }
    
        public async Task<DefaultAPIResponse> GetPostedConsolidatedOrdersAsync()
        {
            //todo: gets posted consolidated orders
            DefaultAPIResponse rsp = null;

            try
            {
                dtStore dstore = new dtStore();
                var consolOrders = await dstore.getPostedConsolidatedItemsAsync();

                rsp = new DefaultAPIResponse()
                {
                    status = consolOrders != null ? true: false,
                    message = consolOrders != null ? $"{consolOrders.Count()} items": "An error occured.Please see Administrator",
                    data = consolOrders
                };

                return rsp;
            }
            catch(Exception x)
            {
                return rsp = new DefaultAPIResponse()
                {
                    status = false,
                    message = $"error: {x.Message}"
                };
            }
        }

    }
}
