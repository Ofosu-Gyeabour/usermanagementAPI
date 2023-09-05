﻿#nullable disable

using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;
using UserManagementAPI.POCOs;
using UserManagementAPI.Resources.Interfaces;
using UserManagementAPI.Response;
using UserManagementAPI.utils;

namespace UserManagementAPI.Resources.Implementations
{
    public class UserService :IUserService
    {
        swContext config;
        Helper helper;

        public UserService()
        {
            config = new swContext();
            helper = new Helper();
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
    
        public async Task<UserAPIResponse> GetUserAsync(UserInfo userCredential)
        {
            UserAPIResponse response = null;

            try
            {
                var user_info = await config.Tusrs.Where(x => x.Usrname == userCredential.username).Where(x=>x.Usrpassword == userCredential.password)
                    .Include(m => m.Profile)
                    .Include(m=>m.Company)
                    .Include(m=>m.Department)
                    .FirstOrDefaultAsync();

                if (user_info.UsrId > 0)
                {
                        response = new UserAPIResponse()
                        {
                            status = true,
                            message = @"success",
                            user = new User()
                            {
                                id = user_info.UsrId,
                                surname = user_info.Surname,
                                firstname = user_info.Firstname,
                                othernames = user_info.Othernames,
                                usrname = user_info.Usrname,
                                usrpassword = user_info.Usrpassword,
                                isAdmin = user_info.IsAdmin,
                                isLogged = user_info.IsLogged,
                                isActive = user_info.IsActive
                            },
                            company = new Company()
                            {
                                id = user_info.Company.CompanyId,
                                company = user_info.Company.Company,
                                companyAddress = user_info.Company.CompanyAddress,
                                incorporationDate = user_info.Company.IncorporationDate
                            },
                            profile = new Profile()
                            {
                                id = user_info.Profile.ProfileId,
                                profileString = user_info.Profile.ProfileString,
                                inUse = user_info.Profile.InUse,
                                dateAdded = user_info.Profile.DteAdded
                            },
                            department = new Department()
                            {
                                id = user_info.Department.Id,
                                departmentName = user_info.Department.DepartmentName,
                                departmentDescription = user_info.Department.Describ
                            }
                        };

                        return response;
                }
                else
                {
                    return new UserAPIResponse() { status =false,message = @"No data found" };
                }
            }
            catch(Exception x)
            {
                return new UserAPIResponse() { 
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

        public async Task<DefaultAPIResponse> SetLoggedFlagAsync(UserInfo _usr)
        {
            //method logs the user in
            DefaultAPIResponse apiResponse = null;

            try
            {
                var user_record = await config.Tusrs.Where(x => x.Usrname == _usr.username).Where(x => x.Usrpassword == _usr.password).FirstOrDefaultAsync();

                if (user_record.UsrId > 0)
                {
                    user_record.IsLogged = 1;
                    await config.SaveChangesAsync();

                    apiResponse = new DefaultAPIResponse() { status = true, message= $"{_usr.username} successfully logged in" };
                }
                else { apiResponse = new DefaultAPIResponse() { status = false, message = @"No data exists for user" }; }

                return apiResponse;
            }
            catch(Exception x)
            {
                apiResponse = new DefaultAPIResponse() {
                    status = false,
                    message = $"error: {x.Message} || inner exception: {x.InnerException.Message}"
                };

                return apiResponse;
            }
        }

        public async Task<DefaultAPIResponse> SetLoggedOutFlagAsync(UserInfo _usr)
        {
            DefaultAPIResponse apiResponse = null;

            try
            {
                var user_record = await config.Tusrs.Where(x => x.Usrname == _usr.username).Where(x => x.Usrpassword == _usr.password).FirstOrDefaultAsync();

                if (user_record.UsrId > 0)
                {
                    user_record.IsLogged = 0;
                    await config.SaveChangesAsync();

                    apiResponse = new DefaultAPIResponse() { status = true, message = $"{_usr.username} successfully logged out" };
                }
                else { apiResponse = new DefaultAPIResponse() { status = false, message = @"No data exists for user" }; }

                return apiResponse;
            }
            catch (Exception x)
            {
                apiResponse = new DefaultAPIResponse()
                {
                    status = false,
                    message = $"error: {x.Message} || inner exception: {x.InnerException.Message}"
                };

                return apiResponse;
            }
        }

        public async Task<DefaultAPIResponse> GetUserProfileAsync(UserInfo _usr)
        {
            //gets user profile given user name
            DefaultAPIResponse response = null;

            try
            {
                var t = await config.Tusrs.Where(x => x.Usrname == _usr.username)
                                .Include(p => p.Profile).FirstOrDefaultAsync();

                if (t != null)
                {
                    SystemProfile sys = new SystemProfile()
                    {
                        Id =t.Profile.ProfileId,
                        nameOfProfile = t.Profile.ProfileName,
                        profileModules = t.Profile.ProfileString,
                        inUse = (int) t.Profile.InUse,
                        dateAdded = (DateTime) t.Profile.DteAdded
                    };

                    response = new DefaultAPIResponse()
                    {
                        status = true,
                        message = @"success",
                        data = sys
                    };
                    
                }

                return response;
            }
            catch(Exception err)
            {
                return response = new DefaultAPIResponse()
                {
                    status = false,
                    message = $"{err.Message}"
                };
            }
        }

        public async Task<DefaultAPIResponse> AmendUserProfileAsync(UserProfile payLoad)
        {
            //method amends the profile of a user and changes the modules he/she has access to
            DefaultAPIResponse apiResponse = null;

            try
            {
                var amendment_status = await helper.AmendUserModules(payLoad.username, payLoad.profile.nameOfProfile);
                return apiResponse = new DefaultAPIResponse() { status = amendment_status, message = $"{payLoad.username}'s profile amended successfully" };
            }
            catch(Exception x)
            {
                return apiResponse = new DefaultAPIResponse() { status = false, message = $"{x.Message}" };
            }
        }

        public async Task<DefaultAPIResponse> CreateUserAsync(userRecord payLoad)
        {
            DefaultAPIResponse response = null;
            try
            {
                var obj = new Tusr()
                {
                    Surname = payLoad.sname,
                    Firstname = payLoad.fname,
                    Othernames = payLoad.othernames,
                    Usrname = payLoad.userCredentials.username,
                    Usrpassword = payLoad.userCredentials.password,
                    CompanyId = payLoad.companyId,
                    DepartmentId = payLoad.departmentid,
                    IsAdmin = payLoad.isAdministrator,
                    IsLogged = payLoad.isLogged,
                    IsActive = payLoad.isActive,
                    ProfileId = payLoad.profileid
                };

                await config.AddAsync(obj);
                await config.SaveChangesAsync();

                return response = new DefaultAPIResponse() { status = true, message = $"User {obj.Usrname} created successfully!!!", data = obj };
            }
            catch(Exception e)
            {
                return response = new DefaultAPIResponse() { status = false, message = $"{e.Message} | inner exception: {e.InnerException.Message}" };
            }
        }
    }
}
