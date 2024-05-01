#nullable disable
using Microsoft.EntityFrameworkCore;
using UserManagementAPI.Resources;
using UserManagementAPI.POCOs;
using UserManagementAPI.Response;
using UserManagementAPI.Resources.Interfaces;
using System.Security.Cryptography;
using Microsoft.Extensions.Logging.Abstractions;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System.Runtime.InteropServices;
using System.Transactions;
using UserManagementAPI.utils;
using UserManagementAPI.Models;
using UserManagementAPI.website;

namespace UserManagementAPI.Resources.Implementations
{
    public class ClientService : IClientService
    {
        swContext config;

        public ClientService()
        {
            config = new swContext();
        }

        public async Task<DefaultAPIResponse> GetClientInformationAsync(int page = 1, int pageSize = 10)
        {
            DefaultAPIResponse response = null;

            try
            {
                Helper helper = new Helper();
                var customerData = await helper.getAllClientAsync();

                //paginating data
                var totalCount = customerData.Count();
                var totalPages =  (int)Math.Ceiling((decimal)totalCount / pageSize);
                
                return response = new DefaultAPIResponse() { 
                    status = customerData.Count() > 0 ? true: false,
                    message = customerData.Count() > 0 ? $"{customerData.Count()} records fetched successfully" : @"No data fetched",
                    data = customerData
                                .Skip((page - 1) * pageSize)
                                .Take(pageSize)
                                .ToList()
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

        public async Task<GenericCustomerLookup> GetGenericCustomer(string acctParam)
        {
            //gets a unique customer whose account number is the param passed
            GenericCustomerLookup obj = null;

            try
            {
                var genericQuery = (from tc in config.TClients
                                    join ct in config.TClientTypes on tc.ClientTypeId equals ct.Id
                                    join tca in config.TClientAddresses on tc.Id equals tca.ClientId
                                    join cty in config.TCities on tc.ClientCityId equals cty.Id

                                    join cnt in config.TCountryLookups on tc.ClientCountryId equals cnt.CountryId   //added
                                    join rf in config.Tclientreferralsources on tc.ReferralId equals rf.Id

                                    where tc.ClientAccNo == acctParam

                                    select new
                                    {
                                        uniqueID = tc.Id,
                                        clientTypeId = tc.ClientTypeId,
                                        clientTypeDescrib = ct.Describ,
                                        accNo = tc.ClientAccNo,
                                        nameOrcompany = ct.Id == 1 ? string.Format("{0} {1} {2}", tc.Firstname, tc.Middlenames.Trim(), tc.Surname) : tc.ClientBusinessName,
                                        postCode = tc.ClientPostCode,
                                        mobileNo = tc.MobileNo == null ? string.Empty : tc.MobileNo,
                                        whatsappNo = tc.WhatsappNo == null ? string.Empty : tc.WhatsappNo,
                                        address = string.Format("{0} {1} {2} {3}", tca.ClientAddr1, tca.ClientAddr2, tca.ClientAddr3, tca.ClientAddr4),
                                        email = tc.ClientEmailAddr == null ? string.Empty : tc.ClientEmailAddr,
                                        cityid = cty.Id,
                                        cityName = cty.CityName,

                                        //added country
                                        countryId = tc.ClientCountryId,
                                        countryName = cnt.CountryName,
                                        referralId = tc.ReferralId,
                                        nameOfreferral = rf.ReferralSource
                                    });

                var accountCriteriaList = await genericQuery.ToListAsync().ConfigureAwait(false);

                obj = accountCriteriaList
                                .Select(q => new GenericCustomerLookup()
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
                                    mobileNo = q.mobileNo,
                                    whatsappNo = q.whatsappNo,
                                    emailAddress = q.email,

                                    oCity = new CityLookup()
                                    {
                                        id = q.cityid,
                                        nameOfcity = q.cityName
                                    },

                                    oCountry = new CountryLookup()
                                    {
                                        id = (int)q.countryId,
                                        nameOfcountry = q.countryName
                                    },
                                    oReferral = new ReferralLookup()
                                    {
                                        id = (int)q.referralId,
                                        sourceOfReferral = q.nameOfreferral
                                    }
                                }).FirstOrDefault();

                return obj;
            }
            catch(Exception x)
            {
                return obj;
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
                            join cty in config.TCities on tc.ClientCityId equals cty.Id

                            join cnt in config.TCountryLookups on tc.ClientCountryId equals cnt.CountryId   //added
                            join rf in config.Tclientreferralsources on tc.ReferralId equals rf.Id

                            where tc.ClientAccNo.StartsWith(param.stringValue)

                            select new
                            {
                                uniqueID = tc.Id,
                                clientTypeId = tc.ClientTypeId,
                                clientTypeDescrib = ct.Describ,
                                accNo = tc.ClientAccNo,
                                nameOrcompany = ct.Id == 1 ? string.Format("{0} {1} {2}", tc.Firstname, tc.Middlenames.Trim(), tc.Surname) : tc.ClientBusinessName,
                                postCode = tc.ClientPostCode,
                                mobileNo = tc.MobileNo == null? string.Empty:tc.MobileNo,
                                whatsappNo = tc.WhatsappNo == null? string.Empty: tc.WhatsappNo,
                                address = string.Format("{0} {1} {2} {3}", tca.ClientAddr1, tca.ClientAddr2, tca.ClientAddr3, tca.ClientAddr4),
                                email = tc.ClientEmailAddr == null ? string.Empty: tc.ClientEmailAddr,
                                cityid = cty.Id,
                                cityName = cty.CityName,

                                //added country
                                countryId = tc.ClientCountryId,
                                countryName = cnt.CountryName,
                                referralId = tc.ReferralId,
                                nameOfreferral = rf.ReferralSource
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
                                mobileNo = q.mobileNo,
                                whatsappNo = q.whatsappNo,
                                emailAddress = q.email,

                                oCity = new CityLookup()
                                {
                                    id = q.cityid,
                                    nameOfcity = q.cityName
                                },

                                oCountry = new CountryLookup()
                                {
                                    id = (int)q.countryId,
                                    nameOfcountry = q.countryName
                                },
                                oReferral = new ReferralLookup()
                                {
                                    id = (int) q.referralId,
                                    sourceOfReferral = q.nameOfreferral
                                }
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
                            join cty in config.TCities on tc.ClientCityId equals cty.Id
                            join rf in config.Tclientreferralsources on tc.ReferralId equals rf.Id

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
                                address = string.Format("{0} {1} {2} {3}", tca.ClientAddr1, tca.ClientAddr2, tca.ClientAddr3, tca.ClientAddr4),

                                cityid = cty.Id,
                                cityName = cty.CityName,
                                referralId = tc.ReferralId,
                                nameOfreferral = rf.ReferralSource
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
                                mobileNo = q.mobileNo,

                                oCity = new CityLookup()
                                {
                                    id = q.cityid,
                                    nameOfcity = q.cityName
                                },
                                oReferral = new ReferralLookup()
                                {
                                    id = (int) q.referralId,
                                    sourceOfReferral = q.nameOfreferral
                                }
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
                            join cty in config.TCities on tc.ClientCityId equals cty.Id
                            join rf in config.Tclientreferralsources on tc.ReferralId equals rf.Id

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
                                address = string.Format("{0} {1} {2} {3}", tca.ClientAddr1, tca.ClientAddr2, tca.ClientAddr3, tca.ClientAddr4),

                                cityid = cty.Id,
                                cityName = cty.CityName,
                                referralId = tc.ReferralId,
                                nameOfreferral = rf.ReferralSource
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
                                mobileNo = q.mobileNo,

                                oCity = new CityLookup()
                                {
                                    id = q.cityid,
                                    nameOfcity = q.cityName
                                },
                                oReferral = new ReferralLookup()
                                {
                                    id = (int) q.referralId,
                                    sourceOfReferral = q.nameOfreferral
                                }
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
                            join cty in config.TCities on tc.ClientCityId equals cty.Id
                            join rf in config.Tclientreferralsources on tc.ReferralId equals rf.Id

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
                                address = string.Format("{0} {1} {2} {3}", tca.ClientAddr1, tca.ClientAddr2, tca.ClientAddr3, tca.ClientAddr4),

                                cityid = cty.Id,
                                cityName= cty.CityName,
                                referralId = tc.ReferralId,
                                nameOfreferral = rf.ReferralSource
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
                                mobileNo = q.mobileNo,

                                oCity = new CityLookup()
                                {
                                    id = q.cityid,
                                    nameOfcity = q.cityName
                                },
                                oReferral = new ReferralLookup()
                                {
                                    id = (int) q.referralId,
                                    sourceOfReferral = q.nameOfreferral
                                }
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
                            join cty in config.TCities on tc.ClientCityId equals cty.Id
                            join rf in config.Tclientreferralsources on tc.ReferralId equals rf.Id

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
                                address = string.Format("{0} {1} {2} {3}", tca.ClientAddr1, tca.ClientAddr2, tca.ClientAddr3, tca.ClientAddr4),

                                cityid = cty.Id,
                                cityName = cty.CityName,
                                referralId = tc.ReferralId,
                                nameOfreferral = rf.ReferralSource
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
                                mobileNo = q.mobileNo,

                                oCity =new CityLookup()
                                {
                                    id = q.cityid,
                                    nameOfcity = q.cityName
                                },
                                oReferral = new ReferralLookup()
                                {
                                    id = (int) q.referralId,
                                    sourceOfReferral = q.nameOfreferral
                                }
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
                                        join cty in config.TCities on tc.ClientCityId equals cty.Id
                                        join rf in config.Tclientreferralsources on tc.ReferralId equals rf.Id

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
                                            address = string.Format("{0} {1} {2} {3}", tca.ClientAddr1, tca.ClientAddr2, tca.ClientAddr3, tca.ClientAddr4),
                                        
                                            cityid = cty.Id,
                                            cityName = cty.CityName,
                                            referralId = tc.ReferralId,
                                            nameOfreferral = rf.ReferralSource
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
                                mobileNo = q.mobileNo,
                                oCity = new CityLookup()
                                {
                                    id = q.cityid,
                                    nameOfcity = q.cityName
                                },
                                oReferral = new ReferralLookup()
                                {
                                    id = (int) q.referralId,
                                    sourceOfReferral = q.nameOfreferral
                                }
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
            Helper helper = new Helper();

            try
            {

                //if (param.searchCriteria == @"ID")
                //{
                //    var Query = (from tc in config.TClients
                //                 join ct in config.TClientTypes on tc.ClientTypeId equals ct.Id
                //                 join cmp in config.Tcompanies on tc.AssociatedCompanyId equals cmp.CompanyId
                //                 join chn in config.TChannelTypes on tc.ChannelTypeId equals chn.ChannelTypeId
                //                 join cty in config.TCities on tc.ClientCityId equals cty.Id
                //                 join cntr in config.TCountryLookups on tc.ClientCountryId equals cntr.CountryId
                //                 join rf in config.Tclientreferralsources on tc.ReferralId equals rf.Id
                //                 join crt in config.Tusrs on tc.CreatedBy equals crt.UsrId
                //                 join usr in config.Tusrs on tc.LastModifiedBy equals usr.UsrId
                //                 where tc.Id == int.Parse(param.stringValue)

                //                 select new
                //                 {
                //                     uniqueID = tc.Id,
                //                     clientType = ct.Describ,
                //                     clientTypeId = ct.Id,
                //                     associatedCompany = cmp.Company,
                //                     associatedCompanyId = tc.AssociatedCompanyId,
                //                     channelType = chn.Channel,
                //                     channelTypeId = tc.ChannelTypeId,
                //                     firstname = tc.Firstname,
                //                     middlenames = tc.Middlenames,
                //                     surname = tc.Surname,
                //                     clientBusiness = tc.ClientBusinessName,
                //                     mobileNo = tc.MobileNo,
                //                     whatsappNo = tc.WhatsappNo,
                //                     homeTel = tc.HomeTelephone,
                //                     workTel = tc.WorkTelephone,
                //                     emailAddr = tc.ClientEmailAddr,
                //                     emailAddr2 = tc.ClientEmailAddr2,
                //                     accNo = tc.ClientAccNo,
                //                     cityId = tc.ClientCityId,
                //                     city = cty.CityName,
                //                     countryId = tc.ClientCountryId,
                //                     nameOfcountry = cntr.CountryName,
                //                     postCode = tc.ClientPostCode,
                //                     referralId = tc.ReferralId,
                //                     referral = rf.ReferralSource,
                //                     collectionInstruction = tc.CollectionInstruction
                //                 });

                //    var obj = await Query.FirstOrDefaultAsync().ConfigureAwait(false);
                //    if ((obj != null) && (obj.clientTypeId == 1))
                //    {
                //        dta = new IndividualCustomerLookup()
                //        {
                //            id = obj.uniqueID,
                //            oClientType = new ClientTypeLookup()
                //            {
                //                id = obj.clientTypeId,
                //                clientTypeDescrib = obj.clientType
                //            },
                //            oCompany = new CompanyLookup()
                //            {
                //                id = (int)obj.associatedCompanyId,
                //                nameOfcompany = obj.associatedCompany
                //            },
                //            oChannelType = new ChannelTypeLookup()
                //            {
                //                id = (int)obj.channelTypeId,
                //                nameOfchannel = obj.channelType
                //            },
                //            firstname = obj.firstname.Trim().ToUpper(),
                //            middlenames = obj.middlenames.Trim().ToUpper(),
                //            surname = obj.surname.Trim().ToUpper(),
                //            mobileNo = obj.mobileNo.Trim(),
                //            whatsappNo = obj.whatsappNo.Trim(),
                //            homeTelephone = obj.homeTel.Trim(),
                //            workTelephone = obj.workTel.Trim(),
                //            clientEmail = obj.emailAddr.Trim(),
                //            clientEmail2 = obj.emailAddr2.Trim(),
                //            oCity = new CityLookup()
                //            {
                //                id = (int)obj.cityId,
                //                nameOfcity = obj.city.Trim().ToUpper()
                //            },
                //            oCountry = new CountryLookup()
                //            {
                //                id = (int)obj.countryId,
                //                nameOfcountry = obj.nameOfcountry.Trim().ToUpper()
                //            },
                //            postCode = obj.postCode.Trim().ToUpper(),
                //            oReferral = new ReferralLookup()
                //            {
                //                id = (int)obj.referralId,
                //                sourceOfReferral = obj.referral.Trim().ToUpper()
                //            },
                //            collectionInstruction = obj.collectionInstruction,
                //            accountNo = obj.accNo != string.Empty ? obj.accNo.Trim().ToUpper() : string.Empty,
                //            clientBusiness = obj.clientBusiness == string.Empty ? string.Empty : obj.clientBusiness.Trim().ToUpper(),

                //            //add the address via a different object call
                //            oAddress = await helper.getClientAddressAsync(obj.uniqueID)
                //        };
                //    }

                //    if ((obj != null) && (obj.clientTypeId == 2))
                //    {
                //        dta = new CorporateCustomerLookup() {
                //            id = obj.uniqueID,
                //            oClientType = new ClientTypeLookup()
                //            {
                //                id = obj.clientTypeId,
                //                clientTypeDescrib = obj.clientType.Trim().ToUpper()
                //            },
                //            oCompany = new CompanyLookup()
                //            {
                //                id = (int)obj.associatedCompanyId,
                //                nameOfcompany = obj.associatedCompany.Trim().ToUpper()
                //            },
                //            oChannelType = new ChannelTypeLookup()
                //            {
                //                id = (int)obj.channelTypeId,
                //                nameOfchannel = obj.channelType.Trim().ToUpper()
                //            },
                //            clientBusiness = obj.clientBusiness == string.Empty ? string.Empty : obj.clientBusiness.Trim().ToUpper(),
                //            mobileNo = obj.mobileNo.Trim(),
                //            whatsappNo = obj.whatsappNo.Trim(),
                //            homeTelephone = obj.homeTel.Trim(),
                //            workTelephone = obj.workTel.Trim(),
                //            clientEmail = obj.emailAddr.Trim(),
                //            clientEmail2 = obj.emailAddr2.Trim(),
                //            oCity = new CityLookup()
                //            {
                //                id = (int)obj.cityId,
                //                nameOfcity = obj.city.Trim().ToUpper()
                //            },
                //            oCountry = new CountryLookup()
                //            {
                //                id = (int)obj.countryId,
                //                nameOfcountry = obj.nameOfcountry.Trim().ToUpper()
                //            },
                //            postCode = obj.postCode.Trim().ToUpper(),
                //            oReferral = new ReferralLookup()
                //            {
                //                id = (int)obj.referralId,
                //                sourceOfReferral = obj.referral.Trim().ToUpper()
                //            },
                //            collectionInstruction = obj.collectionInstruction,
                //            accountNo = obj.accNo == string.Empty ? string.Empty : obj.accNo.Trim().ToUpper(),

                //            oAddress = await helper.getClientAddressAsync(obj.uniqueID)
                //        };
                //    }

                //    response = new DefaultAPIResponse() { 
                //        status = true,
                //        message = @"success",
                //        data = dta
                //    };

                //}

                dta = await helper.getClientUsingParamAsync(param);
                response = new DefaultAPIResponse() {
                    status = dta != null ? true : false,
                    message = dta != null ? @"success" : @"An error occured. Please see the administrator",
                    data = dta
                };
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

        public async Task<DefaultAPIResponse> SaveCorporateClientRecordAsync(CorporateCustomerLookup payLoad)
        {
            //saves corporate client record
            DefaultAPIResponse response = null;
            clientCreatedRecord cRekord = null;

            try
            {
                Helper helper = new Helper();
                cRekord = await helper.createCorporateClientRecordAsync(payLoad);

                return response = new DefaultAPIResponse()
                {
                    status = true,
                    message = $"Customer record {payLoad.clientBusiness.ToUpper()} created successfully: Account number is {cRekord.acctNo}",
                    data = payLoad
                };
            }
            catch (Exception x)
            {
                return response = new DefaultAPIResponse()
                {
                    status = false,
                    message = $"error: {x.Message}"
                };
            }
        }

        public async Task<DefaultAPIResponse> SaveIndividualClientRecordAsync(IndividualCustomerLookup payLoad)
        {
            DefaultAPIResponse response = null;
            clientCreatedRecord rekord = null;

            try
            {
                Helper helper = new Helper();

                rekord = await helper.createIndividualClientRecordAsync(payLoad);

                return response = new DefaultAPIResponse()
                {
                    status = true,
                    message = $"Customer record created successfully: Account Number is {rekord.acctNo}",
                    //data = new { rekord.clientObj, tad }
                    data = payLoad 
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

        public async Task<DefaultAPIResponse> SaveOnlineCustomerAsync(clsCustomer payLoad)
        {
            DefaultAPIResponse rsp = null;
            clsCustomer cc = null;
            bool bln = false;

            try
            {
                Helper helper = new Helper();

                //email validation
                if (await helper.doesEmailExistAsync(@"main", payLoad.clientEmail))
                {
                    return rsp = new DefaultAPIResponse() { 
                        status = false,
                        message = $"Customer's emailaddress {payLoad.clientEmail} is already on file and cannot be used for registration",
                        data = payLoad 
                    };
                }

                if (await helper.doesEmailExistAsync(@"alternate", payLoad.clientEmail2))
                {
                    return rsp = new DefaultAPIResponse()
                    {
                        status = false,
                        message = $"Customer's alternate emailaddress {payLoad.clientEmail2} is already on file and cannot be used for registration",
                        data = payLoad
                    };
                }

                //phone validation
                //mobile
                if (await helper.doesPhoneExistAsync(@"mobile", payLoad.mobileNo))
                {
                    return rsp = new DefaultAPIResponse()
                    {
                        status = false,
                        message = $"Customer's mobile number {payLoad.mobileNo} is already on file and cannot be used for registration",
                        data = payLoad
                    };
                }

                //whatsapp
                if (await helper.doesPhoneExistAsync(@"whatsapp", payLoad.whatsappNo))
                {
                    return rsp = new DefaultAPIResponse()
                    {
                        status = false,
                        message = $"Customer's whatsapp number {payLoad.whatsappNo} is already on file and cannot be used for registration",
                        data = payLoad
                    };
                }

                //worktelephone
                if (await helper.doesPhoneExistAsync(@"workTelephone", payLoad.workTelephone))
                {
                    return rsp = new DefaultAPIResponse()
                    {
                        status = false,
                        message = $"Customer's work telephone number {payLoad.workTelephone} is already on file and cannot be used for registration",
                        data = payLoad
                    };
                }

                var r = await helper.createOnlineCustomerAsync(payLoad);
                if (r != null)
                {
                    cc = (clsCustomer)r;
                    return rsp = new DefaultAPIResponse()
                    {
                        status = r != null ? true : false,
                        message = r != null ? $"Dear {cc.surname} {cc.firstname} {cc.clientBusiness} <br> Thank you for registering on swiftship. Your temporary password is <b>{cc.pwd}</b> <br> Please go to the <a href={cc.verificationLink}>Verification Page</a> to verify your login <br><br> Thank you. <br>SWIFT-Ship" : @"failed",
                        data = r
                    };
                }
                else
                {
                    return rsp = new DefaultAPIResponse() { 
                        status = false,
                        message = $"A user account could not be created for {payLoad.surname} {payLoad.firstname} {payLoad.clientBusiness} <br> Please contact the Administrator"
                    };
                }

                //return rsp;
            }
            catch(Exception x)
            {
                return rsp = new DefaultAPIResponse()
                {
                    status = false,
                    message = $"error message: {x.Message}"
                };
            }
        }

        public async Task<DefaultAPIResponse> VerifyOnlineCustomerAsync(string user, string verificationCode)
        {
            //verifying online customer
            DefaultAPIResponse rsp = null;
            bool bln = false;

            try
            {
                Helper helper = new Helper();

                if (await helper.VerifyOnlineCustomerAsync(user, verificationCode))
                {
                    bln = true;
                    rsp = new DefaultAPIResponse()
                    {
                        status = bln,
                        message = bln ? @"Customer verified. It is advised to change password immediately on logging in" : @"failed",
                        data = new SearchParam() { searchCriteria = user, stringValue = verificationCode }
                    };
                }
                else
                {
                    rsp = new DefaultAPIResponse()
                    {
                        status = bln,
                        message = @"Customer record does not exist. Please see the Administrator",
                        data = new SearchParam() { searchCriteria = user, stringValue = verificationCode }
                    };
                }

                return rsp;
            }
            catch(Exception x)
            {
                return rsp = new DefaultAPIResponse()
                {
                    status = false,
                    message = $"error: {x.Message}"
                };
            }
        }

        public async Task<DefaultAPIResponse> UpdateClientInformationAsync(IndividualCustomerLookup payLoad)
        {
            //updates client information
            DefaultAPIResponse response = null;
            var dd = new IndividualCustomerLookup();
            TClient d = null;
            TClientAddress dc = null;

            try
            {
                /*
                var Query = (from tc in config.TClients
                             join ct in config.TClientTypes on tc.ClientTypeId equals ct.Id
                             join cmp in config.Tcompanies on tc.AssociatedCompanyId equals cmp.CompanyId
                             join chn in config.TChannelTypes on tc.ChannelTypeId equals chn.ChannelTypeId
                             join cty in config.TCities on tc.ClientCityId equals cty.Id
                             join cntr in config.TCountryLookups on tc.ClientCountryId equals cntr.CountryId
                             join rf in config.Tclientreferralsources on tc.ReferralId equals rf.Id
                             join crt in config.Tusrs on tc.CreatedBy equals crt.UsrId
                             join usr in config.Tusrs on tc.LastModifiedBy equals usr.UsrId
                             join addr in config.TClientAddresses on tc.Id equals addr.ClientId
                             where tc.Id == payLoad.id

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
                                 collectionInstruction = tc.CollectionInstruction,

                                 createdBy = tc.CreatedBy,
                                 modifiedBy = tc.LastModifiedBy,
                                 shipperStatus = tc.IsShipper,
                                 password = tc.ClientPassword,
                                 canLogin= tc.CanLogin,

                                 address1 = addr.ClientAddr1 != null ? addr.ClientAddr1: string.Empty,
                                 address2 = addr.ClientAddr2 != null ? addr.ClientAddr2 : string.Empty,
                                 address3 = addr.ClientAddr3 != null ? addr.ClientAddr3 : string.Empty,
                             });
                
                var clientData = await Query.ToListAsync().ConfigureAwait(false);


                dd = clientData.Select(q => new IndividualCustomerLookup()
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
                    clientEmail = q.emailAddr.Length > 0 ? q.emailAddr : string.Empty,
                    clientEmail2 = q.emailAddr.Length > 0 ? q.emailAddr : string.Empty,
                    clientPassword = q.password,
                    canLogin = (bool)q.canLogin,
                    clientBusiness = q.clientBusiness,
                    collectionInstruction = q.collectionInstruction,

                    firstname = q.firstname.Length > 0 ? q.firstname : string.Empty,
                    middlenames = q.middlenames.Length > 0 ? q.middlenames : string.Empty,
                    surname = q.surname.Length > 0 ? q.surname : string.Empty,
                    
                    oAddress = new AddressLookup()
                    {
                        address1 = q.address1,
                        address2 = q.address2,
                        address3 = q.address3
                    },
                    createdBy = (int)q.createdBy,
                    modifiedBy = (int)q.modifiedBy,
                    shipper = (bool)q.shipperStatus

                }).FirstOrDefault();

                */
                //update

                d = await config.TClients.Where(x => x.Id == payLoad.id).FirstOrDefaultAsync();

                //d.Id = payLoad.id;
                d.ClientTypeId = payLoad.oClientType.id;
                d.AssociatedCompanyId = payLoad.oCompany.id;
                //d.oChannelType.id = payLoad.oChannelType.id;
                d.Firstname = payLoad.firstname.Length > 0 ? payLoad.firstname.ToUpper() : string.Empty;
                d.Middlenames = payLoad.middlenames.Length > 0 ? payLoad.middlenames.ToUpper() : string.Empty;
                d.Surname = payLoad.surname.Length > 0 ? payLoad.surname.ToUpper() : string.Empty;
                d.ClientBusinessName = payLoad.clientBusiness.Length > 0 ? payLoad.clientBusiness.ToUpper() : string.Empty;
                d.MobileNo = payLoad.mobileNo.Length > 0 ? payLoad.mobileNo : string.Empty;
                d.WhatsappNo = payLoad.whatsappNo.Length > 0 ? payLoad.whatsappNo : string.Empty;
                d.HomeTelephone = payLoad.homeTelephone.Length > 0 ? payLoad.homeTelephone : string.Empty;
                d.WorkTelephone = payLoad.workTelephone.Length > 0 ? payLoad.workTelephone : string.Empty;
                d.ClientEmailAddr = payLoad.clientEmail.Length > 0 ? payLoad.clientEmail : string.Empty;
                d.ClientEmailAddr2 = payLoad.clientEmail2.Length > 0 ? payLoad.clientEmail2 : string.Empty;
                d.ClientCityId = payLoad.oCity.id;
                d.ClientCountryId = payLoad.oCountry.id;
                d.ClientPostCode = payLoad.postCode.ToUpper();
                d.ReferralId = payLoad.oReferral.id;
                d.CollectionInstruction = payLoad.collectionInstruction.Length > 0 ? payLoad.collectionInstruction : string.Empty;
                d.CreatedBy = payLoad.createdBy;
                d.LastModifiedBy = payLoad.modifiedBy;
                d.IsShipper = payLoad.shipper;
                d.ClientAccNo = payLoad.accountNo;

                await config.SaveChangesAsync();

                return response = new DefaultAPIResponse() {
                    status = true,
                    message = @"success",
                    data = d
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

        public async Task<DefaultAPIResponse> UpdateClientAddressAsync(IndividualCustomerLookup payLoad)
        {
            //update client address
            DefaultAPIResponse response = null;

            try
            {
                var dc = await config.TClientAddresses.Where(c => c.ClientId == payLoad.id).FirstOrDefaultAsync();

                if (dc != null)
                {
                    dc.ClientAddr1 = payLoad.oAddress.address1.Length > 0 ? payLoad.oAddress.address1.Trim().ToUpper() : string.Empty;
                    dc.ClientAddr2 = payLoad.oAddress.address1.Length > 0 ? payLoad.oAddress.address2.Trim().ToUpper() : string.Empty;
                    dc.ClientAddr3 = payLoad.oAddress.address1.Length > 0 ? payLoad.oAddress.address3.Trim().ToUpper() : string.Empty;

                    await config.SaveChangesAsync();

                    response = new DefaultAPIResponse()
                    {
                        status = true,
                        message = @"Client Addresses saved",
                        data = dc
                    };
                }
                else
                {
                    response = new DefaultAPIResponse()
                    {
                        status = false,
                        message = @"No data fetched for addresses"
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

    }
}
