#nullable disable
using Microsoft.EntityFrameworkCore;
using UserManagementAPI.Resources.Interfaces;
using UserManagementAPI.Response;
using UserManagementAPI.POCOs;
using UserManagementAPI.utils;
using System.Xml.Linq;
using System.Diagnostics;

namespace UserManagementAPI.Resources.Implementations
{
    public class RegionService :IRegionService
    {
        swContext config;

        public RegionService()
        {
            config = new swContext();    
        }
        public async Task<DefaultAPIResponse> GetRegionAsync()
        {
            //gets the region data from data store
            DefaultAPIResponse response = null;
            List<RegionLookup> results = null;

            try
            {
                var regionList = await config.TRegionLookups.ToListAsync();
                if (regionList != null)
                {
                    results = new List<RegionLookup>();
                    foreach(var region in regionList)
                    {
                        var obj = new RegionLookup()
                        {
                            id = region.RegionId,
                            nameOfregion = region.RegionName
                        };
                        results.Add(obj);
                    }

                    response = new DefaultAPIResponse()
                    {
                        status = true,
                        message = @"success",
                        data = results
                    };

                }
                else { response = new DefaultAPIResponse() { status = false, message = @"No data" }; }

                return response;
            }
            catch(Exception ex)
            {
                return response = new DefaultAPIResponse()
                {
                    status = false,
                    message = $"error: {ex.Message}"
                };
            }
        }
        public async Task<DefaultAPIResponse> CreateRegionAsync(RegionLookup payLoad)
        {
            //create a region resource in the data store
            DefaultAPIResponse response = null;

            try
            {
                var obj = await config.TRegionLookups.Where(r => r.RegionName == payLoad.nameOfregion).FirstOrDefaultAsync();
                if (obj == null)
                {
                    TRegionLookup objRegion = new TRegionLookup()
                    {
                        RegionName = payLoad.nameOfregion
                    };

                    await config.AddAsync(objRegion);
                    await config.SaveChangesAsync();

                    response = new DefaultAPIResponse()
                    {
                        status = true,
                        message = $"Region with name {payLoad.nameOfregion} added to the data store",
                        data = payLoad
                    };
                }
                else { response = new DefaultAPIResponse() { status = false, message = $"Region with name {payLoad.nameOfregion} already exist in the data store" }; }

                return response;
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

    }
}
