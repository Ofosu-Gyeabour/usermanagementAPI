#nullable disable

using UserManagementAPI.Resources.Implementations;
using UserManagementAPI.Resources.Interfaces;
using UserManagementAPI.Response;
using UserManagementAPI.POCOs;
using System.ComponentModel.DataAnnotations;

namespace UserManagementAPI.Resources.Implementations
{
    public class ModuleService :IModuleService
    {
        swContext configuration;

        public ModuleService()
        {
            configuration = new swContext();
        }

        public async Task<DefaultAPIResponse> GetModulesAsync()
        {
            //get all modules in data store
            DefaultAPIResponse response = null;

            try
            {
                var all_modules = await configuration.TModules.ToListAsync();
                if (all_modules != null)
                {
                    response = new DefaultAPIResponse()
                    {
                        status = true,
                        message = @"success",
                        data = all_modules
                    };
                }
                else { response = new DefaultAPIResponse() { status = false, message = @"No data" }; }

                return response;
            }
            catch(Exception e)
            {
                return response = new DefaultAPIResponse()
                {
                    status = false,
                    message = $"error: {e.Message}"
                };
            }
        }

        public async Task<DefaultAPIResponse> GetModulesInUseAsync()
        {
            DefaultAPIResponse response = null;
            int inUseFlag = 1;

            try
            {
                var dta = await configuration.TModules.Where(tm => tm.InUse == inUseFlag).ToListAsync();
                if (dta != null)
                {
                    response = new DefaultAPIResponse()
                    {
                        status = true,
                        message = @"success",
                        data = dta
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
    }
}
