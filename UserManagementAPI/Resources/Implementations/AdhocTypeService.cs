#nullable disable

using UserManagementAPI.POCOs;
using UserManagementAPI.Response;
using UserManagementAPI.Resources.Interfaces;
using UserManagementAPI.Models;

namespace UserManagementAPI.Resources.Implementations
{
    public class AdhocTypeService : IAdhocTypeService
    {
        swContext config;

        public AdhocTypeService()
        {
            config = new swContext();
        }

        public async Task<DefaultAPIResponse> GetAdHocTypesAsync()
        {
            //gets all adhoc types
            DefaultAPIResponse rsp = null;
            List<AdhocTypeLookup> adhocs = null;

            try
            {
                var dta = await config.TAdhocTypes.ToListAsync();
                if (dta != null)
                {
                    adhocs = new List<AdhocTypeLookup>();
                    foreach(var d in dta)
                    {
                        var obj = new AdhocTypeLookup() { 
                            id = d.Id,
                            name = d.AdhocName,
                            nomCode = d.Nomcode
                        };

                        adhocs.Add(obj);
                    }

                    rsp = new DefaultAPIResponse() { 
                        status = true,
                        message = @"success",
                        data = adhocs
                    };
                }
                else { rsp = new DefaultAPIResponse() { status = false, message = @"No data" }; }
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
        public async Task<DefaultAPIResponse> CreateAdhocTypeAsync(AdhocTypeLookup payLoad)
        {
            //creates an adhoc resource
            DefaultAPIResponse rsp = null;

            try
            {
                var dt = await config.TAdhocTypes.Where(t => t.AdhocName == payLoad.name).FirstOrDefaultAsync();
                if (dt == null)
                {
                    TAdhocType adhoc = new TAdhocType()
                    {
                        AdhocName = payLoad.name,
                        Nomcode = payLoad.nomCode
                    };

                    await config.AddAsync(adhoc);
                    await config.SaveChangesAsync();

                    rsp = new DefaultAPIResponse() { 
                        status = true, 
                        message = $"Adhoc Type '{payLoad.name}' added successfully to the data store",
                        data = payLoad
                    };
                }
                else { rsp = new DefaultAPIResponse() { status = false, message = $"Adhoc Type '{payLoad.name}' already exist in the data store" }; }

                return rsp;
            }
            catch(Exception x)
            {
                return rsp = new DefaultAPIResponse()
                {
                    status = false,
                    message = $"error {x.Message}"
                };
            }
        }
        public async Task<UploadAPIResponse> UploadAdhocTypeDataAsync(IEnumerable<AdhocTypeLookup> payLoad)
        {
            UploadAPIResponse response = null;
            int success = 0;
            int failed = 0;
            List<AdhocTypeLookup> successList = new List<AdhocTypeLookup>();
            List<AdhocTypeLookup> errorList = new List<AdhocTypeLookup>();
            List<string> errors = new List<string>();

            try
            {
                foreach(var record in payLoad)
                {
                    try
                    {
                        var adhocQuery = (from ad in config.TAdhocTypes
                                          where ad.AdhocName == record.name &&
                                          ad.Nomcode == record.nomCode
                                          select new
                                          {
                                              id = ad.Id,
                                              adhocName = ad.AdhocName,
                                              adhocCode = ad.Nomcode
                                          });

                        if (adhocQuery.Count() == 0)
                        {
                            TAdhocType objAdHoc = new TAdhocType()
                            {
                                AdhocName = record.name.Trim(),
                                Nomcode = record.nomCode.Trim()
                            };

                            await config.AddAsync(objAdHoc);
                            await config.SaveChangesAsync();

                            success += 1;
                            successList.Add(record);
                        }
                        else
                        {
                            failed += 1;
                            errorList.Add(record);
                            errors.Add($"Adhoc type '{record.name}' with code '{record.nomCode}' already exist in the data store");
                        }
                    }
                    catch(Exception innerExc)
                    {
                        failed += 1;
                        errors.Add($"innerException error: {innerExc.Message}");
                    }
                }

                return response = new UploadAPIResponse()
                {
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
                return response = new UploadAPIResponse()
                {
                    status = false,
                    message = $"error: {x.Message}",
                    errorList = errorList
                };
            }
        }

        public async Task<DefaultAPIResponse> GetAdhoc(SingleParam param)
        {
            DefaultAPIResponse response = null;
            List<AdhocTypeLookup> adhoctypes = null;

            try
            {
                var Q = (from ad in config.TAdhocTypes
                         where ad.AdhocName.StartsWith(param.stringValue)
                         select new
                         {
                             id = ad.Id,
                             adhocName = ad.AdhocName,
                             nomCode = ad.Nomcode
                         });

                var QList = await Q.ToListAsync().ConfigureAwait(false);
                adhoctypes = QList.Select(x => new AdhocTypeLookup()
                {
                    id = x.id,
                    name = x.adhocName,
                    nomCode = x.nomCode.Trim()
                }).ToList();

                return response = new DefaultAPIResponse() { 
                    status = true,
                    message = @"success",
                    data = adhoctypes
                };
            }
            catch(Exception x)
            {
                return response = new DefaultAPIResponse() { 
                    status = false,
                    message = $"error: {x.Message}"
                };
            }
        }

        public async Task<DefaultAPIResponse> ComputeOrderSummary(OrderSummaryParameter param)
        {
            //computes order summary
            DefaultAPIResponse rsp = null;
            IndividualCustomerLookup obj = new IndividualCustomerLookup();

            try
            {
                var orderSummary = await obj.getOrderSummaryAsync(param);
                rsp = new DefaultAPIResponse() { 
                    status = true,
                    message = @"success",
                    data = orderSummary
                };

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
