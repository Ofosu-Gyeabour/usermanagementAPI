#nullable disable
using System.Collections.Generic;
using System.Collections;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using UserManagementAPI.POCOs;
using UserManagementAPI.Response;
using Microsoft.JSInterop.Implementation;

namespace UserManagementAPI.utils
{
    public class Helper
    {
        swContext config;
        public Helper() {
            config = new swContext();
        }

        public async Task<bool> AmendUserModules(string _usr, string _newProfile)
        {
            try
            {
                try
                {
                    var pObj = await config.TProfiles.Where(p => p.ProfileName == _newProfile).FirstOrDefaultAsync();
                    var u = await config.Tusrs.Where(x => x.Usrname == _usr).FirstOrDefaultAsync();
                    if ((u != null) & (pObj != null))
                    {
                        //amend associated profile id
                        u.ProfileId = pObj.ProfileId;
                    }

                    await config.SaveChangesAsync();

                    return true;
                }
                catch (Exception transErr)
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<int> getCountryIdAsync(string name)
        {
            //gets the country Id
            try
            {
                var obj = await config.TCountryLookups.Where(x => x.CountryName == name).FirstOrDefaultAsync();
                return obj.CountryId;
            }
            catch (Exception x)
            {
                Debug.Print($"error: {x.Message}");
                return 0;
            }
        }

        public async Task<TCountryLookup> getCountryAsync(int id)
        {
            //gets the country object from the data store
            TCountryLookup _country;

            try
            {
                _country = await config.TCountryLookups.Where(x => x.CountryId == id).FirstOrDefaultAsync();
                return _country;
            }
            catch (Exception x)
            {
                return _country = new TCountryLookup() { CountryId = 0 };
            }
        }

        public async Task<CountryLookup> getCountry(int Id)
        {
            CountryLookup obj = null;
            try
            {
                var dt = await config.TCountryLookups.Where(x => x.CountryId == Id).Include(r => r.Region).FirstOrDefaultAsync();
                if (dt != null)
                {
                    obj = new CountryLookup()
                    {
                        id = Id,
                        nameOfcountry = dt.CountryName,
                        codeOfcountry = dt.CountryCode,
                        oRegion = new RegionLookup()
                        {
                            id = dt.Region.RegionId,
                            nameOfregion = dt.Region.RegionName
                        }
                    };
                }

                return obj;
            }
            catch (Exception x)
            {
                return obj = new CountryLookup() { id = 0 };
            }
        }
        public async Task<TCountryLookup> getCountryAsync(string name)
        {
            //gets the country object from the data store
            TCountryLookup _country;

            try
            {
                _country = await config.TCountryLookups.Where(x => x.CountryName == name).FirstOrDefaultAsync();
                return _country;
            }
            catch (Exception x)
            {
                return _country = new TCountryLookup() { CountryId = 0 };
            }
        }

        public async Task<TCity> getCityAsync(string name)
        {
            //gets city record using city name
            TCity obj = null;

            try
            {
                obj = await config.TCities.Where(x => x.CityName == name).FirstOrDefaultAsync();
                return obj;
            }
            catch (Exception ex)
            {
                return obj = new TCity() { Id = 0 };
            }
        }

        public async Task<AddressLookup> getClientAddressAsync(int clientID)
        {
            AddressLookup obj = null;

            try
            {
                var Q = (from addr in config.TClientAddresses
                         where addr.ClientId == clientID

                         select new
                         {
                             uniqueId = addr.Id,
                             id = clientID,
                             address1 = addr.ClientAddr1 == null ? string.Empty : addr.ClientAddr1,
                             address2 = addr.ClientAddr2 == null ? string.Empty : addr.ClientAddr2,
                             address3 = addr.ClientAddr3 == null ? string.Empty : addr.ClientAddr3,
                             isUK = addr.IsUk
                         });

                var QList = await Q.ToListAsync().ConfigureAwait(false);

                obj = QList.Select(x => new AddressLookup()
                {
                    id = x.uniqueId,
                    clientId = x.id,
                    address1 = x.address1,
                    address2 = x.address2,
                    address3 = x.address3,
                    isUK = (bool)x.isUK
                }).FirstOrDefault();

                return obj;
            }
            catch (Exception x)
            {
                return obj;
            }
        }

        //writes log
        public async Task<bool> WriteLogAsync(Log oLogger)
        {
            try
            {
                var obj = new TLogger()
                {
                    LogId = oLogger.id,
                    LogEvent = oLogger.eventId,
                    LogActor = oLogger.actor,
                    LogEntity = oLogger.entity,
                    LogEntityValue = oLogger.entityValue,
                    CompanyId = oLogger.companyId,
                    LogDate = oLogger.logDate
                };

                await config.TLoggers.AddAsync(obj);
                await config.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<TEvent> getEventLookupAsync(string _evtDescription)
        {
            //gets the event object using the Id
            TEvent objEvent = null;
            try
            {
                var obj = await config.TEvents.Where(x => x.EventDescription == _evtDescription).FirstOrDefaultAsync();
                if (obj.Id > 0)
                {
                    objEvent = obj;
                }

                return objEvent;
            }
            catch (Exception x)
            {
                return objEvent;
            }
        }

        public async Task<SingleParam> createOrderNumber(string _orderType)
        {
            //creates an order number
            SingleParam result = null;

            try
            {
                var newID = config.TAdhocs.Max(u => (int)u.Id);

                result = new SingleParam() { stringValue = newID.ToString() };
                return result;
            }
            catch (Exception x)
            {
                Debug.Print(x.Message);
                return result;
            }
        }

        public async Task<TAdhocType> getAdhocType(string adhocName)
        {
            //gets the unique ID of the adhoc object

            try
            {
                var obj = await config.TAdhocTypes.Where(x => x.AdhocName == adhocName.Trim()).FirstOrDefaultAsync();
                return obj;
            }
            catch (Exception x)
            {
                return null;
            }
        }

        public async Task<TPaymentMethod> getPaymentMethod(string _method)
        {
            //gets a payment method record
            TPaymentMethod obj = null;

            try
            {
                obj = await config.TPaymentMethods.Where(x => x.Method == _method).FirstOrDefaultAsync();
                return obj;
            }
            catch (Exception x)
            {
                return obj;
            }
        }

        public async Task<IEnumerable<ChargeEngineLookup>> getAllChargesAsync()
        {
            //gets all charges in the data store
            List<ChargeEngineLookup> engineList = null;

            try
            {
                var query = (from c in config.TChargeEngines
                             join clk in config.TChargeLookups on c.ChargeId equals clk.Id
                             join ot in config.TOrderTypes on c.OrdertypeId equals ot.Id

                             select new
                             {
                                 id = c.Id,
                                 ordertype = ot.Describ,
                                 ordertype_Id = c.OrdertypeId,
                                 chargeId = c.ChargeId,
                                 chargeDescription = clk.Charge,
                                 cRate = c.ChargeRate,
                                 amtThreshold = c.ThresholdAmt
                             });

                var queryList = await query.ToListAsync().ConfigureAwait(false);
                engineList = queryList.Select(q => new ChargeEngineLookup()
                {
                    id = q.id,
                    oOrderType = new OrderTypeLookup()
                    {
                        id = (int)q.ordertype_Id,
                        orderDescription = q.ordertype
                    },
                    oChargeLookup = new ChargeLookup() {
                        id = (int)q.chargeId,
                        nameOfcharge = q.chargeDescription
                    },
                    chargeRate = q.cRate,
                    thresholdAmt = q.amtThreshold
                }).ToList();

                return engineList;
            }
            catch (Exception x)
            {
                throw x;
            }
        }

        public async Task<IEnumerable<ChargeEngineLookup>> getChargesAsync(OrderTypeLookup _orderType)
        {
            //method gets all charges for an order type
            List<ChargeEngineLookup> engineList = null;

            try
            {
                var query = (from c in config.TChargeEngines
                             join clk in config.TChargeLookups on c.ChargeId equals clk.Id
                             join ot in config.TOrderTypes on c.OrdertypeId equals ot.Id
                             where ot.Describ == _orderType.orderDescription

                             select new
                             {
                                 id = c.Id,
                                 ordertype = ot.Describ,
                                 ordertype_Id = c.OrdertypeId,
                                 chargeId = c.ChargeId,
                                 chargeDescription = clk.Charge,
                                 cRate = c.ChargeRate,
                                 amtThreshold = c.ThresholdAmt
                             });

                var queryList = await query.ToListAsync().ConfigureAwait(false);
                engineList = queryList.Select(q => new ChargeEngineLookup()
                {
                    id = q.id,
                    oOrderType = new OrderTypeLookup()
                    {
                        id = (int)q.ordertype_Id,
                        orderDescription = q.ordertype
                    },
                    oChargeLookup = new ChargeLookup() {
                        id = (int)q.chargeId,
                        nameOfcharge = q.chargeDescription
                    },
                    chargeRate = q.cRate,
                    thresholdAmt = q.amtThreshold
                }).ToList();

                return engineList;
            }
            catch (Exception x)
            {
                throw x;
            }
        }

        public async Task<IEnumerable<ChargeLookup>> getChargeLookupAsync()
        {
            //TODO: get charge lookups
            List<ChargeLookup> results = null;

            try
            {
                using (config)
                {
                    var Q = await config.TChargeLookups.ToListAsync();

                    if (Q != null)
                    {
                        results = new List<ChargeLookup>();
                        foreach (var item in Q)
                        {
                            var obj = new ChargeLookup()
                            {
                                id = item.Id,
                                nameOfcharge = item.Charge
                            };

                            results.Add(obj);
                        }
                    }

                    return results;
                }
            }
            catch (Exception x)
            {
                return results;
            }
        }

        public async Task<bool> addChargeEngineEntryAsync(ChargeEngineLookup obj)
        {
            //TODO: create a charge engine entry
            try
            {
                using (config)
                {
                    try
                    {
                        TChargeEngine chargeEngine = new TChargeEngine() {
                            OrdertypeId = obj.oOrderType.id,
                            ChargeId = obj.oChargeLookup.id,
                            ChargeRate = obj.chargeRate,
                            ThresholdAmt = obj.thresholdAmt,
                            ThresholdRate = obj.thresholdRate,
                            IsLabel = obj.isLabel
                        };

                        await config.AddAsync(chargeEngine);
                        await config.SaveChangesAsync();

                        return true;
                    }
                    catch (Exception x)
                    {
                        Debug.Print(x.Message);
                        return false;
                    }
                }
            }
            catch (Exception xx)
            {
                return false;
            }
        }

        public async Task<IEnumerable<TOrderType>> GetOrderTypesAsync()
        {
            //TODO: gets the list of order types in the data store
            try
            {
                var dd = await config.TOrderTypes.ToListAsync();
                return dd;
            }
            catch (Exception xx)
            {
                throw xx;
            }
        }

        public async Task<bool> doesChargeExist(string chargeDescrib)
        {
            //TODO: determine if charge or tax already exist in the data store
            try
            {
                using (config)
                {
                    var obj = await config.TChargeLookups.Where(c => c.Charge == chargeDescrib).FirstOrDefaultAsync();

                    return obj == null ? false : true;
                }
            }
            catch (Exception x)
            {
                return false;
            }
        }

        public async Task<IEnumerable<OrderSummaryDetails>> getOrderSummaryKeys(OrderTypeLookup ordertypelookup)
        {
            //TODO: use order lookup to fetch accounting keys for order computation
            List<OrderSummaryDetails> resultKeys = null;

            try
            {
                var query = (from ce in config.TChargeEngines
                             join ot in config.TOrderTypes on ce.OrdertypeId equals ot.Id
                             join ch in config.TChargeLookups on ce.ChargeId equals ch.Id
                             where ce.OrdertypeId == ordertypelookup.id
                             select new
                             {
                                 id = ce.Id,
                                 chargeName = ch.Charge,
                                 rate = ce.ChargeRate
                             });

                var qList = await query.ToListAsync().ConfigureAwait(false);
                resultKeys = qList.Select(x => new OrderSummaryDetails()
                {
                    key = x.chargeName,
                    value = (decimal)x.rate
                }).ToList();

                return resultKeys;
            }
            catch (Exception xx)
            {
                return resultKeys;
            }
        }

        public async Task<IEnumerable<OrderSummaryDetails>> updateOrderSummaryKeys(OrderStat obj)
        {
            List<OrderSummaryDetails> results = new List<OrderSummaryDetails>();
            try
            {
                decimal deliveryFee = 0m;
                decimal congestionFee = 0m;
                decimal vatFee = 0m;

                foreach (var item in obj.summary)
                {
                    if (item.key == "DELIVERY")
                    {
                        deliveryFee = (decimal)(obj.totAmt * obj.deliveryCharge);
                        item.value = deliveryFee;
                    }

                    if (item.key == @"CONGESTION CHARGE")
                    {
                        congestionFee = (decimal)(obj.totAmt * obj.congestionCharge);
                        item.value = congestionFee;
                    }

                    if (item.key == @"VAT")
                    {
                        item.value = (decimal)(obj.totAmt * obj.VAT);
                    }

                    if (item.key == @"ITEMS TOTAL")
                    {
                        item.value = (decimal)obj.itemCount;
                    }

                    if (item.key == @"TOTAL")
                    {
                        item.value = (decimal)obj.totAmt;
                    }

                    results.Add(item); 
                }

                return results;
            }
            catch(Exception ex)
            {
                Debug.Print(ex.Message);
                return results;
            }
        }


    }
}
