#nullable disable
using UserManagementAPI.Resources;
using UserManagementAPI.Response;
using UserManagementAPI.POCOs;
using UserManagementAPI.Resources.Interfaces;
using UserManagementAPI.utils;
using System.Reflection.Metadata.Ecma335;

using Xero.NetStandard.OAuth2.Api;
using Xero.NetStandard.OAuth2.Client;
using Xero.NetStandard.OAuth2.Config;
using Xero.NetStandard.OAuth2.Model.Accounting;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Security.Cryptography.Xml;
using UserManagementAPI.Models;

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

            try
            {
                Helper helper = new Helper();
                var dictSaleType = await helper.GetSalesTypeAsync();

                return rsp = new DefaultAPIResponse()
                {
                    status = dictSaleType.Count() > 0 ? true : false,
                    message = dictSaleType.Count() > 0 ? $"{dictSaleType.Count()} records fetched" : @"No data",
                    data = dictSaleType
                };
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
            List<OrderSummaryDetails> results = new List<OrderSummaryDetails>();

            try
            {
                var objFreight = new clsFreight() { cubic = payLoad.cubic};

                var freightParams = await objFreight.determineFreightBand(payLoad);
                var freightUSD = await objFreight.determineFreightRateUSD(freightParams.value);

                var CnD = await objFreight.determineClearanceAndDelivery(payLoad);

                if(payLoad.deliveryMethodId == 9)
                {
                    //door to door and duty paid
                    var wifDuty = await objFreight.determineWifDuty(payLoad);
                    if (wifDuty != null)
                    {
                        results.Add(wifDuty);
                    }
                }

                CountryLookup countryObj = await new CountryLookup() { }.Get(payLoad.oCountry.id);
                if (countryObj.nameOfcountry == @"JAMAICA")
                {
                    var jTS = await objFreight.determineJTSClearanceAndDelivery(payLoad);
                    if (jTS != null)
                    {
                        OrderSummaryDetails jcd = new OrderSummaryDetails()
                        {
                            id = 31,
                            key = @"jTS",
                            value = (decimal)jTS.jtscd
                        };

                        OrderSummaryDetails jdt = new OrderSummaryDetails()
                        {
                            id = 32,
                            key = @"jTSDuty",
                            value = (decimal)jTS.jtsduty
                        };

                        results.Add(jcd);
                        results.Add(jdt);
                    }
                }
                

                results.Add(freightParams);
                results.Add(freightUSD);
                results.Add(CnD);

                
                return response = new DefaultAPIResponse()
                {
                    status = true,
                    message = @"success",
                    //data = freightParams
                    data = results.ToList()
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

                //save uploaded image
                foreach(var item in payLoad.oShippingOrderItems)
                {
                    if (item.picturePath != null)
                    {
                        await helper.SvImageAsync(string.Format("{0}_{1}", payLoad.oShipping.bolNumber, item.item.name), item.picturePath);
                    }                   
                }

                CountryLookup countryObj = await new CountryLookup() { }.Get(payLoad.destinationCountry);
                if (countryObj.nameOfcountry == @"JAMAICA")
                {
                    //compute jts earnings and jts duties
                    clsShippingOrderCommission commissionObj = new clsShippingOrderCommission() { 
                        shippingOrderId = payLoad.oShipping.shippingOrderId,
                        wifduty = payLoad.wifduty,
                        jtsduty = payLoad.jtsduty,
                        dutyEarnings = (decimal) (payLoad.wifduty - payLoad.jtsduty),
                        wifcd = payLoad.wifcd,
                        jtscd = payLoad.jtsearning,
                        earningsOnCnD = (payLoad.wifcd - payLoad.jtsearning),
                        cubicPerMeter = payLoad.cubic
                    };

                    bool bln = await helper.saveShippingOrderCommmissionAsync(commissionObj);
                }

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

        public async Task<DefaultAPIResponse> createRateOfExchangeAsync(clsROE payLoad)
        {
            //TODO: creates a rate of exchange in the data store
            DefaultAPIResponse rsp = null;

            try
            {
                bool bln = await payLoad.AddRateOfExchangeAsync();
                return rsp = new DefaultAPIResponse() { 
                    status = bln,
                    message = bln ? $"Rate of exchange {payLoad.roe} saved successfully into database": @"failed",
                    data = payLoad
                };
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

        public async Task<PaginationAPIResponse> ListRateOfExchangeAsync(int page, int pageSize)
        {
            //TODO: gets a paginated data of rate of exchange
            PaginationAPIResponse rsp = null;

            try
            {
                clsROE obj = new clsROE();
                var roeList = await obj.Get();

                var totalCount = roeList.Count();
                var totalPages = (int)Math.Ceiling((decimal)totalCount / pageSize);

                return rsp = new PaginationAPIResponse() {
                    status = totalCount > 0 ? true : false,
                    message = totalCount > 0 ? $"Page {page} of {totalPages} fetched successfully!!!" : @"failed",
                    total = totalCount,
                    data = roeList.Skip((page - 1) * pageSize)
                                    .Take(pageSize)
                                    .ToList()
                };
            }
            catch(Exception x)
            {
                return rsp = new PaginationAPIResponse() { 
                    status = false,
                    message = $"error: {x.Message}"
                };
            }
        }

        public async Task<DefaultAPIResponse> GetAppropriateMonths(int YR)
        {
            //TODO: gets the appropriate months for the year selected
            DefaultAPIResponse rsp = null;
            List<clsMonth> result = new List<clsMonth>();

            try
            {
                clsMonth month = new clsMonth();
                clsROE roe = new clsROE();

                var monthList = await month.Get();
                var roeList = await roe.Get();

                var roeYearEntries = roeList.Where(r => r.year == YR).ToList();

                result = monthList.ToList();

                //iteration
                foreach(var m in monthList.ToList())
                {
                    foreach(var ro in roeYearEntries)
                    {
                        if (m.nameOfmonth == ro.month)
                        {
                            //remove from month list
                            result.Remove(m);
                        }
                    }
                }

                int totalcount = result.Count();

                //return api response object
                return rsp = new DefaultAPIResponse() { 
                    status = totalcount > 0 ? true: false,
                    message = totalcount > 0 ? $"{totalcount} records fetched successfully": @"failed",
                    data = result.ToList()
                };
            }
            catch(Exception x)
            {
                return rsp = new DefaultAPIResponse() { 
                    status = false,
                    message = $"error: {x.Message}"
                };
            }
        }

        public async Task<UploadAPIResponse> UploadRateOfExchange(IEnumerable<clsROE> payLoad)
        {
            //TODO: upload rate of exchange into the data store
            UploadAPIResponse rsp = null;
            int success = 0;
            int failed = 0;
            List<clsROE> successList = new List<clsROE>();
            List<clsROE> errorList = new List<clsROE>();
            List<string> errors = new List<string>();

            try
            {
                foreach(var item in payLoad)
                {
                    //check if record exists
                    var o = await item.Get(item.month, item.year);
                    if (o == null)
                    {
                        //item does not exist. proceed to save item
                        if (await item.AddRateOfExchangeAsync())
                        {
                            success += 1;
                            successList.Add(item);
                        }
                        else {
                            errorList.Add(item);
                            failed += 1; 
                        }
                    }
                    else
                    {
                        failed += 1;
                        errors.Add($"Rate of Exchange for month {item.month} and year {item.year} already exist in the datastore");
                    }
                }

                return rsp = new UploadAPIResponse() {
                    status = true,
                    message = $"Total records= {payLoad.Count().ToString()}, successful inserts= {success.ToString()}, failed inserts= {failed.ToString()}",
                    data = successList,
                    successCount = success,
                    errorList = errorList,
                    errorMessageList = errors,
                    errorCount = failed
                };
            }
            catch(Exception x)
            {
                return rsp = new UploadAPIResponse() { 
                    status = false,
                    message = $"error: {x.Message}"
                };
            }
        }

        public async Task<PaginationAPIResponse> GetShipmentReportAsync(DateTime df, DateTime dt, int page, int pageSize)
        {
            //TODO: gets the shipment report for the specified date range
            PaginationAPIResponse rsp = null;
            List<clsShipmentReport> resultant = null;
            List<clsShipmentReport> shipmentReports = new List<clsShipmentReport>();
            const decimal const_CBM = 0.30m;

            try
            {
                clsShipmentReport obj = new clsShipmentReport();
                var dta = await obj.GetShipmentReportAsync(df, dt);

                resultant = dta.ToList();

                //iteration and computation of duties
                foreach(var r in resultant)
                {
                    var _rateYr = r.dte.Year;
                    var _rateMonth = r.dte.Month.ToString();

                    if (_rateMonth.Length == 1)
                    {
                        _rateMonth = string.Format("{0}{1}", "0", _rateMonth);
                    }

                    var monthString = await new clsMonth() { }.Get(_rateMonth);
                    var roeObj = await new clsROE() { }.Get(monthString, _rateYr);

                    r.roe = roeObj.fx;
                    
                    //wif duties
                    //r.wifduties = r.wifduties * ((r.cbm / const_CBM) * r.roe);
                    r.jtsduties = ((r.cbm / const_CBM) * r.roe);

                    r.earningsOnDuties = (r.wifduties - r.jtsduties);
                    r.totalEarnings = (r.earningsOnDuties + r.earningsOnCD);

                    shipmentReports.Add(r);
                }

                int totalCount = shipmentReports.Count;
                int totalPages = (int)Math.Ceiling((decimal)totalCount / pageSize);

                rsp = new PaginationAPIResponse() { 
                    status = totalCount > 0 ? true: false,
                    message = totalCount > 0 ? @"success" : @"failed",
                    total = totalCount,
                    data = shipmentReports
                                  .Skip((page - 1) * pageSize)
                                  .Take(pageSize)
                                  .ToList()
                };

                return rsp;
            }
            catch(Exception x)
            {
                return rsp = new PaginationAPIResponse() { 
                    status = false,
                    message = $"error: {x.Message}"
                };
            }
        }
    
        public async Task<DefaultAPIResponse> GetCollectionAndDelivery(DateTime df, DateTime dt)
        {
            //TODO: gets deliveries and collections for client display
            DefaultAPIResponse rsp = null;

            try
            {
                var cnd = new clsCollectionAndDelivery();
                var deliv = await cnd.getDeliveryAsync(df, dt);

                int totalCount = deliv.ToList().Count();

                return rsp = new DefaultAPIResponse() { 
                    status = totalCount > 0 ? true: false,
                    message = totalCount > 0 ? @"success": @"failed",
                    data = deliv.ToList()
                };
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
