#nullable disable
using System.Collections.Generic;
using System.Collections;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using UserManagementAPI.POCOs;
using UserManagementAPI.Response;
using Microsoft.JSInterop.Implementation;
using System.Security.Cryptography;
using System.Transactions;
using System.Drawing;
using System.Drawing.Imaging;

namespace UserManagementAPI.utils
{
    public record clientCreatedRecord
    {
        public int id { get; set; }
        public string acctNo { get; set; }
        public TClient clientObj { get; set; }
    }
    public record countryPrefix
    {
        public int prefixId { get; set; }
        public string prefix { get; set; }
        public CountryLookup? oCountry { get; set; }
    }
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

        public async Task<string> formatShippingOrderNumber(string portCode)
        {
            //creates an order number
            string result = string.Empty;
            string prefix = portCode;
            string suffix = string.Empty;

            try
            {
                var newID = config.TShippings.Max(u => (int)u.Id);

                switch (newID++.ToString().Length)
                {
                    case 1:
                        suffix = string.Format("{0}{1}", @"000000", newID++.ToString());
                        break;
                    case 2:
                        suffix = string.Format("{0}{1}", @"00000", newID++.ToString());
                        break;
                    case 3:
                        suffix = string.Format("{0}{1}", @"0000", newID++.ToString());
                        break;
                    case 4:
                        suffix = string.Format("{0}{1}", @"000", newID++.ToString());
                        break;
                    case 5:
                        suffix = string.Format("{0}{1}", @"00", newID++.ToString());
                        break;
                    case 6:
                        suffix = string.Format("{0}{1}", @"0", newID++.ToString());
                        break;
                    default:
                        suffix = string.Format("{0}", newID++.ToString());
                        break;
                }

                return result = $"{prefix}-{suffix}";
            }
            catch (Exception x)
            {
                Debug.Print(x.Message);
                return result = string.Empty;
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
                    id = x.id,
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

        public async Task<IEnumerable<ShippingPortLookup>> getCountryPortsAsync(int countryId)
        {
            //gets ports for a specific country
            List<ShippingPortLookup> results = null;

            try
            {
                var Q = (from p in config.Tshippingports
                         join cnt in config.TCountryLookups on p.CountryId equals cnt.CountryId
                         where p.CountryId == countryId
                         select new
                         {
                             id = p.Id,
                             portName = p.NameOfport,
                             portCode = p.Portcode,
                             sailingTime = p.TraveltimeInDays,
                             countryId = p.CountryId,
                             countryName = cnt.CountryName,
                             countryCode = cnt.CountryCode
                         });

                var QList = await Q.ToListAsync().ConfigureAwait(false);

                return results = QList.Select(x => new ShippingPortLookup()
                {
                    id = x.id,
                    nameOfport = x.portName,
                    codeOfport = x.portCode,
                    sailingTimeInDays = (int)x.sailingTime,
                    oCountry = new CountryLookup()
                    {
                        id = (int)x.id,
                        nameOfcountry = x.countryName,
                        codeOfcountry = x.countryCode
                    }
                }).ToList();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ShippingPortLookup> getPortAsync(int portID)
        {
            try
            {
                var q = (from tsp in config.Tshippingports
                         join cnt in config.TCountryLookups on tsp.CountryId equals cnt.CountryId
                         where tsp.Id == portID
                         select new
                         {
                             id = tsp.Id,
                             portName = tsp.NameOfport,
                             countryId = tsp.CountryId,
                             nameOfcountry = cnt.CountryName,
                             codeOfport = tsp.Portcode,
                             sailingTime = tsp.TraveltimeInDays
                         });

                var qList = await q.ToListAsync().ConfigureAwait(false);

                return qList
                           .Select(a => new ShippingPortLookup()
                           {
                               id = a.id,
                               nameOfport = a.portName,
                               oCountry = new CountryLookup()
                               {
                                   id = (int) a.countryId,
                                   nameOfcountry = a.nameOfcountry
                               },
                               codeOfport = a.codeOfport,
                               sailingTimeInDays = (int) a.sailingTime
                           }).FirstOrDefault();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public async Task<clientCreatedRecord> createIndividualClientRecordAsync(IndividualCustomerLookup record)
        {
            //TODO: creates an individual client record in the data store
            clientCreatedRecord rec = null;
            TClient obj = null;
            TClientAddress tad = null;

            int _Id = 0;

            try
            {
                using (var cfg = new swContext())
                {
                    _Id = config.TClients.Max(u => (int)u.Id);
                }

                using (config)
                {
                    using var transaction = config.Database.BeginTransaction();
                    try
                    {
                        obj = new TClient()
                        {
                            ClientTypeId = record.oClientType.id,
                            AssociatedCompanyId = record.oCompany.id,
                            ChannelTypeId = record.oChannelType.id,

                            Firstname = record.firstname.Trim().ToUpper(),
                            Middlenames = record.middlenames.Trim().ToUpper(),
                            Surname = record.surname.Trim().ToUpper(),

                            ClientBusinessName = record.clientBusiness.Trim().ToUpper(),
                            MobileNo = record.mobileNo.Trim(),
                            WhatsappNo = record.whatsappNo.Trim(),
                            HomeTelephone = record.homeTelephone.Trim(),
                            WorkTelephone = record.workTelephone.Trim(),
                            ClientEmailAddr = record.clientEmail.Trim(),
                            ClientEmailAddr2 = record.clientEmail2.Trim(),
                            ClientCityId = record.oCity.id,
                            ClientCountryId = record.oCountry.id,
                            ClientPostCode = record.postCode.Trim().ToUpper(),
                            ReferralId = record.oReferral.id,
                            CollectionInstruction = record.collectionInstruction.Trim(),
                            IsShipper = true,
                            ClientAccNo = string.Format("{0}{1}{2}", record.firstname.Trim().ToUpper().Substring(0, 1), record.surname.Trim().ToUpper().Substring(0, 1), (_Id + 1)),
                            ClientPassword = record.clientPassword,
                            CanLogin = true,
                            ConsolidatorId = record.consolidator
                        };


                        await config.AddAsync(obj);
                        await config.SaveChangesAsync();

                        if (record.oAddress.address1.Length > 0)
                        {

                            tad = new TClientAddress()
                            {
                                ClientId = obj.Id,
                                ClientAddr1 = record.oAddress.address1.Trim().ToUpper(),
                                ClientAddr2 = record.oAddress.address2.Trim().ToUpper(),
                                ClientAddr3 = record.oAddress.address3.Trim().ToUpper(),
                                ClientAddr4 = record.oAddress.address4.Trim().ToUpper(),
                                IsUk = record.oAddress.isUK
                            };

                            await config.AddAsync(tad);
                            await config.SaveChangesAsync();
                        }

                        await transaction.CommitAsync();
                    }
                    catch(Exception terr)
                    {
                        await transaction.RollbackAsync();
                    } 
                }

                return rec = new clientCreatedRecord()
                {
                    id = obj.Id,
                    acctNo = obj.ClientAccNo,
                    clientObj = obj
                };
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public async Task<clientCreatedRecord> createCorporateClientRecordAsync(CorporateCustomerLookup record)
        {
            //TODO: creates a corporate account in the tclient table data store
            clientCreatedRecord rec = null;
            TClient obj = null;
            TClientAddress tad = null;

            int _Id = 0;

            try
            {
                using (var cfg = new swContext())
                {
                    _Id = config.TClients.Max(u => (int)u.Id);
                }

                using var transaction = config.Database.BeginTransaction();

                try
                {
                    obj = new TClient()
                    {
                        ClientTypeId = record.oClientType.id,
                        AssociatedCompanyId = record.oCompany.id,
                        ChannelTypeId = record.oChannelType.id,
                        ClientBusinessName = record.clientBusiness.Trim().ToUpper(),
                        MobileNo = record.mobileNo.Trim(),
                        WhatsappNo = record.whatsappNo.Trim(),
                        HomeTelephone = record.homeTelephone.Trim(),
                        WorkTelephone = record.workTelephone.Trim(),
                        ClientEmailAddr = record.clientEmail.Trim(),
                        ClientEmailAddr2 = record.clientEmail2.Trim(),
                        ClientCityId = record.oCity.id,
                        ClientCountryId = record.oCountry.id,
                        ClientPostCode = record.postCode.Trim().ToUpper(),
                        ReferralId = record.oReferral.id,
                        CollectionInstruction = record.collectionInstruction.Trim(),
                        IsShipper = true,
                        ClientAccNo = string.Format("{0}{1}", record.clientBusiness.Trim().ToUpper().Substring(0, 3), (_Id + 1)),
                        ClientPassword = record.clientPassword,
                        CanLogin = true,
                        ConsolidatorId = record.consolidator
                    };

                    await config.AddAsync(obj);
                    await config.SaveChangesAsync();

                    if (record.oAddress.address1.Length > 0)
                    {
                        tad = new TClientAddress()
                        {
                            ClientId = obj.Id,
                            ClientAddr1 = record.oAddress.address1.Trim().ToUpper(),
                            ClientAddr2 = record.oAddress.address2.Trim().ToUpper(),
                            ClientAddr3 = record.oAddress.address3.Trim().ToUpper(),
                            ClientAddr4 = record.oAddress.address4.Trim().ToUpper(),
                            IsUk = record.oAddress.isUK
                        };

                        await config.AddAsync(tad);
                        await config.SaveChangesAsync();
                    }

                    await transaction.CommitAsync();
                }
                catch(Exception tErr)
                {
                    await transaction.RollbackAsync();
                }

                return rec = new clientCreatedRecord()
                {
                    id = obj.Id,
                    acctNo = obj.ClientAccNo,
                    clientObj = obj
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    
        public async Task<PackagepriceLookup> getPackagePriceRecordAsync(int companyID, int itemID)
        {
            PackagepriceLookup result = null;

            try
            {
                var q = (from pp in config.TPackagingPrices
                         join tpi in config.TPackagingItems on pp.PackagingItemId equals tpi.Id
                         join cmp in config.Tcompanies on pp.CompanyId equals cmp.CompanyId
                         join tp in config.Tpackagings on pp.PackagingItemId equals tp.Id
                         where pp.CompanyId == companyID && pp.PackagingItemId == itemID

                         select new
                         {
                             uniqueID = pp.Id,
                             packagingItemId = pp.PackagingItemId,
                             packagingItem = tpi.PackagingItem,
                             packagingDescr = tpi.PackagingDescription,
                             uPrice = pp.UnitPrice,
                             wPrice = pp.WholesalePrice,
                             rPrice = pp.RetailPrice,
                             companyId = pp.CompanyId,
                             companyName = cmp.Company,
                             itemcode = tp.Itemcode
                         });

                var qList = await q.ToListAsync().ConfigureAwait(false);

                result = qList
                            .Select(a => new PackagepriceLookup()
                            {
                                id = a.uniqueID,
                                oPackageItem = new PackageItemLookup()
                                {
                                    id = (int) a.packagingItemId,
                                    name = a.packagingItem,
                                    description = a.packagingDescr
                                },
                                unitPrice = (decimal) a.uPrice,
                                wholesalePrice = (decimal) a.wPrice,
                                retailPrice = (decimal) a.rPrice,
                                oCompany = new CompanyLookup()
                                {
                                    id = (int) a.companyId,
                                    nameOfcompany = a.companyName
                                },
                                nomCode = a.itemcode
                            }).FirstOrDefault();

                return result;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<clsRateType>> getRateTypesAsync()
        {
            //TODO: method fetches the types of rates in the data store
            List<clsRateType> rates = null;

            try
            {
                var q = (from r in config.TRateTypes
                         select new
                         {
                             uniqueID = r.Id,
                             rateDescription = r.Describ
                         });

                var qa = await q.ToListAsync().ConfigureAwait(false);

                return rates = qa
                    .Select(a => new clsRateType()
                    {
                        id = a.uniqueID,
                        describ = a.rateDescription
                    }).ToList();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public async Task<string> createShippingOrderRecordAsync(clsShippingOrder order)
        {
            //change returnvalue to string to return a properly formatted SHIPPING ORDER NUMBER
            //method creates shipping order record in the data store
            //int shippingID = 0;

            try
            {
                //using (var cfg = new swContext())
                //{
                //    shippingID = config.TShippings.Max(u => (int)u.Id);
                //}
                    
                using var transaction = await config.Database.BeginTransactionAsync();

                try
                {
                    //tshipping first
                    TShipping shipping = new TShipping() { 
                        CompanyId = order.oShipping.oCompany.id,
                        IsConsolidated = order.oShipping.isConsolidated == 1 ? true: false,
                        ConsolidatorDescrib = order.oShipping.consolidatedDescription,
                        IsInvoiced = order.oShipping.isInvoiced == 1 ? true: false,
                        InvoiceDate = order.oShipping.invoiceDate,
                        CreatedBy = order.oShipping.createdBy,
                        CustomerId = order.oShipping.customerId,
                        ConsignorId = order.oShipping.consignorId,
                        ReceipientId = order.oShipping.recipientId,
                        NotifyPartyId = order.oShipping.notifyPartyId,
                        SealQty = order.oShipping.sealQty,
                        SealPrice = order.oShipping.sealPrice,
                        RoutingId = order.oShipping.routingId,
                        DelMethodId  = order.oShipping.deliveryMethodId,
                        PayMethodId = order.oShipping.payMethodId,
                        ArrivalPortId = order.oShipping.oArrivalPort.id,
                        ContactInstr = order.oShipping.contactInstruction,
                        OrderNote = order.oShipping.orderNote,
                        CargoDescr = order.oShipping.cargoDescription,
                        OrderCreationDate = order.oShipping.orderCreationDate,
                        OrderStatusId = await order.oShipping.oShippingStatus.getId(),
                        BolNo = await formatShippingOrderNumber(order.oShipping.oArrivalPort.codeOfport)
                    };

                    await config.AddAsync(shipping);
                    await config.SaveChangesAsync();

                    //tshippingorderitems
                    foreach(var item in order.oShippingOrderItems)
                    {
                        TShippingOrderItem shippingItem = new TShippingOrderItem() { 
                            ShippingorderId = shipping.Id,
                            ItemId = await item.item.getID(),
                            Qty = item.quantity,
                            ItemDescription = item.itemDescription,
                            ItemWeight = item.itemWeight,
                            ItemVolume = item.itemVolume,
                            UnitPrice = item.unitPrice,
                            Marks = item.marks,
                            Hscode = item.hscode,
                            LpId = 0,
                            ItemPicPath = string.Format("{0}{1}_{2}.{3}",ConfigObject.IMG_FOLDER_PATH, shipping.BolNo, item.item.name,@"png")
                        };

                        await config.AddAsync(shippingItem);
                        await config.SaveChangesAsync();
                    }

                    //tshippingordercharges
                    foreach(var ch in order.oShippingOrderCharges)
                    {
                        TShippingOrderCharge shippingOrderCharge = new TShippingOrderCharge() { 
                            ShippingOrderId = shipping.Id,
                            ChargeId = ch.oCharge.id,
                            ChargeAmt = ch.chargeAmt,
                            ChargeDescription = ch.chargeDescription,
                            CurrencyId = await ch.oCurrency.getID()
                        };

                        await config.AddAsync(shippingOrderCharge);
                        await config.SaveChangesAsync();
                    }

                    //tshippingconsigneeitem
                    TShippingConsigneeItem tconitem = new TShippingConsigneeItem()
                    {
                        ShippingOrderId = shipping.Id,
                        ItemValue = order.oConsigneeItem.itemValue,
                        SealNo = order.oConsigneeItem.sealNumber,
                        CustomerRef = order.oConsigneeItem.customerRef,
                        InputDate = order.oConsigneeItem.inputDate,
                        LatestshippingDate = order.oConsigneeItem.shipByDate,
                        Blfreight = order.oConsigneeItem.BLFreight,
                        FreightPayableId = order.oConsigneeItem.freightPayable
                    };

                    await config.AddAsync(tconitem);
                    await config.SaveChangesAsync();


                    order.oShipping.shippingOrderId = shipping.Id;
                    order.oShipping.bolNumber = shipping.BolNo;

                    //commit transaction asynchronously
                    await transaction.CommitAsync();
                    return shipping.BolNo;
                }
                catch(Exception tex)
                {
                    await transaction.RollbackAsync();
                    return string.Empty;
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> saveImageAsync(string uniqueIdentifier, string base64String)
        {
            //TODO: saves an image file to a specified directory, using the name of a unique identifier
            //unique identifier can be order number or bill of laden
            bool bln = false;
            var filePath = string.Format("{0}{1}.{2}", ConfigObject.IMG_FOLDER_PATH, uniqueIdentifier,@"png");

            try
            {
                base64String = base64String.Replace("data:image/png;base64,", "");
                byte[] imageB = Convert.FromBase64String(base64String);

                //creating an image from the byte array
                using (MemoryStream ms = new MemoryStream(imageB))
                {
                    Image img = Image.FromStream(ms);

                    //saving image on file directory
                    img.Save(filePath,System.Drawing.Imaging.ImageFormat.Png);
                    bln = true;
                }

                return bln;
            }
            catch(Exception ex)
            {
                return bln;
            }
        }

        public async Task<bool> SvImageAsync(string uniqueIdentifier, string base64String)
        {
            try
            {
                //var fileWriteTo = string.Format("{0}{1}.{2}", ConfigObject.IMG_FOLDER_PATH, uniqueIdentifier, @"png");
                var fullPath = Path.Combine(Directory.GetCurrentDirectory(), ConfigObject.IMG_FOLDER_PATH, string.Format("{0}.{1}", uniqueIdentifier, "png"));

                //base64String = base64String.Replace("data:image/png;base64,", "");
                var str = base64String.Split(',');
                byte[] imageB = Convert.FromBase64String(str[1]);

                using (MemoryStream ms = new MemoryStream(imageB))
                {
                    using Stream streamWriteTo = File.Open(fullPath, FileMode.Create);
                    ms.Position = 0;

                    await ms.CopyToAsync(streamWriteTo);
                }

                return true;
            }
            catch(Exception ex)
            {
                return false;
            }          
        }

        public async Task<IEnumerable<countryPrefix>> getAllCountryPrefixListAsync()
        {
            List<countryPrefix> cntPrefixes = new List<countryPrefix>();

            try
            {
                var q = (from cnt in config.TCountryLookups
                         where cnt.CountryId > 1 && cnt.CountryId < 10000
                         select new
                         {
                             countryId = cnt.CountryId,
                             countryName = cnt.CountryName,
                             countryCode = cnt.CountryCode == null ? string.Empty: cnt.CountryCode,
                             countryPrefix = cnt.PreFix
                         });

                var qList = await q.ToListAsync().ConfigureAwait(false);

                var dtPrefixes = qList
                                .Select(a => new countryPrefix()
                                {
                                    prefixId = a.countryId,
                                    prefix = a.countryPrefix.Trim(),
                                    oCountry = new CountryLookup()
                                    {
                                        id = a.countryId,
                                        codeOfcountry = a.countryCode,
                                        nameOfcountry = a.countryName
                                    }
                                }).ToList();

                //loop over to find countries with 2 or more prefix
                foreach(var d in dtPrefixes)
                {
                    if (d.prefix.Contains('|'))
                    {
                        var str = d.prefix.Split('|');
                        foreach(var s in str)
                        {
                            var o = new countryPrefix()
                            {
                                prefixId = d.prefixId,
                                prefix = s,
                                oCountry = new CountryLookup()
                                {
                                    id = d.oCountry.id,
                                    codeOfcountry = d.oCountry.codeOfcountry,
                                    nameOfcountry = d.oCountry.nameOfcountry
                                }
                            };

                            cntPrefixes.Add(o);
                        }
                    }
                    else
                    {
                        cntPrefixes.Add(d);
                    }
                }

                return cntPrefixes;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        #region client - get all

        public async Task<IEnumerable<IndividualCustomerLookup>> getAllClientAsync()
        {
            //TODO: fetches all customers
            //method has pagination features
            List<IndividualCustomerLookup> customers = null;

            try
            {
                var Query = (from tc in config.TClients
                             join ct in config.TClientTypes on tc.ClientTypeId equals ct.Id
                             join cmp in config.Tcompanies on tc.AssociatedCompanyId equals cmp.CompanyId
                             join chn in config.TChannelTypes on tc.ChannelTypeId equals chn.ChannelTypeId
                             join cty in config.TCities on tc.ClientCityId equals cty.Id
                             join cntr in config.TCountryLookups on tc.ClientCountryId equals cntr.CountryId
                             join rf in config.Tclientreferralsources on tc.ReferralId equals rf.Id
                             join crt in config.Tusrs on tc.CreatedBy equals crt.UsrId
                             join usr in config.Tusrs on tc.LastModifiedBy equals usr.UsrId

                             select new
                             {
                                 uniqueID = tc.Id,
                                 clientType = ct.Describ,
                                 clientTypeId = ct.Id,
                                 associatedCompany = cmp.Company,
                                 associatedCompanyId = tc.AssociatedCompanyId,
                                 channelType = chn.Channel,
                                 channelTypeId = tc.ChannelTypeId,
                                 firstname = tc.Firstname,
                                 middlenames = tc.Middlenames,
                                 surname = tc.Surname,
                                 clientBusiness = tc.ClientBusinessName,
                                 mobileNo = tc.MobileNo,
                                 whatsappNo = tc.WhatsappNo,
                                 homeTel = tc.HomeTelephone,
                                 workTel = tc.WorkTelephone,
                                 emailAddr = tc.ClientEmailAddr,
                                 emailAddr2 = tc.ClientEmailAddr2,
                                 accNo = tc.ClientAccNo,
                                 cityId = tc.ClientCityId,
                                 city = cty.CityName,
                                 countryId = tc.ClientCountryId,
                                 nameOfcountry = cntr.CountryName,
                                 postCode = tc.ClientPostCode,
                                 referralId = tc.ReferralId,
                                 referral = rf.ReferralSource,
                                 collectionInstruction = tc.CollectionInstruction
                             });

                var customerList = await Query.ToListAsync().ConfigureAwait(false);

                //convert list to type genericcustomerlookup
                return customers = customerList
                        .Select(q => new IndividualCustomerLookup()
                        {
                            id = q.uniqueID,
                            oClientType = new ClientTypeLookup()
                            {
                                id = (int)q.clientTypeId,
                                clientTypeDescrib = q.clientType
                            },
                            accountNo = q.accNo,
                            oCity = new CityLookup()
                            {
                                id = (int)q.cityId,
                                nameOfcity = q.city
                            },
                            oCountry = new CountryLookup()
                            {
                                id = (int)q.countryId,
                                nameOfcountry = q.nameOfcountry
                            },
                            oCompany = new CompanyLookup()
                            {
                                id = (int)q.associatedCompanyId,
                                nameOfcompany = q.associatedCompany
                            },
                            oChannelType = new ChannelTypeLookup()
                            {
                                id = (int)q.channelTypeId,
                                nameOfchannel = q.channelType
                            },
                            oReferral = new ReferralLookup()
                            {
                                id = (int)q.referralId,
                                sourceOfReferral = q.referral
                            },
                            postCode = q.postCode,
                            mobileNo = q.mobileNo,
                            whatsappNo = q.whatsappNo,
                            homeTelephone = q.homeTel,
                            workTelephone = q.workTel,
                            clientEmail = q.emailAddr
                        }).ToList();
            }
            catch(Exception x)
            {
                throw x;
            }
        }

        #endregion

    }
}
