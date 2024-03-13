#nullable disable

using UserManagementAPI.POCOs;
using UserManagementAPI.Response;
using UserManagementAPI.Resources.Interfaces;
using System.Runtime.InteropServices;
using Newtonsoft.Json;

using UserManagementAPI.utils;
using System.Linq.Expressions;
using UserManagementAPI.Models;

namespace UserManagementAPI.Resources.Implementations
{
    public class TitleService : ITitleService
    {
        swContext config;
        public TitleService()
        {
            config = new swContext();
        }
        public async Task<DefaultAPIResponse> GetTitlesAsync()
        {
            DefaultAPIResponse response = null;
            List<TitleLookup> titles = null;

            try
            {
                var dta = await config.TTitles.ToListAsync();
                if (dta != null)
                {
                    titles = new List<TitleLookup>();
                    foreach(var d in dta)
                    {
                        var obj = new TitleLookup() { 
                            id = d.Id,
                            nameOftitle = d.Title
                        };

                        titles.Add(obj);
                    }

                    response = new DefaultAPIResponse() { 
                        status = true,
                        message = @"success",
                        data = titles
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
        public async Task<PaginationAPIResponse> ListTitlesAsync(int page, int pageSize)
        {
            PaginationAPIResponse response = null;

            try
            {
                Helper helper = new Helper();
                var titleList = await helper.GetTitlesAsync();

                var totalCount = titleList.Count();
                var totalPages = (int)Math.Ceiling((decimal)totalCount / pageSize);


                return response = new PaginationAPIResponse() { 
                    status = titleList.Count() > 0 ? true: false,
                    message = titleList.Count() > 0 ? @"success": @"failed",
                    data = titleList.Skip((page - 1) * pageSize)
                            .Take(pageSize)
                            .ToList(),
                    total = totalCount
                };
            }
            catch(Exception x)
            {
                return response = new PaginationAPIResponse() { 
                    status = false,
                    message = $"error: {x.Message}"
                };
            }
        }
        public async Task<DefaultAPIResponse> CreateTitleAsync(TitleLookup payLoad)
        {
            DefaultAPIResponse response = null;

            try
            {
                var dt = await config.TTitles.Where(t => t.Title == payLoad.nameOftitle).FirstOrDefaultAsync();
                if (dt == null)
                {
                    TTitle objTitle = new TTitle()
                    {
                        Title = payLoad.nameOftitle
                    };

                    await config.AddAsync(objTitle);
                    await config.SaveChangesAsync();

                    response = new DefaultAPIResponse()
                    {
                        status = true,
                        message = $"Title {payLoad.nameOftitle} added to data store",
                        data = payLoad
                    };

                    var helper = new Helper();
                    var evObj = await helper.getEventLookupAsync(EventConfig.ADD_RECORD_OPERATION);

                    //logger
                    var objLogger = new Log()
                    {
                        eventId = evObj.Id,
                        actor = @"nappiah",
                        entity = @"TITLE",
                        entityValue = JsonConvert.SerializeObject(objTitle),
                        companyId = 1,
                        logDate = DateTime.Now
                    };

                    await helper.WriteLogAsync(objLogger);
                }
                else { response = new DefaultAPIResponse() { status = false, message = $"Title '{payLoad.nameOftitle}' already exist in the data store"}; }

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

        public async Task<UploadAPIResponse> UploadTitleAsync(IEnumerable<TitleLookup> payLoad)
        {
            UploadAPIResponse response = null;
            int success = 0;
            int failed = 0;
            List<TitleLookup> successList = new List<TitleLookup>();
            List<TitleLookup> errorList = new List<TitleLookup>();
            List<string> errors = new List<string>();
            TTitle dta = null;

            try
            {
                foreach(var record in payLoad)
                {
                    try
                    {
                        using (var cf = new swContext())
                        {
                            dta = await config.TTitles.Where(rec => rec.Title == record.nameOftitle.Trim()).FirstOrDefaultAsync();
                        }
                            
                        if (dta == null)
                        {
                            TTitle objTitle = new TTitle()
                            {
                                Title = record.nameOftitle.Trim()
                            };

                            await config.AddAsync(objTitle);
                            await config.SaveChangesAsync();

                            success += 1;
                            successList.Add(record);
                        }
                        else
                        {
                            failed += 1;
                            errorList.Add(record);
                            errors.Add($"Title '{record.nameOftitle}' already exist in the data store");
                        }
                    }
                    catch(Exception innerExc)
                    {
                        failed += 1;
                        errorList.Add(record);
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

                return response;
            }
            catch(Exception x)
            {
                return response = new UploadAPIResponse()
                {
                    status = false,
                    message = $"error: {x.Message}"
                };
            }
        }
    }
}
