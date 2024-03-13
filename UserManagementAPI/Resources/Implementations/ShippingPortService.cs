#nullable disable

using UserManagementAPI.POCOs;
using UserManagementAPI.Response;
using UserManagementAPI.Resources.Interfaces;
using System.Diagnostics;
using UserManagementAPI.utils;
using UserManagementAPI.Models;

namespace UserManagementAPI.Resources.Implementations
{
    public class ShippingPortService : IShippingPortService
    {
        swContext config;
        public ShippingPortService()
        {
            config = new swContext();
        }

        public DefaultAPIResponse GetShippingPortsAsync()
        {
            //gets all shipping ports in the data store
            DefaultAPIResponse response = null;
            List<ShippingPortLookup> ports = null;

            try
            {
                var query = (from sp in config.Tshippingports
                             join c in config.TCountryLookups on sp.CountryId equals c.CountryId
                             select new
                             {
                                 id = sp.Id,
                                 nameOfport = sp.NameOfport,
                                 codeOfport = sp.Portcode,
                                 sailingTime = sp.TraveltimeInDays,
                                 countryId = c.CountryId,
                                 nameOfcountry = c.CountryName
                             });

                if (query != null)
                {
                    ports = new List<ShippingPortLookup>();
                    foreach(var q in query)
                    {
                        var obj = new ShippingPortLookup()
                        {
                            id = q.id,
                            nameOfport = q.nameOfport,
                            codeOfport = q.codeOfport,
                            sailingTimeInDays = (int) q.sailingTime,
                            oCountry = new CountryLookup()
                            {
                                id = q.countryId,
                                nameOfcountry = q.nameOfcountry
                            }
                        };

                        ports.Add(obj);
                    }

                    response = new DefaultAPIResponse()
                    {
                        status = true,
                        message = @"success",
                        data = ports
                    };
                }
                else { response = new DefaultAPIResponse() { status = false, message = @"No data" }; }

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
        public async Task<DefaultAPIResponse> CreateShippingPortAsync(ShippingPortLookup payLoad)
        {
            //create a shipping port resource in the data store
            DefaultAPIResponse response = null;

            try
            {
                var dt = await config.Tshippingports.Where(x => x.NameOfport == payLoad.nameOfport).FirstOrDefaultAsync();
                if (dt == null)
                {
                    Tshippingport objShippingPort = new Tshippingport()
                    {
                        NameOfport = payLoad.nameOfport,
                        CountryId = payLoad.oCountry.id,
                        Portcode = payLoad.codeOfport,
                        TraveltimeInDays = payLoad.sailingTimeInDays
                    };

                    await config.AddAsync(objShippingPort);
                    await config.SaveChangesAsync();

                    response = new DefaultAPIResponse()
                    {
                        status = true,
                        message = @"success",
                        data = payLoad
                    };
                }
                else
                {
                    response = new DefaultAPIResponse() { status = false, message = $"Port {payLoad.nameOfport} already exist in the data store" };
                }

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

        public async Task<UploadAPIResponse> UploadShippingPortAsync(IEnumerable<ShippingPortLookup> payLoad)
        {
            UploadAPIResponse response = null;
            int success = 0;
            int failed = 0;
            List<ShippingPortLookup> successList = new List<ShippingPortLookup>();
            List<ShippingPortLookup> errorList = new List<ShippingPortLookup>();
            List<string> errors = new List<string>();
            TCountryLookup cc = null;

            try
            {
                foreach(var record in payLoad)
                {
                    try
                    {
                        using (var cfg = new swContext())
                        {
                            cc = await cfg.TCountryLookups.Where(x => x.CountryName == record.oCountry.nameOfcountry.Trim()).FirstOrDefaultAsync();
                        }

                        if (cc != null)
                        {
                            var query = (from sp in config.Tshippingports
                                         join c in config.TCountryLookups on sp.CountryId equals c.CountryId
                                         where sp.NameOfport == record.nameOfport.Trim() &&
                                         sp.Portcode == record.codeOfport &&
                                         c.CountryId == cc.CountryId &&
                                         sp.TraveltimeInDays == record.sailingTimeInDays
                                         select new
                                         {
                                             id = sp.Id,
                                             port = sp.NameOfport,
                                             codeOfport = sp.Portcode,
                                             countryName = c.CountryName,
                                             countryId = c.CountryId,
                                             sailingDays = sp.TraveltimeInDays
                                         });

                            if (query.Count() == 0)
                            {
                                Tshippingport obj = new Tshippingport() { 
                                    NameOfport = record.nameOfport.Trim(),
                                    CountryId = cc.CountryId,
                                    Portcode = record.codeOfport.Trim(),
                                    TraveltimeInDays = record.sailingTimeInDays
                                };

                                await config.AddAsync(obj);
                                await config.SaveChangesAsync();

                                success += 1;
                                successList.Add(record);
                            }
                            else
                            {
                                failed += 1;
                                errorList.Add(record);
                                errors.Add($"Shipping port '{record.nameOfport}' with code '{record.codeOfport}' already exist in the data store");
                            }
                        }
                    }
                    catch(Exception innerExc)
                    {
                        failed += 1;
                        errors.Add($"error: {innerExc.Message}");
                    }
                }

                return response = new UploadAPIResponse() { 
                    status = true,
                    successCount = success,
                    errorCount = failed,
                    data = successList,
                    errorList = errorList,
                    errorMessageList = errors,
                    message = $"Total records= {payLoad.Count().ToString()}, successful inserts= {success.ToString()}, failed inserts= {failed.ToString()}"
                };
            }
            catch(Exception x)
            {
                return response = new UploadAPIResponse()
                {
                    status = false,
                    message = $"error: {x.Message}",
                    errorList = errorList
                };
            }
        }
    
        public async Task<DefaultAPIResponse> GetCountryShippingPortAsync(CountryLookup payLoad)
        {
            //TODO: gets the shipping ports belonging to a country
            DefaultAPIResponse response = null;

            try
            {
                var objHelper = new Helper();
                var ports_list = await objHelper.getCountryPortsAsync(payLoad.id);

                return response = new DefaultAPIResponse()
                {
                    status = true,
                    message = $"{ports_list.Count()} records fetched",
                    data = ports_list.ToList()
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
    
        public async Task<DefaultAPIResponse> GetShippingPortRecordAsync(SingleParam payLoad)
        {
            //TODO: gets an entire shipping port record from the data store
            DefaultAPIResponse response = null;

            try
            {
                Helper helper = new Helper();
                var portRecord = await helper.getPortAsync(int.Parse(payLoad.stringValue));

                return response = new DefaultAPIResponse() { 
                    status = portRecord != null ? true: false,
                    message = portRecord != null ? $"Record fetched successfully from datastore" : @"No data",
                    data = portRecord
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

        public async Task<PaginationAPIResponse> GetShippingPortPageAsync(int page, int pageSize)
        {
            //TODO: gets shipping port and paginate it
            PaginationAPIResponse response = null;

            try
            {
                Helper helper = new Helper();
                var portRecords = await helper.getPortAsync();

                var totalCount = portRecords.Count();
                var totalPages = (int)Math.Ceiling((decimal)totalCount / pageSize);

                response = new PaginationAPIResponse() {
                    status = totalCount > 0 ? true : false,
                    message = totalCount > 0 ? $"Page {page} out of {totalPages}" : @"failed",
                    data = portRecords.Skip((page - 1) * pageSize).Take(pageSize).ToList(),
                    total = totalCount
                };

                return response;
            }
            catch(Exception x)
            {
                return response = new PaginationAPIResponse()
                {
                    status = false,
                    message = $"error: {x.Message}"
                };
            }
        }

    }
}
