#nullable disable

using UserManagementAPI.POCOs;
using UserManagementAPI.Response;
using UserManagementAPI.Resources.Interfaces;

namespace UserManagementAPI.Resources.Implementations
{
    public class DialCodeService :IDialCodeService
    {
        swContext config;

        public DialCodeService()
        {
            config = new swContext();
        }
        public async Task<DefaultAPIResponse> GetDialCodesAsync()
        {
            DefaultAPIResponse rsp = null;
            List<DialCodeLookup> dialcodes = null;

            try
            {
                var query = (from dc in config.TDialCodes
                             join c in config.TCountryLookups on dc.CountryId equals c.CountryId
                             select new
                             {
                                 Id = dc.Id,
                                 code = dc.Code,
                                 countryName = c.CountryName,
                                 countryId = c.CountryId
                             });

                if (query != null)
                {
                    dialcodes = new List<DialCodeLookup>();
                    foreach(var q in query)
                    {
                        dialcodes.Add(new DialCodeLookup()
                        {
                            id = q.Id,
                            dialCode = q.code,
                            oCountry = new CountryLookup()
                            {
                                nameOfcountry = q.countryName,
                                id = q.countryId
                            }
                        });
                    }

                    rsp = new DefaultAPIResponse()
                    {
                        status = true,
                        message = @"success",
                        data = dialcodes
                    };
                }
                else { rsp = new DefaultAPIResponse() { status = false, message = @"No data" }; }

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
        public async Task<DefaultAPIResponse> CreateDialCodeAsync(DialCodeLookup payLoad)
        {
            //creates dial code resource
            DefaultAPIResponse rsp = null;

            try
            {
                var dt = await config.TDialCodes.Where(dc => dc.Code == payLoad.dialCode).FirstOrDefaultAsync();
                if (dt == null)
                {
                    TDialCode tdc = new TDialCode()
                    {
                        Code = payLoad.dialCode,
                        CountryId = payLoad.oCountry.id
                    };

                    await config.AddAsync(tdc);
                    await config.SaveChangesAsync();

                    rsp = new DefaultAPIResponse() { 
                        status = true,
                        message = $"Dial code '{payLoad.dialCode}' has been added to the data store",
                        data = payLoad
                    };
                }
                else { rsp = new DefaultAPIResponse() { status = false, message = @"No data"}; }
                return rsp;
            }
            catch(Exception x)
            {
                return rsp = new DefaultAPIResponse() { 
                    status = false,
                    message = $"error: {x.Message}"
                };
            }
        }
    }
}
