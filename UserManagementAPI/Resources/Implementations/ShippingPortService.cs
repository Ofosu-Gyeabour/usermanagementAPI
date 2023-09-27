#nullable disable

using UserManagementAPI.POCOs;
using UserManagementAPI.Response;
using UserManagementAPI.Resources.Interfaces;
using System.Diagnostics;

namespace UserManagementAPI.Resources.Implementations
{
    public class ShippingPortService : IShippingPortService
    {
        swContext config;
        public ShippingPortService()
        {
            config = new swContext();
        }

        public async Task<DefaultAPIResponse> GetShippingPortsAsync()
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
                Tshippingport objShippingPort = new Tshippingport()
                {
                    NameOfport = payLoad.nameOfport,
                    CountryId = payLoad.oCountry.id,
                    Portcode = payLoad.codeOfport,
                    TraveltimeInDays = payLoad.sailingTimeInDays
                };

                await config.AddAsync(objShippingPort);
                await config.SaveChangesAsync();

                return response = new DefaultAPIResponse()
                {
                    status = true,
                    message = @"success",
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
