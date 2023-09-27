#nullable disable

using UserManagementAPI.Resources.Interfaces;
using UserManagementAPI.POCOs;
using UserManagementAPI.Response;
using System.Diagnostics;
using System.ComponentModel.Design;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Storage;

namespace UserManagementAPI.Resources.Implementations
{
    public class CompanyService :ICompanyService
    {
        swContext configure;
        public CompanyService()
        {
            configure = new swContext();
        }

        #region implementations

        public async Task<DefaultAPIResponse> GetCompaniesAsync()
        {
            //gets list of companies
            DefaultAPIResponse response = null;
            List<CompanyLookup> companyList = null;

            try
            {
                var result = (from c in configure.Tcompanies
                              join tc in configure.TCities on c.CompanyTownId equals tc.Id
                              join tcc in configure.TCountryLookups on c.CompanyCountryId equals tcc.CountryId

                              select new
                              {
                                  Id = c.CompanyId,
                                  company = c.Company,
                                  address = c.CompanyAddress,
                                  cityId = tc.Id,
                                  nameOfcity = tc.CityName,
                                  countryId = tcc.CountryId,
                                  nameOfcountry = tcc.CountryName,
                                  companyLogo = c.CompanyLogo,
                                  incorporationDate = c.IncorporationDate
                              });

                //iterate
                if (result != null)
                {
                    companyList = new List<CompanyLookup>();

                    foreach(var item in result)
                    {
                        var obj = new CompanyLookup()
                        {
                            id = item.Id,
                            nameOfcompany = item.company,
                            addressOfcompany = item.address,
                            oCity = new CityLookup()
                            {
                                id = item.cityId,
                                nameOfcity = item.nameOfcity
                            },
                            oCountry = new CountryLookup()
                            {
                                id = item.countryId,
                                nameOfcountry = item.nameOfcountry
                            },
                            companyLogo = item.companyLogo,
                            dateOfIncorporation = (DateTime) item.incorporationDate
                        };

                        companyList.Add(obj);
                    }

                    response = new DefaultAPIResponse()
                    {
                        status = true,
                        message = @"success",
                        data = companyList 
                    };
                }
                else { return response = new DefaultAPIResponse() { status = false, message = @"No data" }; }

                return response;
            }
            catch(Exception exc)
            {
                return response = new DefaultAPIResponse()
                {
                    status = false,
                    message = $"error: {exc.Message}"
                };
            }
        }

        public async Task<DefaultAPIResponse> GetCompanyTypesAsync()
        {
            //get the list of company type in the data store
            DefaultAPIResponse rsp = null;
            List<CompanyTypeLookup> results = null;
            try
            {
                var company_type_list = await configure.Tcompanytypes.ToListAsync();
                if (company_type_list != null)
                {
                    results = new List<CompanyTypeLookup>();
                    foreach(var ctl in company_type_list)
                    {
                        var obj = new CompanyTypeLookup() { 
                            id = ctl.Id,
                            typeOfCompany= ctl.Type
                        };

                        results.Add(obj);
                    }

                    rsp = new DefaultAPIResponse() { status= true, message = @"company types fetched successfully",data = results  };
                }
                else { rsp = new DefaultAPIResponse() { status = false, message = @"No data" }; }

                return rsp;
            }
            catch(Exception exc)
            {
                return rsp = new DefaultAPIResponse() { 
                    status = false,
                    message = $"error: {exc.Message}"
                };
            }
        }

        public async Task<DefaultAPIResponse> CreateCompanyTypeAsync(CompanyTypeLookup payLoad)
        {
            //creates a new resource in the company types data store
            DefaultAPIResponse rsp = null;
            try
            {
                var cRecord = await configure.Tcompanytypes.Where(ct => ct.Type == payLoad.typeOfCompany).FirstOrDefaultAsync();
                if (cRecord == null)
                {
                    var record = new Tcompanytype() { Type = payLoad.typeOfCompany };
                    await configure.AddAsync(record);
                    await configure.SaveChangesAsync();

                    rsp = new DefaultAPIResponse() { 
                        status = true,
                        message = $"company type {payLoad.typeOfCompany.Trim().ToUpper()} saved successfully!!!",
                        data = payLoad
                    };
                }
                else { rsp = new DefaultAPIResponse() { status = false, message = $"Company Type {payLoad.typeOfCompany.Trim().ToUpper()} already exist"  }; }

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

        public async Task<DefaultAPIResponse> UpdateCompanyTypeAsync(CompanyTypeLookup payLoad)
        {
            //updates an already existing company type resource
            DefaultAPIResponse rsp = null;
            string oldcomptypename_ = string.Empty;

            try
            {
                //needs the Id
                var record = await configure.Tcompanytypes.Where(ct => ct.Id == payLoad.id).FirstOrDefaultAsync();
                if (record != null)
                {
                    oldcomptypename_ = record.Type;

                    //updating...
                    record.Type = payLoad.typeOfCompany.Trim().ToUpper();

                    await configure.SaveChangesAsync();
                    rsp = new DefaultAPIResponse() { 
                        status = true,
                        message = $"Company Type {oldcomptypename_} updated to {payLoad.typeOfCompany.Trim().ToUpper()}",
                        data = payLoad
                    };
                }
                else { rsp = new DefaultAPIResponse() { status = false, message = $"Company type {payLoad.typeOfCompany} does not exist to update" }; }

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

        #endregion

    }
}
