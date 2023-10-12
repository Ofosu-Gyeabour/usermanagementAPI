#nullable disable
using UserManagementAPI.Response;
using UserManagementAPI.Resources.Interfaces;
using UserManagementAPI.POCOs;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;

namespace UserManagementAPI.Resources.Implementations
{
    public class ContainerTypeService: IContainerTypeService
    {
        swContext config;
        public ContainerTypeService()
        {
            config = new swContext();
        }

        public async Task<DefaultAPIResponse> GetContainerTypesAsync()
        {
            DefaultAPIResponse rsp = null;
            List<ContainerTypeLookup> container_types = null;

            try
            {
                var dta = await config.TcontainerTypes.ToListAsync();
                if (dta != null)
                {
                    container_types = new List<ContainerTypeLookup>();
                    foreach(var d in dta)
                    {
                        var cnt = new ContainerTypeLookup()
                        {
                            id = d.Id,
                            containerType = d.Ctype,
                            containerVolume = (decimal)d.Cvolume
                        };

                        container_types.Add(cnt);
                    }

                    rsp = new DefaultAPIResponse() { 
                        status = true,
                        message = @"success",
                        data = container_types
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
        public async Task<DefaultAPIResponse> CreateContainerTypeAsync(ContainerTypeLookup payLoad)
        {
            DefaultAPIResponse rsp = null;

            try
            {
                var item = await config.TcontainerTypes.Where(tc => tc.Ctype == payLoad.containerType).FirstOrDefaultAsync();
                if (item == null)
                {
                    TcontainerType tct = new TcontainerType()
                    {
                        Ctype = payLoad.containerType,
                        Cvolume = payLoad.containerVolume
                    };

                    await config.AddAsync(tct);
                    await config.SaveChangesAsync();

                    rsp = new DefaultAPIResponse() { 
                        status = true,
                        message = $"Container Type '{payLoad.containerType}' added to the data store",
                        data = payLoad
                    };
                }
                else { rsp = new DefaultAPIResponse() { status = false, message = $"Container type '{payLoad.containerType}' already exist in the data store" }; }

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

        public async Task<UploadAPIResponse> UploadContainerTypeDataAsync(IEnumerable<ContainerTypeLookup> payLoad)
        {
            UploadAPIResponse response = null;
            int success = 0;
            int failed = 0;
            List<ContainerTypeLookup> successList = new List<ContainerTypeLookup>();
            List<ContainerTypeLookup> errorList = new List<ContainerTypeLookup>();
            List<string> errors = new List<string>();
            
            try
            {
                foreach(var record in payLoad)
                {
                    try
                    {
                        var query = (from ct in config.TcontainerTypes
                                     where ct.Ctype == record.containerType &&
                                     ct.Cvolume == record.containerVolume
                                     select new
                                     {
                                         id = ct.Id,
                                         ctype = ct.Ctype,
                                         volume = ct.Cvolume
                                     });

                        if (query.Count() == 0)
                        {
                            TcontainerType objContainerType = new TcontainerType() { 
                                Ctype = record.containerType.Trim(),
                                Cvolume = record.containerVolume
                            };

                            await config.AddAsync(objContainerType);
                            await config.SaveChangesAsync();

                            success += 1;
                            successList.Add(record);
                        }
                        else
                        {
                            failed += 1;
                            errorList.Add(record);
                            errors.Add($"Container type '{record.containerType}' with volume '{record.containerVolume}' already exist in the data store");
                        }
                    }
                    catch(Exception innerExc)
                    {
                        failed += 1;
                        errors.Add($"innerException error: {innerExc.Message}");
                    }
                }

                return response = new UploadAPIResponse() { 
                    status = true,
                    successCount = success,
                    errorCount = failed,
                    errorList = errorList,
                    data = successList,
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
