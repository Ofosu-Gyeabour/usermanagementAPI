#nullable disable
using Microsoft.EntityFrameworkCore;
using UserManagementAPI.POCOs;
using UserManagementAPI.Response;
using UserManagementAPI.Resources.Interfaces;
using System.Runtime.CompilerServices;

namespace UserManagementAPI.Resources.Implementations
{
   
    public class CountryService : ICountryService
    {
        swContext config;

        public CountryService()
        {
            config = new swContext();
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

    }
}
