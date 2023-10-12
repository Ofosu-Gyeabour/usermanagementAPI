#nullable disable

using UserManagementAPI.Resources.Interfaces;
using UserManagementAPI.POCOs;
using UserManagementAPI.Response;
using System.Diagnostics;
using System.ComponentModel.Design;

namespace UserManagementAPI.Resources.Implementations
{
    public class ReferralService :IReferralService
    {
        swContext configure;

        public ReferralService()
        {
            configure = new swContext();
        }

        #region implementations

        public async Task<DefaultAPIResponse> getReferralsAsync()
        {
            //gets list of referrals in the data store
            DefaultAPIResponse response = null;
            List<ReferralLookup> referral_list = null;

            try
            {
                var referral_data = await configure.Tclientreferralsources.ToListAsync();
                if (referral_data != null)
                {
                    referral_list = new List<ReferralLookup>();
                    foreach(var rd in referral_data)
                    {
                        var obj = new ReferralLookup()
                        {
                            id = rd.Id,
                            sourceOfReferral = rd.ReferralSource
                        };
                        referral_list.Add(obj);
                    }

                    response = new DefaultAPIResponse() { 
                        status = true,
                        message = @"referral list retrieved successfully",
                        data = referral_list
                    };
                }
                else { response = new DefaultAPIResponse() { status = false, message = @"No data" }; }

                return response;
            }
            catch(Exception ex)
            {
                return response = new DefaultAPIResponse() { 
                    status = false,
                    message = $"error message: {ex.Message}"
                };
            }
        }

        public async Task<DefaultAPIResponse> CreateReferralAsync(ReferralLookup payLoad)
        {
            //creates a new referral in the data store
            DefaultAPIResponse rsp = null;

            try
            {
                var record = await configure.Tclientreferralsources.Where(r => r.ReferralSource == payLoad.sourceOfReferral.Trim()).FirstOrDefaultAsync();
                if (record == null)
                {
                    Tclientreferralsource obj = new Tclientreferralsource()
                    {
                        ReferralSource = payLoad.sourceOfReferral.Trim().ToUpper()
                    };

                    await configure.AddAsync(obj);
                    await configure.SaveChangesAsync();

                    rsp = new DefaultAPIResponse() { status = true, message = $"Refferal {payLoad.sourceOfReferral} added successfully to datastore" };
                }
                else { rsp = new DefaultAPIResponse() { status = false, message = $"Referral source {payLoad.sourceOfReferral} already exist in data store" }; }

                return rsp;
            }
            catch(Exception exc)
            {
                return rsp = new DefaultAPIResponse() { 
                    status = false,
                    message = $"{exc.Message}"
                };
            }
        }

        public async Task<DefaultAPIResponse> UpdateReferralAsync(ReferralLookup payLoad)
        {
            //updates a referral resource in the data store

            string oldreferralName = string.Empty;
            DefaultAPIResponse rsp = null;

            try
            {
                var record = await configure.Tclientreferralsources.Where(r => r.Id == payLoad.id).FirstOrDefaultAsync();
                if (record != null)
                {
                    oldreferralName = record.ReferralSource;

                    record.ReferralSource = payLoad.sourceOfReferral.Trim().ToUpper();
                    await configure.SaveChangesAsync();

                    rsp = new DefaultAPIResponse() { status = true, message = $"Referral {oldreferralName} updated to {payLoad.sourceOfReferral} successfully!!!"  };
                }
                else { rsp = new DefaultAPIResponse() { status = false, message = @"Id of Refferal cannot be zero (0)" }; }

                return rsp;
            }
            catch(Exception exc)
            {
                return rsp = new DefaultAPIResponse()
                {
                    status = false,
                    message = $"error: {exc.Message}"
                };
            }
        }
        
        public async Task<UploadAPIResponse> UploadRefferalDataAsync(IEnumerable<ReferralLookup> payLoad)
        {
            UploadAPIResponse response = null;
            int success = 0;
            int failed = 0;
            List<ReferralLookup> successList = new List<ReferralLookup>();
            List<ReferralLookup> errorList = new List<ReferralLookup>();
            List<string> errors = new List<string>();

            try
            {
                foreach(var record in payLoad)
                {                   
                    try
                    {
                        var query = (from rf in configure.Tclientreferralsources
                                     where rf.ReferralSource == record.sourceOfReferral
                                     select new
                                     {
                                         Id = rf.Id,
                                         refSource = rf.ReferralSource
                                     });

                        if (query.Count() == 0)
                        {
                            //insert 
                            Tclientreferralsource objRefferalSource = new Tclientreferralsource()
                            {
                                ReferralSource = record.sourceOfReferral.Trim()
                            };

                            await configure.AddAsync(objRefferalSource);
                            await configure.SaveChangesAsync();

                            success += 1;
                            successList.Add(record);
                        }
                        else
                        {
                            failed += 1;
                            errorList.Add(record);
                            errors.Add($"Referral by name '{record.sourceOfReferral}' already exist in the data store");
                        }
                    }
                    catch(Exception innerExc)
                    {
                        failed += 1;
                        errors.Add($"inner Exception error: {innerExc.Message}");
                    }
                }

                return response = new UploadAPIResponse() { 
                    status = true,
                    successCount = success,
                    errorCount = failed,
                    data = successList,
                    errorList = errorList,
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

        #endregion
    }
}
