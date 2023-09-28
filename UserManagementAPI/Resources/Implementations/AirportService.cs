#nullable disable

using UserManagementAPI.POCOs;
using UserManagementAPI.Response;
using UserManagementAPI.Resources.Interfaces;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;

namespace UserManagementAPI.Resources.Implementations
{
    public class AirportService : IAirportService
    {
        swContext config;

        public AirportService()
        {
            config = new swContext();
        }

        public async Task<DefaultAPIResponse> GetAirportsAsync()
        {
            DefaultAPIResponse response = null;
            List<AirportLookup> airports = null;

            try
            {
                var query = (from a in config.TAirports
                             join c in config.TCountryLookups on a.CountryId equals c.CountryId
                             select new
                             {
                                 Id = a.Id,
                                 airport = a.Airport,
                                 airportCode = a.Mnemonic,
                                 countryName = c.CountryName,
                                 countryId = c.CountryId
                             });

                if (query != null)
                {
                    airports = new List<AirportLookup>();
                    foreach(var q in query)
                    {
                        var air = new AirportLookup()
                        {
                            id = q.Id,
                            nameOfairport = q.airport,
                            airportMnemonic = q.airportCode,
                            oCountry = new CountryLookup()
                            {
                                id = q.countryId,
                                nameOfcountry = q.countryName
                            }
                        };

                        airports.Add(air);
                    }

                    response = new DefaultAPIResponse()
                    {
                        status = true,
                        message = @"success",
                        data = airports
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

        public async Task<DefaultAPIResponse> CreateAirportAsync(AirportLookup payLoad)
        {
            DefaultAPIResponse rsp = null;

            try
            {
                var dt = await config.TAirports.Where(a => a.Airport == payLoad.nameOfairport).FirstOrDefaultAsync();
                if (dt == null)
                {
                    TAirport objAirport = new TAirport()
                    {
                        Airport = payLoad.nameOfairport,
                        Mnemonic = payLoad.airportMnemonic,
                        CountryId = payLoad.oCountry.id
                    };

                    await config.AddAsync(objAirport);
                    await config.SaveChangesAsync();

                    rsp = new DefaultAPIResponse()
                    {
                        status = true,
                        message = $"Airport with name '{payLoad.nameOfairport}' added to the data store"
                    };
                }
                else { rsp = new DefaultAPIResponse() { status = false, message = $"Airport with name '{payLoad.nameOfairport}' already exist in the datastore" }; }

                return rsp;
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
