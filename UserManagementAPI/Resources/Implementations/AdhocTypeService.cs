#nullable disable

using UserManagementAPI.POCOs;
using UserManagementAPI.Response;
using UserManagementAPI.Resources.Interfaces;
using UserManagementAPI.Data;

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

    }
}
