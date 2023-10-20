#nullable disable
using Microsoft.EntityFrameworkCore;
using UserManagementAPI.Resources;
using UserManagementAPI.POCOs;
using UserManagementAPI.Response;
using UserManagementAPI.Resources.Interfaces;

namespace UserManagementAPI.Resources.Implementations
{
    public class ClientService : IClientService
    {
        swContext config;

        public ClientService()
        {
            config = new swContext();
        }

        public async Task<DefaultAPIResponse> GetClientInformationAsync()
        {
            DefaultAPIResponse response = null;
            List<CorporateCustomerLookup> customerList = null;

            try
            {
                var Query = (from tc in config.TClients 
                             join ct in config.TClientTypes on tc.ClientTypeId equals ct.Id
                             join cmp in config.Tcompanies on tc.AssociatedCompanyId equals cmp.CompanyId
                             join chn in config.TChannelTypes on tc.ChannelTypeId equals chn.ChannelTypeId
                             join cty in config.TCities on tc.ClientCityId equals cty.Id
                             join cntr in config.TCountryLookups on tc.ClientCountryId equals cntr.CountryId
                             join rf in config.Tclientreferralsources on tc.ReferralId equals rf.Id
                             join crt in config.Tusrs on tc.CreatedBy equals crt.UsrId
                             join usr in config.Tusrs on tc.LastModifiedBy equals usr.UsrId
                             select new
                             {
                                 id = tc.Id,
                                 accNo = tc.ClientAccNo,
                                 cityId = tc.ClientCityId,
                                 city = cty.CityName,
                                 countryId = tc.ClientCountryId,
                                 nameOfcountry = cntr.CountryName,
                                 postCode = tc.ClientPostCode,
                                 mobileNo = tc.MobileNo,
                                 whatsappNo = tc.WhatsappNo,
                                 homeTel = tc.HomeTelephone,
                                 workTel = tc.WorkTelephone,
                                 emailAddr = tc.ClientEmailAddr
                             });

                if (Query.Count() > 0)
                {
                    customerList = new List<CorporateCustomerLookup>();
                    foreach(var q in Query)
                    {
                        var obj = new CorporateCustomerLookup() { 
                            id = q.id,
                            accountNo = q.accNo,
                            oCity = new CityLookup() { 
                                id = (int)q.cityId,
                                nameOfcity = q.city
                            },
                            oCountry = new CountryLookup() { 
                                id = (int) q.countryId,
                                nameOfcountry = q.nameOfcountry
                            },
                            postCode = q.postCode,
                            mobileNo = q.mobileNo,
                            whatsappNo = q.whatsappNo,
                            homeTelephone = q.homeTel,
                            workTelephone = q.workTel,
                            clientEmail = q.emailAddr
                        };

                        customerList.Add(obj);
                    }
                }

                return response = new DefaultAPIResponse() { 
                    status = true,
                    message = @"success",
                    data = customerList
                };
            }
            catch(Exception x)
            {
                return response = new DefaultAPIResponse() { 
                    status = false,
                    message = $"error: {x.Message}"
                };
            }
        }

