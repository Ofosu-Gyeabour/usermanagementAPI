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
   
        public async Task<UploadAPIResponse> UploadDialCodesAsync(IEnumerable<DialCodeLookup> payLoad)
        {
            UploadAPIResponse response = null;
            int success = 0;
            int failed = 0;
            List<DialCodeLookup> successList = new List<DialCodeLookup>();
            List<DialCodeLookup> errorList = new List<DialCodeLookup>();
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
                            var dialCodeQuery = (from dc in config.TDialCodes
                                                 join c in config.TCountryLookups on dc.CountryId equals c.CountryId
                                                 where dc.Code == record.dialCode.Trim()
                                                 select new
                                                 {
                                                     id = dc.Id,
                                                     code = dc.Code,
                                                     countryName = c.CountryName,
                                                     countryId = c.CountryId
                                                 });

                            if (dialCodeQuery.Count() == 0)
                            {
                                TDialCode tdc = new TDialCode()
                                {
                                    Code = record.dialCode.Trim(),
                                    CountryId = cc.CountryId
                                };

                                await config.AddAsync(tdc);
                                await config.SaveChangesAsync();

                                success += 1;
                                successList.Add(record);
                            }
                            else
                            {
                                failed += 1;
                                errorList.Add(record);
                                errors.Add($"Dial code '{record.dialCode}' for country '{record.oCountry.nameOfcountry}' already exist in data store");
                            }
                        }
                        else
                        {
                            failed += 1;
                            errorList.Add(record);
                            errors.Add($"Country with name '{record.oCountry.nameOfcountry}' does not exist in the data store");
                        }
                    }
                    catch(Exception innerExc)
                    {
                        errorList.Add(record);
                        errors.Add($"Error: {innerExc.Message}");
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
                return response = new UploadAPIResponse() { 
                    status = false,
                    message = $"error: {x.Message}",
                    errorList = errorList
                };
            }
        }

    }
}
