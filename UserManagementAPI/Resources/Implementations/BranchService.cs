#nullable disable

using UserManagementAPI.Resources;
using UserManagementAPI.Resources.Interfaces;
using UserManagementAPI.POCOs;
using UserManagementAPI.Response;
using System.Runtime.InteropServices;

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

    }
}
