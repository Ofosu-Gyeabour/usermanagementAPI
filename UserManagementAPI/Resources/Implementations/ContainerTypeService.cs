#nullable disable
using UserManagementAPI.Response;
using UserManagementAPI.Resources.Interfaces;
using UserManagementAPI.POCOs;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using UserManagementAPI.Models;
using UserManagementAPI.utils;
using System.Drawing.Printing;
using UserManagementAPI.Procs;

namespace UserManagementAPI.Resources.Implementations
{
    public class ContainerTypeService : IContainerTypeService
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
                    foreach (var d in dta)
                    {
                        var cnt = new ContainerTypeLookup()
                        {
                            id = d.Id,
                            containerType = d.Ctype,
                            containerVolume = (decimal)d.Cvolume
                        };

                        container_types.Add(cnt);
                    }

                    rsp = new DefaultAPIResponse()
                    {
                        status = true,
                        message = @"success",
                        data = container_types
                    };
                }
                else { rsp = new DefaultAPIResponse() { status = false, message = @"No data" }; }

                return rsp;
            }
            catch (Exception x)
            {
                return rsp = new DefaultAPIResponse()
                {
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

                    rsp = new DefaultAPIResponse()
                    {
                        status = true,
                        message = $"Container Type '{payLoad.containerType}' added to the data store",
                        data = payLoad
                    };
                }
                else { rsp = new DefaultAPIResponse() { status = false, message = $"Container type '{payLoad.containerType}' already exist in the data store" }; }

                return rsp;
            }
            catch (Exception x)
            {
                return rsp = new DefaultAPIResponse()
                {
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
                foreach (var record in payLoad)
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
                            TcontainerType objContainerType = new TcontainerType()
                            {
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
                    catch (Exception innerExc)
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
                    errorList = errorList,
                    data = successList,
                    errorMessageList = errors,
                    message = $"Total records= {payLoad.Count().ToString()}, successful inserts= {success.ToString()}, failed inserts= {failed.ToString()}"
                };
            }
            catch (Exception x)
            {
                return response = new UploadAPIResponse()
                {
                    status = false,
                    message = $"error: {x.Message}",
                    errorList = errorList
                };
            }
        }

        public async Task<DefaultAPIResponse> SaveContainerRecordAsync(clsShippingContainer payLoad)
        {
            DefaultAPIResponse rsp = null;

            try
            {
                clsContainerDocValue cdoc = new clsContainerDocValue();
                Helper helper = new Helper();

                var lookuplist = await helper.getContainerStatsLookupAsync();
                var cdocList = await cdoc.fetchContainerDocLookup();

                bool bln = await payLoad.recordExists();

                bool blnOperation = bln == false ? await payLoad.AddContainerAsync(lookuplist.ToList(),cdocList.ToList()) : await payLoad.UpdateContainer();

                return rsp = new DefaultAPIResponse()
                {
                    status = blnOperation == true ? true : false,
                    message = blnOperation == true ? $"Container with name {payLoad.nameOfcontainer} saved in the datastore" : @"Failed: contact Administrator",
                    data = payLoad
                };
            }
            catch (Exception ex)
            {
                return rsp = new DefaultAPIResponse()
                {
                    status = false,
                    message = $"error: {ex.Message}"
                };
            }
        }

        public async Task<DefaultAPIResponse> GetContainerLoadingStatusAsync()
        {
            //TODO: gets the status for loading containers
            DefaultAPIResponse rsp = null;

            try
            {
                clsShippingContainer obj = new clsShippingContainer();
                var containerStatuses = await obj.fetchContainerStatesAsync();
                int totalCount = containerStatuses.ToList().Count;

                return rsp = new DefaultAPIResponse()
                {
                    status = totalCount > 0 ? true : false,
                    message = totalCount > 0 ? @"success" : @"failed",
                    data = containerStatuses.ToList()
                };
            }
            catch (Exception x)
            {
                return rsp = new DefaultAPIResponse()
                {
                    status = false,
                    message = $"error: {x.Message}"
                };
            }
        }

        public async Task<PaginationAPIResponse> GetContainersToLoadListAsync(int page, int pageSize)
        {
            //TODO: gets the list of containers loaded with the system
            PaginationAPIResponse rsp = null;

            try
            {
                string strParam = @"*";
                dtStore dstore = new dtStore();
                var containerList = await dstore.getContainerListAsync(strParam);

                int totalCount = containerList.ToList().Count;
                int totalPages = (int)Math.Ceiling((decimal)totalCount / pageSize);

                rsp = new PaginationAPIResponse()
                {
                    status = totalCount > 0 ? true : false,
                    message = totalCount > 0 ? @"success" : @"failed",
                    total = totalCount,
                    data = containerList.ToList()
                            .Skip((page - 1) * pageSize)
                            .Take(pageSize)
                            .ToList()
                };

                return rsp;
            }
            catch (Exception x)
            {
                return rsp = new PaginationAPIResponse()
                {
                    status = false,
                    message = $"error: {x.Message}"
                };
            }
        }

        public async Task<PaginationAPIResponse> GetUnloadedContainerItemsListAsync(int page, int pageSize)
        {
            //TODO: gets the list of unloaded shipping order items from the data store

            PaginationAPIResponse rsp = null;

            try
            {
                dtStore dstore = new dtStore();
                var yetToLoadData = await dstore.getUnloadedContainerItemsAsync();

                int totalCount = yetToLoadData.ToList().Count;
                int totalPages = (int)Math.Ceiling((decimal)totalCount / pageSize);

                rsp = new PaginationAPIResponse()
                {
                    status = totalCount > 0 ? true : false,
                    message = totalCount > 0 ? @"success" : @"failed",
                    total = totalCount,
                    data = yetToLoadData.ToList()
                           .Skip((page - 1) * pageSize)
                           .Take(pageSize)
                           .ToList()
                };

                return rsp;
            }
            catch (Exception x)
            {
                return rsp = new PaginationAPIResponse()
                {
                    status = false,
                    message = $"error: {x.Message}"
                };
            }
        }

        public async Task<DefaultAPIResponse> InitiateContainerLoadingAsync(string containerAttribute)
        {
            //TODO: initiates the loading of a container
            //loads all data in the associated tables
            //brings back prior records if the process has started earlier
            DefaultAPIResponse rsp = null;
            recContainerStat recObj = new recContainerStat();

            try
            {
                Helper helper = new Helper();
                dtStore dstore = new dtStore();

                var contObj = await helper.getContainerObject(containerAttribute);
                var contStatistics = await dstore.fetchContainerStatisticsAsync(contObj.Id);

                recObj.cBio = contObj;
                recObj.statList = contStatistics.ToList();

                rsp = new DefaultAPIResponse()
                {
                    status = contObj.Id > 0 ? true : false,
                    message = contObj.Id > 0 ? @"success" : @"failed",
                    data = recObj
                };

                return rsp;
            }
            catch (Exception x)
            {
                return rsp = new DefaultAPIResponse()
                {
                    status = false,
                    message = $"error: {x.Message}"
                };
            }
        }

        public async Task<DefaultAPIResponse> LoadContainerItemAsync(LoadedItem payLoad)
        {
            //TODO: loads the item into the data store
            DefaultAPIResponse rsp = null;

            try
            {
                Helper helper = new Helper();
                clsContainerItem obj = new clsContainerItem();

                var contObj = await helper.getContainerObject(payLoad.containerCode);
                bool bol = await obj.AddContainerItemAsync(contObj.Id, payLoad);

                return rsp = new DefaultAPIResponse()
                {
                    status = bol,
                    message = bol ? @"success" : @"failed",
                    data = payLoad
                };
            }
            catch (Exception x)
            {
                return rsp = new DefaultAPIResponse()
                {
                    status = false,
                    message = $"error: {x.Message}"
                };
            }
        }

        public async Task<DefaultAPIResponse> GetContainerItemsAsync(string containerAttribute)
        {
            //TODO: gets loaded items for a container given the bar code
            DefaultAPIResponse rsp = null;

            try
            {
                Helper helper = new Helper();

                var contObj = await helper.getContainerObject(containerAttribute);
                clsContainerItem obj = new clsContainerItem() { id = contObj.Id };
                var contItems = await obj.getContainerItems();

                rsp = new DefaultAPIResponse()
                {
                    status = contObj != null ? true : false,
                    message = contObj != null ? @"success" : @"failed",
                    data = contItems
                };

                return rsp;
            }
            catch (Exception x)
            {
                return rsp = new DefaultAPIResponse()
                {
                    status = false,
                    message = $"error: {x.Message}"
                };
            }

        }
    
        public async Task<DefaultAPIResponse> GetContainerDocLookup()
        {
            //TODO: gets the containerdoclookup table
            DefaultAPIResponse response = null;

            try
            {
                clsContainerDocValue cdoc = new clsContainerDocValue();
                var cdocValues = await cdoc.fetchContainerDocLookup();

                return response = new DefaultAPIResponse()
                {
                    status = cdocValues.Count() > 0? true: false,
                    message = cdocValues.Count() > 0 ? @"success": @"failed",
                    data = cdocValues
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

        public async Task<DefaultAPIResponse> GetContainerDocLookup(int containerId)
        {
            //TODO: gets container documentation for a specific container
            DefaultAPIResponse response = null;

            try
            {
                dtStore dstore = new dtStore();
                var cdocValues = await dstore.fetchContainerDocumentationAsync(containerId);

                response = new DefaultAPIResponse()
                {
                    status = cdocValues.Count() > 0 ? true : false,
                    message = cdocValues.Count() > 0 ? @"success" : @"failed",
                    data = cdocValues
                };

                return response;
            }
            catch(Exception x)
            {
                return response = new DefaultAPIResponse() { 
                    status = false,
                    message = $"error: {x.Message}"
                };
            }
        }

        public async Task<DefaultAPIResponse> RemoveContainerItemAsync(GenericLookup containerPayLoad)
        {
            //TODO: remove an item added to a container
            DefaultAPIResponse response = null;

            try
            {
                clsContainerItem obj = new clsContainerItem() { 
                    shippingOrderId = containerPayLoad.id,
                    itembcode = containerPayLoad.idValue
                };

                bool blnStatus = await obj.RemoveContainerItemAsync();

                response = new DefaultAPIResponse()
                {
                    status = blnStatus ? true: false,
                    message = blnStatus? @"Item successfully removed from container": @"failed",
                    data = obj
                };

                return response;
            }
            catch(Exception x)
            {
                return response = new DefaultAPIResponse() { 
                    status = false,
                    message = $"error: {x.Message}"
                };
            }
        }

    }
}
