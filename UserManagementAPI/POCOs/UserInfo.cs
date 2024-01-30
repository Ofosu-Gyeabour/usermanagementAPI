namespace UserManagementAPI.POCOs
{
    public class UserInfo
    {
        public int Id { get; set; }
        public string username { get; set; } = string.Empty;
        public string password { get; set; } = string.Empty;

        public async Task<bool> logInvalidLogingAttemptAsync()
        {
            //TODO: finds user who tried logging in
            bool bln = false;
            try
            {
                var config = new swContext();
                using (config)
                {
                    try
                    {
                        var _obj = await config.Tusrs.Where(u => u.Usrname == username).FirstOrDefaultAsync();
                        if (_obj != null)
                        {
                            _obj.Invalidattempt++;

                            await config.SaveChangesAsync();
                            bln = true;
                        }
                    }
                    catch(Exception qEr)
                    {
                        bln = false;
                    }
                }

                return bln;
            }
            catch(Exception x)
            {
                return bln = false;
            }
        }

        public async Task<bool> clearInvalidLoginAttemptsAsync()
        {
            //TODO: clears all record of invalid login attempts for the specific user
            bool bn = false;

            try
            {
                var config = new swContext();
                using (config)
                {
                    try
                    {
                        var _obj = await config.Tusrs.Where(u => u.Usrname == username).FirstOrDefaultAsync();
                        _obj.Invalidattempt = 0;

                        await config.SaveChangesAsync();
                        bn = true;
                    }
                    catch(Exception configEr)
                    {
                        bn = false;
                    }
                }

                return bn;
            }
            catch(Exception x)
            {
                return bn;
            }
        }

        
    }

    
}
