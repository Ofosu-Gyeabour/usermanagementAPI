#nullable disable
using UserManagementAPI.Resources;
using UserManagementAPI.Response;
using UserManagementAPI.POCOs;
using UserManagementAPI.Resources.Interfaces;
using UserManagementAPI.utils;
using System.Reflection.Metadata.Ecma335;

namespace UserManagementAPI.Resources.Implementations
{
    public class UtilityService : IUtilityService
    {
        private swContext config;

        public UtilityService()
        {
            config = new swContext();
        }

        public async Task<DefaultAPIResponse> createChargeAsync(ChargeEngineLookup payLoad)
        {
            //makes a charge engine entry
            DefaultAPIResponse rsp = null;
            Helper helper = new Helper();

            try
            {
                bool bln = await helper.addChargeEngineEntryAsync(payLoad);
                rsp = new DefaultAPIResponse()
                {
                    status = bln,
                    message = $"{payLoad.oChargeLookup.nameOfcharge} added successfully to the Charge Engine",
                    data = payLoad
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
    
        public async Task<DefaultAPIResponse> getChargeEngineAsync(OrderTypeLookup payLoad)
        {
            //gets all the charge metrics for a specific order
            DefaultAPIResponse response = null;
            Helper helper = new Helper();

            try
            {
                var chargeList = await helper.getChargesAsync(payLoad);

                return response = new DefaultAPIResponse() { 
                    status = true,
                    message = $"data fetched successfully",
                    data = chargeList
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

        public async Task<DefaultAPIResponse> getAllChargesAsync()
        {
            //gets all charges in the data store
            Helper helper = new Helper();
            DefaultAPIResponse response = null;

            try
            {
                var allchargeListData = await helper.getAllChargesAsync();
                return response = new DefaultAPIResponse() { 
                    status = true,
                    message = $"data fetched successfully",
                    data = allchargeListData
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
        public async Task<DefaultAPIResponse> amendChargeListAsync(ChargeEngineLookup payLoad)
        {
            //amends a charge list record
            DefaultAPIResponse rsp = null;
            

            try
            {
                using (config)
                {
                    var objChargeEngine = await config.TChargeEngines.Where(x => x.Id == payLoad.id).FirstOrDefaultAsync();
                    if (objChargeEngine != null)
                    {
                        
                        objChargeEngine.OrdertypeId = payLoad.oOrderType.id;
                        objChargeEngine.ChargeId = payLoad.oChargeLookup.id;
                        objChargeEngine.ChargeRate = payLoad.chargeRate;
                        objChargeEngine.ThresholdAmt = payLoad.thresholdAmt;

                        await config.SaveChangesAsync();

                        rsp = new DefaultAPIResponse()
                        {
                            status = true,
                            message = $"Charge amended to {payLoad.oChargeLookup.nameOfcharge} successfully",
                            data = payLoad
                        };
                    }
                }
                
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
        public async Task<DefaultAPIResponse> getOrderTypeAsync()
        {
            //TODO: get order types
            DefaultAPIResponse rsp = null;
            Helper helper = new Helper();
            List<OrderTypeLookup> ordertypeList = null;

            try
            {
                var returnedData = await helper.GetOrderTypesAsync();

                if (returnedData != null)
                {
                    ordertypeList = new List<OrderTypeLookup>();

                    foreach(var r in returnedData)
                    {
                        var obj = new OrderTypeLookup() {
                            id = r.Id,
                            orderDescription = r.Describ
                        };

                        ordertypeList.Add(obj);
                    }

                    rsp = new DefaultAPIResponse()
                    {
                        status = true,
                        message = @"data fetched",
                        data = ordertypeList
                    };
                }

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
        public async Task<DefaultAPIResponse> getChargeEngineLinesAsync(OrderTypeLookup payLoad)
        {
            //gets the charge engine lines
            DefaultAPIResponse response = null;
            List<ChargeEngineLookup> chargeLines = null;

            try
            {
                var Query = (from ch in config.TChargeEngines 
                             join clk in config.TChargeLookups on ch.ChargeId equals clk.Id
                             join ot in config.TOrderTypes on ch.OrdertypeId equals ot.Id
                             where ot.Id == payLoad.id

                             select new
                             {
                                 id = ch.Id,
                                 chargeId = ch.ChargeId,
                                 charge = clk.Charge,
                                 rate = ch.ChargeRate,
                                 threshold = ch.ThresholdAmt,
                                 ordertypeId = ch.OrdertypeId,
                                 ordertypeDescrib = ot.Describ
                                 //add threshold amount later after context migration
                             });

                var queryList = await Query.ToListAsync().ConfigureAwait(false);
                chargeLines = queryList.Select(x => new ChargeEngineLookup()
                {
                    id = x.id,
                    oOrderType = new OrderTypeLookup()
                    {
                        id = (int) x.ordertypeId,
                        orderDescription = x.ordertypeDescrib
                    },
                    oChargeLookup = new ChargeLookup() { 
                        id = (int) x.chargeId,
                        nameOfcharge = x.charge
                    },
                    chargeRate = x.rate,
                    thresholdAmt = x.threshold
                    
                }).ToList();

                return response = new DefaultAPIResponse() { 
                    status = true,
                    message = $"{chargeLines.Count()} fetched from data store",
                    data = chargeLines
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
        public async Task<DefaultAPIResponse> getSalesTypeAsync()
        {
            DefaultAPIResponse rsp = null;
            List<GenericLookup> saleTypeList = null;

            try
            {
                var sList = await config.TSaleTypeLookups.ToListAsync();
                if (sList != null)
                {
                    saleTypeList = new List<GenericLookup>();
                    foreach(var s in sList)
                    {
                        var obj = new GenericLookup() { id = s.Id, idValue = s.SaleTypeDescrib };
                        saleTypeList.Add(obj);
                    }

                    rsp = new DefaultAPIResponse()
                    {
                        status = true,
                        message = $"{saleTypeList.Count()} records fetched from datastore",
                        data = saleTypeList
                    };
                }

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
        public async Task<DefaultAPIResponse> getDeliveryTimeAsync()
        {
            DefaultAPIResponse rsp = null;
            List<GenericLookup> deliveryTimes = null;

            try
            {
                var dt = await config.TDeliveryTimeLookups.ToListAsync();
                if (dt != null)
                {
                    deliveryTimes = new List<GenericLookup>();
                    foreach(var d in dt)
                    {
                        var obj = new GenericLookup() { id = d.Id, idValue = d.DeliverytimeDescrib };
                        deliveryTimes.Add(obj);
                    }

                    rsp = new DefaultAPIResponse()
                    {
                        status = true,
                        message = $"{deliveryTimes.Count()} records fetched from data store",
                        data = deliveryTimes
                    };
                }

                return rsp;
            }
            catch(Exception x)
            {
                return rsp = new DefaultAPIResponse()
                {
                    status = false,
                    message = $"{x.Message}"
                };
            }
        }
        public async Task<DefaultAPIResponse> addChargeOrTaxAsync(ChargeLookup payLoad)
        {
            DefaultAPIResponse r = null;
            Helper helper = new Helper();

            try
            {
                if (! await helper.doesChargeExist(payLoad.nameOfcharge))
                {
                    TChargeLookup obj = new TChargeLookup() { Charge = payLoad.nameOfcharge };

                    await config.AddAsync(obj);
                    await config.SaveChangesAsync();

                    r = new DefaultAPIResponse() { 
                        status = true,
                        message = $"{payLoad.nameOfcharge} added to the Charge list in the data store",
                        data = payLoad
                    };
                }

                return r;
            }
            catch(Exception x)
            {
                return r = new DefaultAPIResponse()
                {
                    status = false,
                    message = $"error: {x.Message}"
                };
            }
        }
        public async Task<DefaultAPIResponse> getChargeOrTaxListAsync()
        {
            //TODO: get charge or tax list
            DefaultAPIResponse response = null;
            IEnumerable<ChargeLookup> results = null;
            Helper helper = new Helper();

            try
            {
                results = await helper.getChargeLookupAsync();

                return response = new DefaultAPIResponse()
                {
                    status = true,
                    message = $"{results.Count()} records fetched from datastore",
                    data = results
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
        public async Task<DefaultAPIResponse> getOrderSummaryKeysAsync(OrderTypeLookup payLoad)
        {
            //TODO: gets keys for order summary
            DefaultAPIResponse response = null;
            Helper helper = new Helper();

            try
            {
                var returnedKeys = await helper.getOrderSummaryKeys(payLoad);
                return response = new DefaultAPIResponse() { 
                    status = true,
                    message = $"{returnedKeys.Count()} keys returned successfully",
                    data = returnedKeys
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
        public async Task<DefaultAPIResponse> updateAccountKeysAsync(OrderStat payLoad)
        {
            //TODO: update account keys for the given order
            DefaultAPIResponse response = null;

            try
            {
                Helper obj = new Helper();
                var dta = await obj.updateOrderSummaryKeys(payLoad);

                return response = new DefaultAPIResponse() { 
                    status = true,
                    message = @"success",
                    data = dta
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
        public async Task<DefaultAPIResponse> getShippingItemsAsync()
        {
            //TODO: gets shipping items 
            DefaultAPIResponse response = null;
            
            try
            {
                var obj = new clsShippingItem();
                var dta = await obj.getShippingItemsAsync();

                return response = new DefaultAPIResponse()
                {
                    status = true,
                    message = $"{dta.Count()} records fetched",
                    data = dta.ToList()
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
        public async Task<DefaultAPIResponse> getShippingOrderItemsAsync()
        {
            //TODO: gets shipping order items
            DefaultAPIResponse response = null;

            try
            {
                var obj = new clsShippingOrderItem();
                var dta = await obj.getShippingOrderItemsAsync();

                return response = new DefaultAPIResponse() {
                    status = true,
                    message = $"{dta.Count()} records fetched",
                    data = dta.ToList()
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
        public async Task<DefaultAPIResponse> getConsigneesAsync(consigneeParam payLoad)
        {
            //TODO: gets consignees
            DefaultAPIResponse response = null;

            try
            {
                var obj = new clsConsigee();
                var consigee_data = await obj.fetchTopN(payLoad);

                return response = new DefaultAPIResponse() { 
                    status = true,
                    message = $"{consigee_data.Count()} records fetched",
                    data = consigee_data.ToList()
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
        public async Task<DefaultAPIResponse> getTotalParishesAsync()
        {
            //TODO: fetches all the parishes in the data store
            DefaultAPIResponse response = null;

            try
            {
                var obj = new clsParish();
                var parishList = await obj.getAllParishesAsync();

                return response = new DefaultAPIResponse()
                {
                    status = true,
                    message = $"{parishList.Count()} records fetched",
                    data = parishList.ToList()
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
        public async Task<DefaultAPIResponse> getZoneFromParishAsync(clsParish payLoad)
        {
            //TODO: gets a zone record using parish name
            DefaultAPIResponse response = null;

            try
            {
                var obj = new clsZone();
                var zoneRecord = await obj.getZoneAsync(payLoad);

                return response = new DefaultAPIResponse()
                {
                    status = true,
                    message = @"success",
                    data = zoneRecord
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
        public async Task<DefaultAPIResponse> calculateFreightCostAsync(clsFreightInput payLoad)
        {
            //calculate freight
            DefaultAPIResponse response = null;

            try
            {
                var objFreight = new clsFreight() { cubic = payLoad.cubic};

                var freightParams = await objFreight.determineFreightBand(payLoad);

                return response = new DefaultAPIResponse()
                {
                    status = true,
                    message = @"success",
                    data = freightParams
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
        public async Task<DefaultAPIResponse> getRateListAsync()
        {
            //TODO: get list of rates in the data store
            DefaultAPIResponse response = null;

            try
            {
                Helper helper = new Helper();
                var rates_data = await helper.getRateTypesAsync();

                return response = new DefaultAPIResponse()
                {
                    status = rates_data.Count() > 0 ? true: false,
                    message = rates_data.Count() > 0 ? $"{rates_data.Count()} records fetched successfully": @"No data",
                    data = rates_data
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

        public async Task<DefaultAPIResponse> createRecordAsync(clsShippingOrder payLoad)
        {
            //TODO: creates a record
            DefaultAPIResponse response = null;

            try
            {
                Helper helper = new Helper();
                var opStatus = await helper.createShippingOrderRecordAsync(payLoad);

                return response = new DefaultAPIResponse()
                {
                    status = opStatus != string.Empty ? true: false,
                    message = opStatus != string.Empty? $"Order saved successfully with order number {payLoad.oShipping.bolNumber}" : @"An error occured. Please see Administrator",
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

    }
}
