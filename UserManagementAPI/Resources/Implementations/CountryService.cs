#nullable disable
using Microsoft.EntityFrameworkCore;
using UserManagementAPI.POCOs;
using UserManagementAPI.Response;
using UserManagementAPI.Resources.Interfaces;
using System.Runtime.CompilerServices;
using UserManagementAPI.utils;

namespace UserManagementAPI.Resources.Implementations
{

    public class CountryService : ICountryService
    {
        private swContext config;

        public CountryService()
        {
            config = new swContext();
        }

        public async Task<DefaultAPIResponse> Get()
        {
            DefaultAPIResponse response = null;
            List<CountryLookup> countryList = null;

            try
            {

                var query = (from c in config.TCountryLookups
                                join r in config.TRegionLookups on c.RegionId equals r.RegionId
                                where c.CountryId > 0
                                select new
                                {
                                    id = c.CountryId,
                                    cName = c.CountryName,
                                    cCode = c.CountryCode,
                                    regionId = c.RegionId,
                                    regionName = r.RegionName
                                });

                var cList = await query.ToListAsync().ConfigureAwait(false);
                countryList = cList.Select(x => new CountryLookup()
                {
                    id = x.id,
                    nameOfcountry = x.cName.ToUpper(),
                    codeOfcountry = x.cCode == null ? @"" : x.cCode,
                    oRegion = new RegionLookup()
                    {
                        id = (int)x.regionId,
                        nameOfregion = x.regionName.ToUpper()
                    }
                }).ToList();

                response = new DefaultAPIResponse()
                {
                    status = true,
                    message = @"success",
                    data = countryList
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
        public async Task<DefaultAPIResponse> GetCountriesAsync()
        {
            DefaultAPIResponse rsp = null;
            List<CountryLookup> countries = null;

            try
            {

                var data_ = await config.TCountryLookups.Where(c => c.CountryId > 0).Include(r => r.Region).ToListAsync();
                if (data_ != null)
                {
                    countries = new List<CountryLookup>();

                    foreach (var d in data_)
                    {
                        var obj = new CountryLookup()
                        {
                            id = d.CountryId,
                            nameOfcountry = d.CountryName.ToUpper(),
                            codeOfcountry = d.CountryCode == null ? @"" : d.CountryCode.ToUpper(),
                            oRegion = new RegionLookup()
                            {
                                id = d.Region.RegionId,
                                nameOfregion = d.Region.RegionName.ToUpper()
                            }
                        };

                        countries.Add(obj);
                    }

                    rsp = new DefaultAPIResponse()
                    {
                        status = true,
                        message = @"countries retrieved successfully!!!",
                        data = countries
                    };
                }
                else { rsp = new DefaultAPIResponse() { status = false, message = @"No data" }; }
                     
                return rsp;
            }
            catch(Exception ex)
            {
                return rsp = new DefaultAPIResponse()
                {
                    status = false,
                    message = $"error {ex.Message}"
                };
            }
        }

        public async Task<DefaultAPIResponse> CreateCountryAsync(CountryLookup payLoad)
        {
            DefaultAPIResponse response = null;

            try
            {
                var cRecord = await config.TCountryLookups.Where(c => c.CountryName == payLoad.nameOfcountry.Trim()).FirstOrDefaultAsync();
                if (cRecord == null)
                {
                    var regionObj = await config.TRegionLookups.Where(r => r.RegionName == payLoad.oRegion.nameOfregion.Trim()).FirstOrDefaultAsync();
                    if (regionObj != null)
                    {
                        payLoad.oRegion.id = regionObj.RegionId;

                        TCountryLookup country = new TCountryLookup()
                        {
                            CountryName = payLoad.nameOfcountry.Trim().ToUpper(),
                            CountryCode = payLoad.codeOfcountry.Trim(),
                            RegionId = payLoad.oRegion.id
                        };

                        await config.TCountryLookups.AddAsync(country);
                        await config.SaveChangesAsync();

                        response = new DefaultAPIResponse()
                        {
                            status = true,
                            message = $"country {payLoad.nameOfcountry} added to the datastore successfully",
                            data = payLoad
                        };
                    }
                    else { response = new DefaultAPIResponse() { status = false,message = @"region Id cannot be zero (0)" }; }
                }
                else { response = new DefaultAPIResponse() { status = false, message = $"country {payLoad.nameOfcountry} already exists" }; }

                return response;
            }
            catch(Exception ex)
            {
                return response = new DefaultAPIResponse()
                {
                    status = false,
                    message = $"error: {ex.Message}",
                };
            }
        }

        public async Task<DefaultAPIResponse> UpdateCountryAsync(CountryLookup payLoad)
        {
            //updates a country record
            DefaultAPIResponse response = null;
            string oldCountryName = string.Empty;

            try
            {
                var obj = await config.TCountryLookups.Where(c => c.CountryId == payLoad.id).FirstOrDefaultAsync();
                if (obj != null)
                {
                    oldCountryName = obj.CountryName;
                    obj.CountryName = payLoad.nameOfcountry.Trim().ToUpper();
                    obj.RegionId = payLoad.oRegion.id;

                    await config.SaveChangesAsync();
                    response = new DefaultAPIResponse() { 
                        status = true, 
                        message = $"Country updated from {oldCountryName.ToUpper()} to {payLoad.nameOfcountry.Trim().ToUpper()}" 
                    };
                }
                else { response = new DefaultAPIResponse() {status = false, message = @"error occured: country Id cannot be zero (0)" }; }

                return response;
            }
            catch(Exception exc)
            {
                return response = new DefaultAPIResponse() { 
                    status = false,
                    message = $"error: {exc.Message}"
                };
            }
        }

        public async Task<UploadAPIResponse> UploadCountryAsync(IEnumerable<CountryLookup> payLoad)
        {
            //uploads the content of a file
            UploadAPIResponse response = null;
            int success = 0;
            int failed = 0;
            List<CountryLookup> successList = new List<CountryLookup>();
            List<CountryLookup> errorList = new List<CountryLookup>();
            List<string> errors = new List<string>();

            try
            {
                foreach(var record in payLoad)
                {
                    try
                    {
                        var cObj = await config.TCountryLookups.Where(c => c.CountryName == record.nameOfcountry.Trim()).FirstOrDefaultAsync();
                        if (cObj == null)
                        {
                            //country record exist. check region
                            var rr = await config.TRegionLookups.Where(x => x.RegionName == record.oRegion.nameOfregion.Trim()).FirstOrDefaultAsync();
                            if (rr != null)
                            {
                                //insert record
                                TCountryLookup countryRecord = new TCountryLookup()
                                {
                                    RegionId = rr.RegionId,
                                    CountryName = record.nameOfcountry.Trim(),
                                    CountryCode = record.codeOfcountry.Trim()
                                };

                                await config.AddAsync(countryRecord);
                                await config.SaveChangesAsync();

                                successList.Add(record);
                                success += 1;
                            }
                            else
                            {
                                failed += 1;
                                errorList.Add(record);
                                errors.Add($"Region '{record.oRegion.nameOfregion}' does not exist in the data store for country '{record.nameOfcountry}'");
                            }
                        }
                        else
                        {
                            //country exist...get region and modify it
                            var r = await config.TRegionLookups.Where(o => o.RegionName == record.oRegion.nameOfregion.Trim()).FirstOrDefaultAsync();
                            if (r != null)
                            {
                                cObj.CountryCode = record.codeOfcountry;
                                cObj.RegionId = r.RegionId;

                                await config.SaveChangesAsync();
                            }
                            else
                            {
                                //region does not exist. report as an error
                                failed += 1;
                                errors.Add($"Region '{record.oRegion.nameOfregion}' does not exist in the data store for country '{record.nameOfcountry}'");
                            }
                        }
                    }
                    catch (Exception innerE)
                    {
                        errorList.Add(record);
                        failed += 1;
                    }                   
                }

                response = new UploadAPIResponse()
                {
                    status = true,
                    message = $"Total records= {payLoad.Count().ToString()}, successful inserts= {success.ToString()}, failed inserts= {failed.ToString()}",
                    data = successList,
                    successCount = success,
                    errorList = errorList,
                    errorMessageList = errors,
                    errorCount = failed
                };

                return response;
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

        public async Task<DefaultAPIResponse> GetCountryPrefixAsync()
        {
            //TODO: gets the prefix of all countries in the data store
            DefaultAPIResponse response = null;

            try
            {
                Helper helper = new Helper();
                var countryPrefixDta = await helper.getAllCountryPrefixListAsync();

                return response = new DefaultAPIResponse()
                {
                    status = countryPrefixDta.Count() > 0 ? true: false,
                    message = countryPrefixDta.Count() > 0 ? $"{countryPrefixDta.Count()} records fetched from datastore": @"No data found",
                    data = countryPrefixDta
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
