#nullable disable

using UserManagementAPI.POCOs;
using UserManagementAPI.Response;
using UserManagementAPI.Resources.Interfaces;
using UserManagementAPI.utils;
using UserManagementAPI.Models;

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
                Helper helper = new Helper();
                var dc = await helper.getDialCodesAsync();
                dialcodes = dc.ToList();

                return rsp = new DefaultAPIResponse() { 
                    status = dialcodes.Count() > 0 ? true: false,
                    message = dialcodes.Count() > 0 ? @"success": @"failed",
                    data = dialcodes
                };
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
        
        public async Task<PaginationAPIResponse> GetDialCodesListAsync(int page, int pageSize)
        {
            PaginationAPIResponse rsp = null;
            List<DialCodeRecord> dcRecord = null;

            try
            {
                Helper helper = new Helper();
                var dc = await helper.getDialCodesAsync();

                dcRecord = dc.
                             Select(a => new DialCodeRecord()
                             {
                                 id = a.id,
                                 dialCode = a.dialCode.Trim(),
                                 nameOfcountry = a.oCountry.nameOfcountry,
                                 countryId = a.oCountry.id
                             }).ToList();

                var totalCount = dcRecord.Count();
                var totalPages = (int) Math.Ceiling((decimal) totalCount / pageSize);

                rsp = new PaginationAPIResponse()
                {
                    status = dcRecord.Count() > 0 ? true : false,
                    message = dcRecord.Count() > 0 ? $"Page {page} out of {totalPages}" : @"An error occured. Please see the Administrator",
                    data = dcRecord.Skip((page - 1) * pageSize).Take(pageSize).ToList(),
                    total = totalCount
                };

                return rsp;
            }
            catch(Exception x)
            {
                return rsp = new PaginationAPIResponse() { 
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
