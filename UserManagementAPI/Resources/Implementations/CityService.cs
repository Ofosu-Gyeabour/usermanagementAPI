#nullable disable
using Microsoft.EntityFrameworkCore;
using UserManagementAPI.Resources.Interfaces;
using UserManagementAPI.Response;
using UserManagementAPI.POCOs;
using UserManagementAPI.utils;
using System.Xml.Linq;
using System.Diagnostics;
using UserManagementAPI.Models;

namespace UserManagementAPI.Resources.Implementations
{
    public class CityService
        : ICityService
    {
        swContext config_;
        public CityService()
        {
            config_ = new swContext();
        }

        #region implementations

        public async Task<PaginationAPIResponse> GetCitiesAsync(int page, int pageSize)
        {
            PaginationAPIResponse response = null;

            try
            {
                Helper helper = new Helper();
                var dt = await helper.getActiveCitiesAsync();

                //paginating data
                var totalCount = dt.Count();
                var totalPages = (int)Math.Ceiling((decimal)totalCount / pageSize);

                response = new PaginationAPIResponse() {
                    status = dt.Count() > 0 ? true : false,
                    message = dt.Count() > 0 ? @"success" : @"failed",
                    data = dt.Skip((page - 1) * pageSize)
                            .Take(pageSize)
                            .ToList(),
                    total = totalCount
                };

                return response;
            }
            catch(Exception ex)
            {
                return response = new PaginationAPIResponse()
                {
                    status = false,
                    message = $"error: {ex.Message}"
                };
            }
            


        }

        public async Task<DefaultAPIResponse> GetAllCitiesAsync()
        {
            DefaultAPIResponse rsp = null;
            List<CityLookup> results = null;

            try
            {
                var cities = await config_.TCities.ToListAsync();
                if(cities != null)
                {
                    results = new List<CityLookup>();
                    foreach(var city in cities)
                    {
                        var obj = new CityLookup()
                        {
                            id = city.Id,
                            nameOfcity = city.CityName,
                            oCountry = await new Helper() { }.getCountry((int) city.CountryId)
                        };

                        results.Add(obj);
                    }

                    rsp = new DefaultAPIResponse()
                    {
                        status = true,
                        message = @"success",
                        data = results
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
                    message = $"error: {ex.Message}"
                };
            }
        }
        public async Task<DefaultAPIResponse> CreateCityAsync(CityLookup payLoad)
        {
            //endpoint create a city resource in the data store
            DefaultAPIResponse response;

            try
            {
                var obj = await config_.TCities.Where(x => x.CityName == payLoad.nameOfcity.Trim()).FirstOrDefaultAsync();
                if (obj == null)
                {
                    var country = await config_.TCountryLookups.Where(x => x.CountryName == payLoad.oCountry.nameOfcountry.Trim()).FirstOrDefaultAsync();

                    if (country != null)
                    {
                        TCity city = new TCity()
                        {
                            CityName = payLoad.nameOfcity,
                            CountryId = country.CountryId
                        };

                        await config_.TCities.AddAsync(city);
                        await config_.SaveChangesAsync();

                        response = new DefaultAPIResponse() { status = true, message = $"city {payLoad.nameOfcity} added to data store successfully" };
                    }
                    else
                    {
                        response = new DefaultAPIResponse() { status = false, message = @"error occured: countryID cannot be zero(0)" };
                    }

                    return response;
                }
                else { return response = new DefaultAPIResponse() { status = false, message = $"record with city {payLoad.nameOfcity} already exist in data store" }; }
            }
            catch(Exception ex)
            {
                response = new DefaultAPIResponse() { status = false, message = $"error: {ex.Message}" };
                return response;
            }
        }

        public async Task<DefaultAPIResponse> UpdateCityAsync(CityLookup payLoad)
        {
            DefaultAPIResponse response = new DefaultAPIResponse();

            try
            {
                //gets country object
                var objCity = await config_.TCities.Where(x => x.Id == payLoad.id).FirstOrDefaultAsync();
                if (objCity != null)
                {
                    objCity.CityName = payLoad.nameOfcity;

                    await config_.SaveChangesAsync();
                    return response = new DefaultAPIResponse() { status = true, message = $"record updated with new city name {payLoad.nameOfcity}" };
                }
                else
                {
                    return response = new DefaultAPIResponse() { status = false, message = @"error occured: city Id cannot be zero (0)" };
                }
            }
            catch(Exception exc)
            {
                return response = new DefaultAPIResponse()
                {
                    status = false,
                    message = $"error: {exc.Message}"
                };
            }
        }

        public async Task<DefaultAPIResponse> UpdateCountryOfCityAsync(CityLookup payLoad)
        {
            Helper helper = new Helper();
            DefaultAPIResponse response = null;

            try
            {
                var cityObj = await config_.TCities.Where(x => x.CityName == payLoad.nameOfcity.Trim()).FirstOrDefaultAsync();
                if (cityObj != null)
                {
                    var countryObj = await config_.TCountryLookups.Where(x => x.CountryName == payLoad.oCountry.nameOfcountry.Trim()).FirstOrDefaultAsync();
                    if (countryObj != null)
                    {
                        cityObj.CountryId = countryObj.CountryId;

                        await config_.SaveChangesAsync();

                        response = new DefaultAPIResponse() { 
                            status = true, 
                            message = $"country of city {payLoad.nameOfcity} updated to {payLoad.oCountry.nameOfcountry}" ,
                            data = payLoad
                        };
                    }
                    else
                    {
                        response = new DefaultAPIResponse() { status = false, message = @"error: country cannot have an Id of zero (0)" };
                    }
                }
                else
                {
                    response = new DefaultAPIResponse() { status = false, message = @"error: city cannot have an Id of zero (0)" };
                }

                return response;
            }
            catch(Exception ex)
            {
                return response = new DefaultAPIResponse() { status = false, message = $"error: {ex.Message}" };
            }
        }
        public async Task<UploadAPIResponse> UploadCityDataAsync(IEnumerable<CityLookup> payLoad)
        {
            //saves uploaded data into the data store
            UploadAPIResponse response = null;
            int success = 0;
            int failed = 0;
            List<CityLookup> successList = new List<CityLookup>();
            List<CityLookup> errorList = new List<CityLookup>();
            List<string> errors = new List<string>();

            try
            {
                if (payLoad.Count() > 0)
                {
                    foreach(var city in payLoad)
                    {
                        try
                        {
                            var t = await config_.TCities.Where(t => t.CityName == city.nameOfcity).FirstOrDefaultAsync();
                            if (t == null)
                            {
                                var objCountry = await config_.TCountryLookups.Where(c => c.CountryName == city.oCountry.nameOfcountry.Trim()).FirstOrDefaultAsync();
                                if (objCountry != null)
                                {
                                    TCity objCity = new TCity()
                                    {
                                        CityName = city.nameOfcity,
                                        CountryId = objCountry.CountryId
                                    };

                                    await config_.AddAsync(objCity);
                                    await config_.SaveChangesAsync();

                                    successList.Add(city);
                                    success += 1;
                                }
                                else
                                {
                                    errorList.Add(city);
                                    failed += 1;
                                }
                            }
                            else { 
                                failed += 1;
                                errors.Add($"City with name {city.nameOfcity} already exist in the data store");
                            }
                        }
                        catch(Exception innerExc)
                        {
                            Debug.Print($"error: {innerExc.Message}");
                            errorList.Add(city);
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
                }

                return response;
            }
            catch(Exception exc)
            {
                return response = new UploadAPIResponse()
                {
                    status = false,
                    message = $"error: {exc.Message}",
                    errorMessageList = errors
                };
            }
        }

        public async Task<DefaultAPIResponse> Get(int id)
        {
            DefaultAPIResponse response = null;

            try
            {
                var Query = (from c in config_.TCities
                             join cnt in config_.TCountryLookups on c.CountryId equals cnt.CountryId
                             where cnt.CountryId == id

                             select new
                             {
                                 id = c.Id,
                                 nameOfcity = c.CityName,
                                 countryId = cnt.CountryId,
                                 countryName = cnt.CountryName
                             });

                var cityList = await Query.ToListAsync().ConfigureAwait(false);
                var cities = cityList.Select(c => new CityLookup()
                {
                    id = c.id,
                    nameOfcity = c.nameOfcity,
                    oCountry = new CountryLookup()
                    {
                        id = c.countryId,
                        nameOfcountry = c.countryName
                    }
                }).ToList();

                return response = new DefaultAPIResponse() { 
                    status = true,
                    message = @"success",
                    data = cities
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

        public async Task<DefaultAPIResponse> Get(string country)
        {
            //gets the list of cities for country
            DefaultAPIResponse response = null;
            List<CityLookup> results = null;

            try
            {
                var query = (from c in config_.TCities
                             join cnt in config_.TCountryLookups on c.CountryId equals cnt.CountryId
                             where cnt.CountryName == country

                             select new
                             {
                                 id = c.Id,
                                 city = c.CityName,
                                 IdOfcountry = cnt.CountryId
                             });

                var queryList = await query.ToListAsync().ConfigureAwait(false);

                results = queryList.Select(q => new CityLookup()
                {
                    id = (int)q.id,
                    nameOfcity = q.city,
                    oCountry = new CountryLookup()
                    {
                        id = (int)q.IdOfcountry,
                        nameOfcountry = country
                    }
                }).ToList();

                return response = new DefaultAPIResponse() {
                    status = true,
                    message = @"success",
                    data = results
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

        #endregion

    }
}
