#nullable disable
using Microsoft.EntityFrameworkCore;
using UserManagementAPI.Resources.Interfaces;
using UserManagementAPI.Response;
using UserManagementAPI.POCOs;
using UserManagementAPI.utils;
using System.Xml.Linq;

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
        public async Task<DefaultAPIResponse> UploadCityDataAsync(IEnumerable<CityLookup> payLoad)
        {
            return new DefaultAPIResponse() { };
        }

        #endregion

    }
}
