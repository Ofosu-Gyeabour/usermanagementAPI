#nullable disable
using Microsoft.EntityFrameworkCore;
using UserManagementAPI.Resources;
using UserManagementAPI.POCOs;
using UserManagementAPI.Response;
using UserManagementAPI.Resources.Interfaces;
using System.Security.Cryptography;
using Microsoft.Extensions.Logging.Abstractions;
using System.ComponentModel.DataAnnotations;

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
                                 uniqueID = tc.Id,
                                 clientType = ct.Describ,
                                 clientTypeId = ct.Id,
                                 associatedCompany = cmp.Company,
                                 associatedCompanyId = tc.AssociatedCompanyId,
                                 channelType = chn.Channel,
                                 channelTypeId = tc.ChannelTypeId,
                                 firstname = tc.Firstname,
                                 middlenames = tc.Middlenames,
                                 surname = tc.Surname,
                                 clientBusiness = tc.ClientBusinessName,
                                 mobileNo = tc.MobileNo,
                                 whatsappNo = tc.WhatsappNo,
                                 homeTel = tc.HomeTelephone,
                                 workTel = tc.WorkTelephone,
                                 emailAddr = tc.ClientEmailAddr,
                                 emailAddr2 = tc.ClientEmailAddr2,
                                 accNo = tc.ClientAccNo,
                                 cityId = tc.ClientCityId,
                                 city = cty.CityName,
                                 countryId = tc.ClientCountryId,
                                 nameOfcountry = cntr.CountryName,
                                 postCode = tc.ClientPostCode,
                                 referralId = tc.ReferralId,
                                 referral = rf.ReferralSource,
                                 collectionInstruction = tc.CollectionInstruction
                             });


                    var customerList = await Query.ToListAsync().ConfigureAwait(false);

                //convert list to type genericcustomerlookup
                customerList
                .Select(q => new IndividualCustomerLookup()
                    {
                        id = q.uniqueID,
                        oClientType = new ClientTypeLookup()
                        {
                            id = (int)q.clientTypeId,
                            clientTypeDescrib = q.clientType
                        },
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
                        oCompany = new CompanyLookup()
                        {
                            id = (int)q.associatedCompanyId,
                            nameOfcompany = q.associatedCompany
                        },
                        oChannelType = new ChannelTypeLookup()
                        {
                            id = (int)q.channelTypeId,
                            nameOfchannel = q.channelType
                        },
                        oReferral = new ReferralLookup()
                        {
                            id = (int)q.referralId,
                            sourceOfReferral = q.referral
                        },
                        postCode = q.postCode,
                        mobileNo = q.mobileNo,
                        whatsappNo = q.whatsappNo,
                        homeTelephone = q.homeTel,
                        workTelephone = q.workTel,
                        clientEmail = q.emailAddr
                    }).ToList();

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
                    var customerList = await Query.ToListAsync().ConfigureAwait(false);
                    customerList.Select(q => new CorporateCustomerLookup()
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
                    }).ToList();

                    response = new DefaultAPIResponse()
                    {
                        status = true,
                        message = @"success",
                        data = customerList
                    };
                }

                return response;
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
                    var customerList = await Query.ToListAsync().ConfigureAwait(false);
                    customerList.Select(q => new IndividualCustomerLookup()
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
                    }).ToList();

                    response = new DefaultAPIResponse()
                    {
                        status = true,
                        message = @"success",
                        data = customerList
                    };
                }

                return response;

            }
            catch(Exception x)
            {
                return response = new DefaultAPIResponse() { 
                    status = false,
                    message = $"error: {x.Message}"
                };
            }
        }

        
        public async Task<DefaultAPIResponse> GetGenericCustomerListAsync(SearchParam param)
        {
            //gets a generic customer for a snap lookup
            DefaultAPIResponse response = null;
            List<GenericCustomerLookup> gList = null;

            try
            {

                if (param.searchCriteria == @"ACCOUNT No")
                {
                    var genericQuery = (from tc in config.TClients
                            join ct in config.TClientTypes on tc.ClientTypeId equals ct.Id
                            join tca in config.TClientAddresses on tc.Id equals tca.ClientId
                            where tc.ClientAccNo.StartsWith(param.stringValue)

                            select new
                            {
                                uniqueID = tc.Id,
                                clientTypeId = tc.ClientTypeId,
                                clientTypeDescrib = ct.Describ,
                                accNo = tc.ClientAccNo,
                                nameOrcompany = ct.Id == 1 ? string.Format("{0} {1} {2}", tc.Firstname, tc.Middlenames.Trim(), tc.Surname) : tc.ClientBusinessName,
                                postCode = tc.ClientPostCode,
                                mobileNo = string.Format("{0} | {1}", tc.MobileNo, tc.WhatsappNo),
                                address = string.Format("{0} {1} {2} {3}", tca.ClientAddr1, tca.ClientAddr2, tca.ClientAddr3, tca.ClientAddr4)
                            });

                    var accountCriteriaList = await genericQuery.ToListAsync().ConfigureAwait(false);
                    if (accountCriteriaList.Count() > 0)
                    {
                        gList = new List<GenericCustomerLookup>();
                        foreach(var q in accountCriteriaList)
                        {
                            var obj = new GenericCustomerLookup()
                            {
                                id = q.uniqueID,
                                accountNo = q.accNo,
                                oClientType = new ClientTypeLookup()
                                {
                                    id = (int)q.clientTypeId,
                                    clientTypeDescrib = q.clientTypeDescrib
                                },
                                nameOrcompany = q.nameOrcompany,
                                postCode = q.postCode,
                                address = q.address,
                                mobileNo = q.mobileNo
                            };

                            gList.Add(obj);
                        }
                    }
                }

                if (param.searchCriteria == @"NAME")
                {
                    var genericQuery = (from tc in config.TClients
                            join ct in config.TClientTypes on tc.ClientTypeId equals ct.Id
                            join tca in config.TClientAddresses on tc.Id equals tca.ClientId
                            where tc.Surname.StartsWith(param.stringValue) || tc.Firstname.StartsWith(param.stringValue) || tc.Middlenames.StartsWith(param.stringValue) ||
                                  tc.ClientBusinessName.StartsWith(param.stringValue)

                            select new
                            {
                                uniqueID = tc.Id,
                                clientTypeId = tc.ClientTypeId,
                                clientTypeDescrib = ct.Describ,
                                accNo = tc.ClientAccNo,
                                nameOrcompany = ct.Id == 1 ? string.Format("{0} {1} {2}", tc.Firstname, tc.Middlenames.Trim(), tc.Surname) : tc.ClientBusinessName,
                                postCode = tc.ClientPostCode,
                                mobileNo = string.Format("{0} | {1}", tc.MobileNo, tc.WhatsappNo),
                                address = string.Format("{0} {1} {2} {3}", tca.ClientAddr1, tca.ClientAddr2, tca.ClientAddr3, tca.ClientAddr4)
                            });

                    var clientNameList = await genericQuery.ToListAsync().ConfigureAwait(false);
                    if (clientNameList.Count() > 0)
                    {
                        gList = new List<GenericCustomerLookup>();
                        foreach(var q in clientNameList)
                        {
                            var obj = new GenericCustomerLookup()
                            {
                                id = q.uniqueID,
                                accountNo = q.accNo,
                                oClientType = new ClientTypeLookup()
                                {
                                    id = (int)q.clientTypeId,
                                    clientTypeDescrib = q.clientTypeDescrib
                                },
                                nameOrcompany = q.nameOrcompany,
                                postCode = q.postCode,
                                address = q.address,
                                mobileNo = q.mobileNo
                            };

                            gList.Add(obj);
                        }
                    }
                }

                if (param.searchCriteria == @"MOBILE")
                {
                    var genericQuery = (from tc in config.TClients
                            join ct in config.TClientTypes on tc.ClientTypeId equals ct.Id
                            join tca in config.TClientAddresses on tc.Id equals tca.ClientId
                            where tc.MobileNo.StartsWith(param.stringValue) || tc.WhatsappNo.StartsWith(param.stringValue)

                            select new
                            {
                                uniqueID = tc.Id,
                                clientTypeId = tc.ClientTypeId,
                                clientTypeDescrib = ct.Describ,
                                accNo = tc.ClientAccNo,
                                nameOrcompany = ct.Id == 1 ? string.Format("{0} {1} {2}", tc.Firstname, tc.Middlenames.Trim(), tc.Surname) : tc.ClientBusinessName,
                                postCode = tc.ClientPostCode,
                                mobileNo = string.Format("{0} | {1}", tc.MobileNo, tc.WhatsappNo),
                                address = string.Format("{0} {1} {2} {3}", tca.ClientAddr1, tca.ClientAddr2, tca.ClientAddr3, tca.ClientAddr4)
                            });

                    var clientList = await genericQuery.ToListAsync().ConfigureAwait(false);
                    if (clientList.Count() > 0)
                    {
                        gList = new List<GenericCustomerLookup>();
                        foreach(var q in clientList)
                        {
                            var obj = new GenericCustomerLookup()
                            {
                                id = q.uniqueID,
                                accountNo = q.accNo,
                                oClientType = new ClientTypeLookup()
                                {
                                    id = (int)q.clientTypeId,
                                    clientTypeDescrib = q.clientTypeDescrib
                                },
                                nameOrcompany = q.nameOrcompany,
                                postCode = q.postCode,
                                address = q.address,
                                mobileNo = q.mobileNo
                            };

                            gList.Add(obj);
                        }
                    }
                }

                if (param.searchCriteria == @"TELEPHONE")
                {
                    var genericQuery = (from tc in config.TClients
                            join ct in config.TClientTypes on tc.ClientTypeId equals ct.Id
                            join tca in config.TClientAddresses on tc.Id equals tca.ClientId
                            where tc.HomeTelephone.StartsWith(param.stringValue) || tc.WorkTelephone.StartsWith(param.stringValue)

                            select new
                            {
                                uniqueID = tc.Id,
                                clientTypeId = tc.ClientTypeId,
                                clientTypeDescrib = ct.Describ,
                                accNo = tc.ClientAccNo,
                                nameOrcompany = ct.Id == 1 ? string.Format("{0} {1} {2}", tc.Firstname, tc.Middlenames.Trim(), tc.Surname) : tc.ClientBusinessName,
                                postCode = tc.ClientPostCode,
                                mobileNo = string.Format("{0} | {1}", tc.MobileNo, tc.WhatsappNo),
                                address = string.Format("{0} {1} {2} {3}", tca.ClientAddr1, tca.ClientAddr2, tca.ClientAddr3, tca.ClientAddr4)
                            });

                    var teleList = await genericQuery.ToListAsync().ConfigureAwait(false);
                    if(teleList.Count() > 0)
                    {
                        gList = new List<GenericCustomerLookup>();
                        foreach(var q in teleList)
                        {
                            var obj = new GenericCustomerLookup()
                            {
                                id = q.uniqueID,
                                accountNo = q.accNo,
                                oClientType = new ClientTypeLookup()
                                {
                                    id = (int)q.clientTypeId,
                                    clientTypeDescrib = q.clientTypeDescrib
                                },
                                nameOrcompany = q.nameOrcompany,
                                postCode = q.postCode,
                                address = q.address,
                                mobileNo = q.mobileNo
                            };

                            gList.Add(obj);
                        }
                    }
                }

                if (param.searchCriteria == @"EMAIL")
                {
                    var genericQuery = (from tc in config.TClients
                            join ct in config.TClientTypes on tc.ClientTypeId equals ct.Id
                            join tca in config.TClientAddresses on tc.Id equals tca.ClientId
                            where tc.ClientEmailAddr.StartsWith(param.stringValue) || tc.ClientEmailAddr2.StartsWith(param.stringValue)

                            select new
                            {
                                uniqueID = tc.Id,
                                clientTypeId = tc.ClientTypeId,
                                clientTypeDescrib = ct.Describ,
                                accNo = tc.ClientAccNo,
                                nameOrcompany = ct.Id == 1 ? string.Format("{0} {1} {2}", tc.Firstname, tc.Middlenames.Trim(), tc.Surname) : tc.ClientBusinessName,
                                postCode = tc.ClientPostCode,
                                mobileNo = string.Format("{0} | {1}", tc.MobileNo, tc.WhatsappNo),
                                address = string.Format("{0} {1} {2} {3}", tca.ClientAddr1, tca.ClientAddr2, tca.ClientAddr3, tca.ClientAddr4)
                            });

                    var genericCustomerList = await genericQuery.ToListAsync().ConfigureAwait(false);
                    if (genericCustomerList.Count() > 0)
                    {
                        gList = new List<GenericCustomerLookup>();
                        foreach(var q in genericCustomerList)
                        {
                            var obj = new GenericCustomerLookup()
                            {
                                id = q.uniqueID,
                                accountNo = q.accNo,
                                oClientType = new ClientTypeLookup()
                                {
                                    id = (int)q.clientTypeId,
                                    clientTypeDescrib = q.clientTypeDescrib
                                },
                                nameOrcompany = q.nameOrcompany,
                                postCode = q.postCode,
                                address = q.address,
                                mobileNo = q.mobileNo
                            };

                            gList.Add(obj);
                        }
                    }
                }

                if (param.searchCriteria == @"POSTCODE")
                {
                    var genericQuery = (from tc in config.TClients
                                        join ct in config.TClientTypes on tc.ClientTypeId equals ct.Id
                                        join tca in config.TClientAddresses on tc.Id equals tca.ClientId
                                        where tc.ClientPostCode.StartsWith(param.stringValue)

                                        select new
                                        {
                                            uniqueID = tc.Id,
                                            clientTypeId = tc.ClientTypeId,
                                            clientTypeDescrib = ct.Describ,
                                            accNo = tc.ClientAccNo,
                                            nameOrcompany = ct.Id == 1 ? string.Format("{0} {1} {2}", tc.Firstname, tc.Middlenames.Trim(), tc.Surname) : tc.ClientBusinessName,
                                            postCode = tc.ClientPostCode,
                                            mobileNo = string.Format("{0} | {1}", tc.MobileNo, tc.WhatsappNo),
                                            address = string.Format("{0} {1} {2} {3}", tca.ClientAddr1, tca.ClientAddr2, tca.ClientAddr3, tca.ClientAddr4)
                                        });

                    var postCodeList = await genericQuery.ToListAsync().ConfigureAwait(false);
                    if(postCodeList.Count() > 0)
                    {
                        gList = new List<GenericCustomerLookup>();
                        foreach(var q in postCodeList)
                        {
                            var obj = new GenericCustomerLookup()
                            {
                                id = q.uniqueID,
                                accountNo = q.accNo,
                                oClientType = new ClientTypeLookup()
                                {
                                    id = (int)q.clientTypeId,
                                    clientTypeDescrib = q.clientTypeDescrib
                                },
                                nameOrcompany = q.nameOrcompany,
                                postCode = q.postCode,
                                address = q.address,
                                mobileNo = q.mobileNo
                            };

                            gList.Add(obj);
                        }
                    }
                }

                return response = new DefaultAPIResponse()
                {
                    status = true,
                    message = @"success",
                    data = gList
                };
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

        public async Task<PagedResponseAPI> PaginationTestAsync(int pageNumber, int pageSize)
        {
            PagedResponseAPI response = null;
            //int INDIVIDUAL_CODE = 1;

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
                                 clientType = ct.Describ,
                                 clientTypeId = ct.Id,
                                 associatedCompany = cmp.Company,
                                 associatedCompanyId = tc.AssociatedCompanyId,
                                 channelType = chn.Channel,
                                 channelTypeId = tc.ChannelTypeId,
                                 firstname = tc.Firstname,
                                 middlenames = tc.Middlenames,
                                 surname = tc.Surname,
                                 clientBusiness = tc.ClientBusinessName,
                                 mobileNo = tc.MobileNo,
                                 whatsappNo = tc.WhatsappNo,
                                 homeTel = tc.HomeTelephone,
                                 workTel = tc.WorkTelephone,
                                 emailAddr = tc.ClientEmailAddr,
                                 emailAddr2 = tc.ClientEmailAddr2,
                                 accNo = tc.ClientAccNo,
                                 cityId = tc.ClientCityId,
                                 city = cty.CityName,
                                 countryId = tc.ClientCountryId,
                                 nameOfcountry = cntr.CountryName,
                                 postCode = tc.ClientPostCode,
                                 referralId = tc.ReferralId,
                                 referral = rf.ReferralSource,
                                 collectionInstruction = tc.CollectionInstruction
                             }).Skip((pageNumber - 1) * pageSize)
                             .Take(pageSize);

                var list = await Query.ToListAsync().ConfigureAwait(false);

                list
                .Select(q => new CorporateCustomerLookup()
                {
                    id = q.id,
                    oClientType = new ClientTypeLookup()
                    {
                        id = (int)q.clientTypeId,
                        clientTypeDescrib = q.clientType
                    },
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
                    oCompany = new CompanyLookup()
                    {
                        id = (int)q.associatedCompanyId,
                        nameOfcompany = q.associatedCompany
                    },
                    oChannelType = new ChannelTypeLookup()
                    {
                        id = (int)q.channelTypeId,
                        nameOfchannel = q.channelType
                    },
                    oReferral = new ReferralLookup()
                    {
                        id = (int)q.referralId,
                        sourceOfReferral = q.referral
                    },
                    postCode = q.postCode,
                    mobileNo = q.mobileNo,
                    whatsappNo = q.whatsappNo,
                    homeTelephone = q.homeTel,
                    workTelephone = q.workTel,
                    clientEmail = q.emailAddr
                }).ToList();

                //return in api response
                
                
                return response = new PagedResponseAPI(pageNumber, pageSize)
                {
                    status = true,
                    message = $"{list.Count().ToString()} records retrieved",
                    data = list,
                    TotalRecords= list.Count(),
                    TotalPages = (list.Count() / pageSize)
                };
            }
            catch(Exception x)
            {
                return response = new PagedResponseAPI(pageNumber, pageSize) {
                    status = false,
                    message = $"error: {x.Message}",
                    data = null
                };
            }
        }
    
        public async Task<DefaultAPIResponse> GetClientRecordAsync(SearchParam param)
        {
            object dta = null;
            DefaultAPIResponse response = null;

            try
            {
                if (param.searchCriteria == @"ID")
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
                                 where tc.Id == int.Parse(param.stringValue)

                                 select new
                                 {
                                     uniqueID = tc.Id,
                                     clientType = ct.Describ,
                                     clientTypeId = ct.Id,
                                     associatedCompany = cmp.Company,
                                     associatedCompanyId = tc.AssociatedCompanyId,
                                     channelType = chn.Channel,
                                     channelTypeId = tc.ChannelTypeId,
                                     firstname = tc.Firstname,
                                     middlenames = tc.Middlenames,
                                     surname = tc.Surname,
                                     clientBusiness = tc.ClientBusinessName,
                                     mobileNo = tc.MobileNo,
                                     whatsappNo = tc.WhatsappNo,
                                     homeTel = tc.HomeTelephone,
                                     workTel = tc.WorkTelephone,
                                     emailAddr = tc.ClientEmailAddr,
                                     emailAddr2 = tc.ClientEmailAddr2,
                                     accNo = tc.ClientAccNo,
                                     cityId = tc.ClientCityId,
                                     city = cty.CityName,
                                     countryId = tc.ClientCountryId,
                                     nameOfcountry = cntr.CountryName,
                                     postCode = tc.ClientPostCode,
                                     referralId = tc.ReferralId,
                                     referral = rf.ReferralSource,
                                     collectionInstruction = tc.CollectionInstruction
                                 });

                    var obj = await Query.FirstOrDefaultAsync().ConfigureAwait(false);
                    if ((obj != null) && (obj.clientTypeId == 1))
                    {
                        dta = new IndividualCustomerLookup()
                        {
                            id = obj.uniqueID,
                            oClientType = new ClientTypeLookup()
                            {
                                id = obj.clientTypeId,
                                clientTypeDescrib = obj.clientType
                            },
                            oCompany = new CompanyLookup()
                            {
                                id = (int)obj.associatedCompanyId,
                                nameOfcompany = obj.associatedCompany
                            },
                            oChannelType = new ChannelTypeLookup()
                            {
                                id = (int)obj.channelTypeId,
                                nameOfchannel = obj.channelType
                            },
                            firstname = obj.firstname.Trim().ToUpper(),
                            middlenames = obj.middlenames.Trim().ToUpper(),
                            surname = obj.surname.Trim().ToUpper(),
                            mobileNo = obj.mobileNo.Trim(),
                            whatsappNo = obj.whatsappNo.Trim(),
                            homeTelephone = obj.homeTel.Trim(),
                            workTelephone = obj.workTel.Trim(),
                            clientEmail = obj.emailAddr.Trim(),
                            clientEmail2 = obj.emailAddr2.Trim(),
                            oCity = new CityLookup()
                            {
                                id = (int)obj.cityId,
                                nameOfcity = obj.city.Trim().ToUpper()
                            },
                            oCountry = new CountryLookup()
                            {
                                id = (int)obj.countryId,
                                nameOfcountry = obj.nameOfcountry.Trim().ToUpper()
                            },
                            postCode = obj.postCode.Trim().ToUpper(),
                            oReferral = new ReferralLookup()
                            {
                                id = (int)obj.referralId,
                                sourceOfReferral = obj.referral.Trim().ToUpper()
                            },
                            collectionInstruction = obj.collectionInstruction,
                            accountNo = obj.accNo != string.Empty ? obj.accNo.Trim().ToUpper() : string.Empty,
                            clientBusiness = obj.clientBusiness == string.Empty ? string.Empty : obj.clientBusiness.Trim().ToUpper()
                        };
                    }

                    if ((obj != null) && (obj.clientTypeId == 2))
                    {
                        dta = new CorporateCustomerLookup() {
                            id = obj.uniqueID,
                            oClientType = new ClientTypeLookup()
                            {
                                id = obj.clientTypeId,
                                clientTypeDescrib = obj.clientType.Trim().ToUpper()
                            },
                            oCompany = new CompanyLookup()
                            {
                                id = (int)obj.associatedCompanyId,
                                nameOfcompany = obj.associatedCompany.Trim().ToUpper()
                            },
                            oChannelType = new ChannelTypeLookup()
                            {
                                id = (int)obj.channelTypeId,
                                nameOfchannel = obj.channelType.Trim().ToUpper()
                            },
                            clientBusiness = obj.clientBusiness == string.Empty ? string.Empty : obj.clientBusiness.Trim().ToUpper(),
                            mobileNo = obj.mobileNo.Trim(),
                            whatsappNo = obj.whatsappNo.Trim(),
                            homeTelephone = obj.homeTel.Trim(),
                            workTelephone = obj.workTel.Trim(),
                            clientEmail = obj.emailAddr.Trim(),
                            clientEmail2 = obj.emailAddr2.Trim(),
                            oCity = new CityLookup()
                            {
                                id = (int)obj.cityId,
                                nameOfcity = obj.city.Trim().ToUpper()
                            },
                            oCountry = new CountryLookup()
                            {
                                id = (int)obj.countryId,
                                nameOfcountry = obj.nameOfcountry.Trim().ToUpper()
                            },
                            postCode = obj.postCode.Trim().ToUpper(),
                            oReferral = new ReferralLookup()
                            {
                                id = (int)obj.referralId,
                                sourceOfReferral = obj.referral.Trim().ToUpper()
                            },
                            collectionInstruction = obj.collectionInstruction,
                            accountNo = obj.accNo == string.Empty ? string.Empty : obj.accNo.Trim().ToUpper()
                        };
                    }

                    response = new DefaultAPIResponse() { 
                        status = true,
                        message = @"success",
                        data = dta
                    };

                }

                return response;
            }
            catch(Exception x)
            {
                return response = new DefaultAPIResponse()
                {
                    status = false,
                    message = $"error:{x.Message}"
                };
            }

            
        }

    }
}
