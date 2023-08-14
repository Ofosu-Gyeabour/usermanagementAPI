using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;
using UserManagementAPI.POCOs;
using UserManagementAPI.Resources.Interfaces;
using UserManagementAPI.Response;

namespace UserManagementAPI.Resources.Implementations
{
    public class UserService :IUserService
    {
        swContext config;

        public UserService()
        {
            config = new swContext();
        }

        public async Task<DefaultAPIResponse> GetUsersAsync()
        {
            //gets the list of users in the datastore
            try
            {
                var userList = await config.Tusrs.ToListAsync();
                return new DefaultAPIResponse() { 
                    status = true,
                    message = @"Success",
                    data = (object)userList
                };
            }
            catch(Exception x)
            {
                return new DefaultAPIResponse()
                {
                    status = false,
                    message = $"{x.Message}"
                };
            }
        }
    
        public async Task<DefaultAPIResponse> GetUserAsync(UserInfo userCredential)
        {
            

            try
            {
                var user_info = await config.Tusrs.Where(x => x.Usrname == userCredential.username).FirstOrDefaultAsync();
                if (user_info != null)
                {
                    var encrypted = await GetMD5EncryptedPasswordAsync(new SingleParam() { stringValue = userCredential.password});

                    if (user_info.Usrpassword == encrypted.data.ToString())
                    {
                        return new DefaultAPIResponse()
                        {
                            status = true,
                            message = @"success",
                            data = (object)user_info
                        };
                    }
                    else
                    {
                        return new DefaultAPIResponse() { status = false, message = @"Incorrect password" };
                    }
                }
                else
                {
                    return new DefaultAPIResponse() { status =false,message = @"No data found" };
                }
            }
            catch(Exception x)
            {
                return new DefaultAPIResponse() { 
                    status = false,
                    message = $"{x.Message}"
                };
            }
        }

        public async Task<DefaultAPIResponse> GetMD5EncryptedPasswordAsync(SingleParam singleParam)
        {
            DefaultAPIResponse results = null;

            try
            {
                var encryptionType = MD5.Create();
                byte[] data = encryptionType.ComputeHash(Encoding.UTF8.GetBytes(singleParam.stringValue));
                var encryptedString = string.Empty;
                for (int i = 0; i < data.Length; i++)
                {
                    encryptedString += data[i].ToString("x2").ToUpperInvariant();
                }

                //singleParam.paramResponse = encryptedString;
                results = new DefaultAPIResponse()
                {
                    status = true,
                    message = @"Success",
                    data = (object)encryptedString
                };

                return results;
            }
            catch(Exception x)
            {
                return new DefaultAPIResponse() { status = false, message = $"{x.Message}" };
            }
        }



    }
}
