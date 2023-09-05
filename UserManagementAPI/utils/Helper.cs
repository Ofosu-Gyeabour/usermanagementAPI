#nullable disable

namespace UserManagementAPI.utils
{
    public class Helper
    {
        swContext config;
        public Helper() {
            config = new swContext();
        }   

        public async Task<bool> AmendUserModules(string _usr, string _newProfile)
        {
            try
            {    
                try
                {
                    var pObj = await config.TProfiles.Where(p => p.ProfileName == _newProfile).FirstOrDefaultAsync();
                    var u = await config.Tusrs.Where(x => x.Usrname == _usr).FirstOrDefaultAsync();
                    if ((u != null) & (pObj != null))
                    {
                        //amend associated profile id
                        u.ProfileId = pObj.ProfileId;
                    }

                    await config.SaveChangesAsync();

                    return true;
                }
                catch (Exception transErr)
                {
                    throw;
                }   
            }
            catch(Exception ex)
            {
                throw;
            }
        }
    }
}
