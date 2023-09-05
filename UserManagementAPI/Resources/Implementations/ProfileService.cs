#nullable disable

using UserManagementAPI.Resources.Implementations;
using UserManagementAPI.Resources.Interfaces;
using UserManagementAPI.Data;
using UserManagementAPI.Response;
using UserManagementAPI.POCOs;

namespace UserManagementAPI.Resources.Implementations
{
    public class ProfileService : IProfileService
    {
        swContext config;
        public ProfileService() {
            config = new swContext();
        }

        public async Task<DefaultAPIResponse> SaveProfileAsync(SystemProfile ProfilePayLoad)
        {
            //method creates a profile in the data store
            DefaultAPIResponse response = null;

            try
            {
                var obj = new TProfile()
                {
                    ProfileId = ProfilePayLoad.Id,
                    ProfileString = ProfilePayLoad.profileModules,
                    ProfileName = ProfilePayLoad.nameOfProfile,
                    CompanyId = ProfilePayLoad.companyId,
                    InUse = ProfilePayLoad.inUse,
                    DteAdded = ProfilePayLoad.dateAdded
                };

                await config.TProfiles.AddAsync(obj);
                await config.SaveChangesAsync();

                return response = new DefaultAPIResponse()
                {
                    status = true,
                    message = @"Profile successfully saved",
                    data = (object)ProfilePayLoad
                };
            }
            catch(Exception profileErr)
            {
                response = new DefaultAPIResponse() { status = false, message = $"error: {profileErr.Message} | inner exception: {profileErr.InnerException.Message}" };
                return response;
            }
        }

        public async Task<DefaultAPIResponse> AmendProfileAsync(SystemProfile ProfilePayLoad)
        {
            //method is used to amend system profile
            DefaultAPIResponse rsp = null;

            try
            {
                var obj = await config.TProfiles.Where(x => x.ProfileName == ProfilePayLoad.nameOfProfile).FirstOrDefaultAsync();
                if (obj.ProfileId > 0)
                {
                    obj.ProfileString = ProfilePayLoad.profileModules;

                    await config.SaveChangesAsync();
                    rsp = new DefaultAPIResponse() { status = true, message = @"profile amended successfully",data = (object)ProfilePayLoad };
                }

                return rsp;
            }
            catch(Exception x)
            {
                return rsp = new DefaultAPIResponse() { 
                    status = false,
                    message = $"error: {x.Message} | inner exception: {x.InnerException.Message}"
                };
            }
        }

        public async Task<DefaultAPIResponse> GetProfilesAsync(int _companyId)
        {
            //gets the list of system profiles belonging to a company
            DefaultAPIResponse response = null;
            List<SystemProfile> profiles = new List<SystemProfile>();
            try
            {
                var dta = await config.TProfiles.Where(c => c.CompanyId == _companyId).ToListAsync();

                if (dta.Count() > 0)
                {
                    response = new DefaultAPIResponse();
                    foreach(var d in dta)
                    {
                        var obj = new SystemProfile() { 
                            Id = d.ProfileId,
                            nameOfProfile = d.ProfileName,
                            profileModules = d.ProfileString,
                            companyId= (int) d.CompanyId,
                            inUse = (int) d.InUse
                        };

                        profiles.Add(obj);
                    }

                    response.status = true;
                    response.message = @"success";
                    response.data = profiles.ToList();
                }

                return response;
            }
            catch(Exception x)
            {
                response = new DefaultAPIResponse() { status = false, message = $"{x.Message}" };
                return response;
            }
        }

        public async Task<DefaultAPIResponse> GetProfileModulesAsync(SingleParam payLoad)
        {
            //gets a profile using the profile name
            DefaultAPIResponse response = null;
            List<TModule> d = new List<TModule>();

            try
            {
                var o = await config.TProfiles.Where(p => p.ProfileName == payLoad.stringValue).FirstOrDefaultAsync();
                if (o != null)
                {
                    var module_list = o.ProfileString.Split('|');
                    if (module_list.Count() > 0)
                    {
                        foreach (var item in module_list)
                        {
                            var mObj = await config.TModules.Where(m => m.SysName == item.ToString()).FirstOrDefaultAsync();
                            if (mObj != null)
                            {
                                d.Add(mObj);
                            }
                        }
                    }
                }

                return response = new DefaultAPIResponse() { status = true, message = @"success", data = d };
            }
            catch(Exception err)
            {
                return response = new DefaultAPIResponse() { status = false, message = $"{err.Message}" };
            }
        }
    }
}
