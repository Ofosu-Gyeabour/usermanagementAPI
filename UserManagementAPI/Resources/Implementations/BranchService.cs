#nullable disable

using UserManagementAPI.Resources;
using UserManagementAPI.Resources.Interfaces;
using UserManagementAPI.POCOs;
using UserManagementAPI.Response;
using System.Runtime.InteropServices;
using Microsoft.EntityFrameworkCore.Storage;
using UserManagementAPI.Models;

namespace UserManagementAPI.Resources.Implementations
{
    public class BranchService
       : IBranchService
    {
        swContext config;

        public BranchService()
        {
            config = new swContext();
        }

        public async Task<DefaultAPIResponse> GetBranchesAsync()
        {
            //get all branches
            DefaultAPIResponse response = null;
            List<BranchLookup> results = null;

            try
            {
                //using linq
                var query = (from b in config.Tbranches
                             join c in config.Tcompanies on b.CompanyId equals c.CompanyId

                             select new
                             {
                                 id = b.Id,
                                 nameOfbranch = b.BranchName,
                                 companyId = c.CompanyId,
                                 nameOfcompany = c.Company
                             });

                if (query != null)
                {
                    results = new List<BranchLookup>();
                    foreach(var q in query)
                    {
                        var obj = new BranchLookup()
                        {
                            id = q.id,
                            nameOfbranch = q.nameOfbranch,
                            oCompany = new CompanyLookup
                            {
                                id = q.companyId,
                                nameOfcompany = q.nameOfcompany
                            }
                        };

                        results.Add(obj);
                    }

                    response = new DefaultAPIResponse()
                    {
                        status = true,
                        message = @"success",
                        data = results
                    };
                }
                else { response = new DefaultAPIResponse() { status = false, message = @"No data" }; }

                return response;
            }
            catch(Exception ex)
            {
                return response = new DefaultAPIResponse()
                {
                    status = false,
                    message = $"error: {ex.Message}"
                };
            }
        }
        public async Task<DefaultAPIResponse> GetBranchAsync()
        {
            //get particular branch
            return new DefaultAPIResponse() { };
        }
        public async Task<DefaultAPIResponse> CreateBranchAsync(BranchLookup payLoad)
        {
            //todo: create new branch
            DefaultAPIResponse response = null;

            try
            {
                //gets both the company and the company Id from the client side for user createion
                var dt = await config.Tbranches.Where(b => b.BranchName == payLoad.nameOfbranch).FirstOrDefaultAsync();
                if (dt == null)
                {
                    Tbranch objBranch = new Tbranch()
                    {
                        BranchName = payLoad.nameOfbranch,
                        CompanyId = payLoad.oCompany.id
                    };

                    await config.AddAsync(objBranch);
                    await config.SaveChangesAsync();

                    response = new DefaultAPIResponse()
                    {
                        status = true,
                        message = $"Branch with name {payLoad.nameOfbranch} added to data store",
                        data = payLoad
                    };
                }
                else { response = new DefaultAPIResponse() { status = false, message = $"Branch with name '{payLoad.nameOfbranch}' already exist in the data store" }; }

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
        public async Task<UploadAPIResponse> UploadBranchAsync(IEnumerable<BranchLookup> payLoad)
        {
            UploadAPIResponse response = null;
            int success = 0;
            int failed = 0;
            List<BranchLookup> successList = new List<BranchLookup>();
            List<BranchLookup> errorList = new List<BranchLookup>();
            List<string> errors = new List<string>();

            Tcompany oc = null;
            //TODO: throwing error...use linq instead
            try
            {
                foreach(var record in payLoad)
                {
                    try
                    {                      
                            using (var cf = new swContext())
                            {
                                oc = await cf.Tcompanies.Where(x => x.Company == record.oCompany.nameOfcompany.Trim()).FirstOrDefaultAsync();
                            }

                            if (oc != null)
                            {
                                var query = (from b in config.Tbranches
                                             join c in config.Tcompanies on b.CompanyId equals c.CompanyId
                                             where b.BranchName == record.nameOfbranch && c.Company == record.oCompany.nameOfcompany
                                             select new
                                             {
                                                 Id = b.Id,
                                                 branchName = b.BranchName,
                                                 companyName = c.Company,
                                                 companyId = c.CompanyId
                                             });

                                if (query.Count() == 0)
                                {
                                    //does not exist. create the branch resource
                                    Tbranch obj = new Tbranch()
                                    {
                                        BranchName = record.nameOfbranch.Trim(),
                                        CompanyId = oc.CompanyId
                                    };

                                    await config.AddAsync(obj);
                                    await config.SaveChangesAsync();

                                    success += 1;
                                    successList.Add(record);
                                }
                                else
                                {
                                    failed += 1;
                                    errorList.Add(record);
                                    errors.Add($"Branch '{record.nameOfbranch}' already exist for company '{record.oCompany.nameOfcompany}' in the data store");
                                }
                            }
                            else
                            {
                                failed += 1;
                                errorList.Add(record);
                            }
                    }
                    catch(Exception x)
                    {
                        errorList.Add(record);
                        errors.Add($"Branch '{record.nameOfbranch}' already exist in the data store");
                    }
                }

                return response = new UploadAPIResponse()
                {
                    status = true,
                    message = $"Total records= {payLoad.Count().ToString()}, successful inserts= {success.ToString()}, failed inserts= {failed.ToString()}",
                    data = successList,
                    successCount = success,
                    errorList = errorList,
                    errorMessageList = errors,
                    errorCount = failed
                };
            }
            catch(Exception x)
            {
                return response = new UploadAPIResponse()
                {
                    status = false,
                    message = $"error: {x.Message}",
                    errorList = errorList
                };
            }
        }

    }
}