        public async Task<DefaultAPIResponse> GetCorporateClientAsync()
        {
            DefaultAPIResponse response = null;
            List<CorporateCustomerLookup> customerList = null;
            int CORPORATE_ID = 2;

            try
            {
                var Query = (from tc in config.TClients
                             join ct in config.TClientTypes on tc.ClientTypeId equals ct.Id
                             join cmp in config.Tcompanies on tc.AssociatedCompanyId equals cmp.CompanyId
                             join chn in config.TChannelTypes on tc.ChannelTypeId equals chn.ChannelTypeId
                             join cty in config.TCities on tc.ClientCityId equals cty.Id
                             join cntr in config.TCountryLookups on tc.ClientCountryId equals cntr.CountryId
                             join rf in config.Tclientreferralsources on tc.ReferralId equals rf.Id
                             join crt in config.Tusrs on tc.CreatedBy equals crt.UsrId
                             join usr in config.Tusrs on tc.LastModifiedBy equals usr.UsrId
                             where tc.ClientTypeId == CORPORATE_ID
                             select new
                             {
                                 id = tc.Id,
                                 accNo = tc.ClientAccNo,
                                 cityId = tc.ClientCityId,
                                 city = cty.CityName,
                                 countryId = tc.ClientCountryId,
                                 nameOfcountry = cntr.CountryName,
                                 postCode = tc.ClientPostCode,
                                 mobileNo = tc.MobileNo,
                                 whatsappNo = tc.WhatsappNo,
                                 homeTel = tc.HomeTelephone,
                                 workTel = tc.WorkTelephone,
                                 emailAddr = tc.ClientEmailAddr
                             });

                if (Query.Count() > 0)
                {
                    customerList = new List<CorporateCustomerLookup>();
                    foreach (var q in Query)
                    {
                        var obj = new CorporateCustomerLookup()
                        {
                            id = q.id,
                            accountNo = q.accNo,
                            oCity = new CityLookup()
                            {
                                id = (int)q.cityId,
                                nameOfcity = q.city
                            },
                            oCountry = new CountryLookup()
                            {
                                id = (int)q.countryId,
                                nameOfcountry = q.nameOfcountry
                            },
                            postCode = q.postCode,
                            mobileNo = q.mobileNo,
                            whatsappNo = q.whatsappNo,
                            homeTelephone = q.homeTel,
                            workTelephone = q.workTel,
                            clientEmail = q.emailAddr
                        };

                        customerList.Add(obj);
                    }
                }

                return response = new DefaultAPIResponse()
                {
                    status = true,
                    message = @"success",
                    data = customerList
                };
            }
            catch(Exception x)
            {
                return response = new DefaultAPIResponse() { 
                    status = false,
                    message = $"error: {x.Message}"
                };
            }
        }
        public async Task<DefaultAPIResponse> GetIndividualClientAsync()
        {
            DefaultAPIResponse response = null;
            List<IndividualCustomerLookup> customerList = null;
            int INDIVIDUAL_CODE = 1;

            try
            {
                var Query = (from tc in config.TClients
                             join ct in config.TClientTypes on tc.ClientTypeId equals ct.Id
                             join cmp in config.Tcompanies on tc.AssociatedCompanyId equals cmp.CompanyId
                             join chn in config.TChannelTypes on tc.ChannelTypeId equals chn.ChannelTypeId
                             join cty in config.TCities on tc.ClientCityId equals cty.Id
                             join cntr in config.TCountryLookups on tc.ClientCountryId equals cntr.CountryId
                             join rf in config.Tclientreferralsources on tc.ReferralId equals rf.Id
                             join crt in config.Tusrs on tc.CreatedBy equals crt.UsrId
                             join usr in config.Tusrs on tc.LastModifiedBy equals usr.UsrId
                             where tc.ClientTypeId == INDIVIDUAL_CODE

                             select new
                             {
                                 id = tc.Id,
                                 accNo = tc.ClientAccNo,
                                 cityId = tc.ClientCityId,
                                 city = cty.CityName,
                                 countryId = tc.ClientCountryId,
                                 nameOfcountry = cntr.CountryName,
                                 postCode = tc.ClientPostCode,
                                 mobileNo = tc.MobileNo,
                                 whatsappNo = tc.WhatsappNo,
                                 homeTel = tc.HomeTelephone,
                                 workTel = tc.WorkTelephone,
                                 emailAddr = tc.ClientEmailAddr,
                                 sname = tc.Surname,
                                 fname = tc.Firstname,
                                 onames = tc.Middlenames
                             });


                if (Query.Count() > 0)
                {
                    customerList = new List<IndividualCustomerLookup>();
                    foreach (var q in Query)
                    {
                        var obj = new IndividualCustomerLookup()
                        {
                            id = q.id,
                            accountNo = q.accNo,
                            oCity = new CityLookup()
                            {
                                id = (int)q.cityId,
                                nameOfcity = q.city
                            },
                            oCountry = new CountryLookup()
                            {
                                id = (int)q.countryId,
                                nameOfcountry = q.nameOfcountry
                            },
                            postCode = q.postCode,
                            mobileNo = q.mobileNo,
                            whatsappNo = q.whatsappNo,
                            homeTelephone = q.homeTel,
                            workTelephone = q.workTel,
                            clientEmail = q.emailAddr,
                            surname = q.sname,
                            firstname = q.fname,
                            middlenames = q.onames
                        };

                        customerList.Add(obj);
                    }
                }

                return response = new DefaultAPIResponse()
                {
                    status = true,
                    message = @"success",
                    data = customerList
                };

            }
            catch(Exception x)
            {
                return response = new DefaultAPIResponse() { 
                    status = false,
                    message = $"error: {x.Message}"
                };
            }
        }

    }
}
