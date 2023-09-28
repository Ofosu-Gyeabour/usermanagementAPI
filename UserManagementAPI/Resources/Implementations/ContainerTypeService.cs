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


    }
}
