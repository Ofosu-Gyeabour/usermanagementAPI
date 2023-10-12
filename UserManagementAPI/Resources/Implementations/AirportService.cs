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

        public async Task<UploadAPIResponse> UploadAirportDataAsync(IEnumerable<AirportLookup> payLoad)
        {
            UploadAPIResponse response = null;
            int success = 0;
            int failed = 0;
            List<AirportLookup> successList = new List<AirportLookup>();
            List<AirportLookup> errorList = new List<AirportLookup>();
            List<string> errors = new List<string>();

            try
            {
                foreach(var record in payLoad)
                {
                    try
                    {
                        using (var cfg = new swContext())
                        {
                            var cc = await cfg.TCountryLookups.Where(c => c.CountryName == record.oCountry.nameOfcountry.Trim()).FirstOrDefaultAsync();
                            if (cc != null)
                            {
                                var result = (from ap in config.TAirports
                                              join c in config.TCountryLookups on ap.CountryId equals c.CountryId
                                              where c.CountryName == record.oCountry.nameOfcountry.Trim()
                                              select new
                                              {
                                                  Id = ap.Id,
                                                  airport = ap.Airport,
                                                  countryName = c.CountryName,
                                                  countryId = c.CountryId,
                                                  airportCode = ap.Mnemonic
                                              });

                                if (result.Count() == 0)
                                {
                                    //create airport resource
                                    TAirport objAirport = new TAirport()
                                    {
                                        Airport = record.nameOfairport,
                                        CountryId = cc.CountryId,
                                        Mnemonic = record.airportMnemonic
                                    };

                                    await config.AddAsync(objAirport);
                                    await config.SaveChangesAsync();

                                    success += 1;
                                    successList.Add(record);
                                }
                                else
                                {
                                    failed += 1;
                                    errorList.Add(record);
                                    errors.Add($"Airport with name '{record.nameOfairport}' already exist for country '{record.oCountry.nameOfcountry.Trim()}'");
                                }
                            }
                            else
                            {
                                failed += 1;
                                errorList.Add(record);
                                errors.Add($"Country '{record.oCountry.nameOfcountry}' does not exist in the data store");
                            }
                        }
                    }
                    catch(Exception innerEx)
                    {
                        failed += 1;
                        errorList.Add(record);
                        errors.Add($"error:{innerEx.Message}");
                    }
                }

                return response = new UploadAPIResponse()
                {
                    status = true,
                    errorList = errorList,
                    errorCount = failed,
                    successCount = success,
                    data = successList,
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

    }
}
