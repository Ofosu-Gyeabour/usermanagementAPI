#nullable disable
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using UserManagementAPI.Resources.Implementations;

namespace UserManagementAPI.POCOs
{
    public record ConsolidatorRecord
    {
        public int recordId { get; set; } = 0;
        public int id { get; set; }
        public string businessName { get; set; } = string.Empty;
        public string? mobileNo { get; set; } = string.Empty;
        public string? whatsappNo { get; set; } = string.Empty;
        public CityLookup? oCity { get; set; }
        public CountryLookup? oCountry { get; set; }
        public string? emailAddress1 { get; set; } = string.Empty;
        public string? emailAddress2 { get; set; } = string.Empty;
        public string? clientAddress { get; set; } = string.Empty;

        public int consolidatorId { get; set; }
        public string consolidatorName { get; set; }
    }

    public record CorporateConsolidatorClient
    {
        public int id { get; set; }
        public string? businessName { get; set; }
        public string? postCode { get; set; } = string.Empty;
        public string? address1 { get; set; } = string.Empty;
        public string? address2 { get; set; } = string.Empty;
        public string? address3 { get; set; } = string.Empty;
        public string? mobileNo { get; set; } = string.Empty;
        public string? whatsappNo { get; set; } = string.Empty;
        public string? primaryEmail { get; set; } = string.Empty;
        public string? secondaryEmail { get; set; } = string.Empty;
        public CountryLookup? oCountry { get; set; }
        public CityLookup? oCity { get; set; }
        public int consolidatorID { get; set; }
        public bool? isUK { get; set; } = false;
        public int? recordInputter { get; set; }
    } 

    public record IndividualConsolidatorClient : CorporateConsolidatorClient
    {
        public string? surname { get; set; } = string.Empty;
        public string? firstname { get; set; } = string.Empty;
        public string? othernames { get; set; } = string.Empty;
    }

    public class clsConsolidator
    {
        swContext config;
        public clsConsolidator()
        {
            config = new swContext();
        }

        #region Methods

        public async Task<ConsolidatorRecord> validateConsolidatorCredentialsAsync(UserInfo userinfo)
        {
            //method is responsible for authenticating the consolidator
            ConsolidatorRecord consolidatorRecord = null;

            try
            {
                UserService usrservice = new UserService();
                var enc = await usrservice.GetMD5EncryptedPasswordAsync(new SingleParam() { stringValue = userinfo.password});

                var Q = (from c in config.TClients
                         join cty in config.TCities on c.ClientCityId equals cty.Id
                         join ctn in config.TCountryLookups on c.ClientCountryId equals ctn.CountryId
                         join caddr in config.TClientAddresses on c.Id equals caddr.ClientId
                         join consol in config.TConsolUsrs on c.Id equals consol.ConsolId

                         where consol.Usrname == userinfo.username
                         && consol.Usrpwd == enc.data.ToString()

                         select new
                         {
                             recordId = consol.Id,
                             id = c.Id,
                             businessName = c.ClientBusinessName,
                             mobile = c.MobileNo != null ? c.MobileNo.ToString() : string.Empty,
                             whatsapp = c.WhatsappNo != null ? c.WhatsappNo.ToString() : string.Empty,
                             town = cty.CityName,
                             townId = c.ClientCityId,
                             countryName = ctn.CountryName,
                             countryId = c.ClientCountryId,
                             email = consol.Usrname, //c.ClientEmailAddr,
                             email2 = c.ClientEmailAddr2 != null ? c.ClientEmailAddr2 : string.Empty,
                             address = $"{caddr.ClientAddr1} {caddr.ClientAddr2} {caddr.ClientAddr3}"
                         });

                var QList = await Q.ToListAsync().ConfigureAwait(false);

                return consolidatorRecord = QList.Select(x => new ConsolidatorRecord()
                {
                    recordId = x.recordId,
                    id = x.id,
                    businessName = x.businessName,
                    mobileNo = x.mobile,
                    whatsappNo = x.whatsapp,
                    oCity = new CityLookup()
                    {
                        id = (int) x.townId,
                        nameOfcity = x.town
                    },
                    oCountry = new CountryLookup()
                    {
                        id = (int)x.countryId,
                        nameOfcountry = x.countryName
                    },
                    emailAddress1 = x.email,
                    emailAddress2 = x.email2,
                    clientAddress = x.address
                }).FirstOrDefault();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> createUserAccountAsync(consolUserRecord cu)
        {
            bool bln = false;
            TConsolUsr consolusr = null;

            try
            {
                UserService usrservice = new UserService();
                var enc = await usrservice.GetMD5EncryptedPasswordAsync(new SingleParam() { stringValue = cu.userCredentials.password });

                consolusr = new TConsolUsr()
                {
                    Sname = cu.sname != null ? cu.sname : string.Empty,
                    Fname = cu.fname != null ? cu.fname : string.Empty,
                    Onames = cu.othernames != null ? cu.othernames : string.Empty,
                    ClientBusinessName = cu.clientBusinessName != null ? cu.clientBusinessName : string.Empty,
                    Usrname = cu.userCredentials.username,
                    Usrpwd = enc.data.ToString(),
                    ConsolId = cu.consolID,
                    IsAdmin = cu.isAdministrator,
                    IsLogged = cu.isLogged,
                    IsActive = cu.isActive,
                    LogAttempt = cu.loggedAttempt,
                    FailedAttempt = cu.failedAttempt,
                    CreatedBy = cu.createdBy
                };

                await config.AddAsync(consolusr);
                await config.SaveChangesAsync();

                return bln = true;
            }
            catch(Exception ex)
            {
                return bln;
            }
        }

        public async Task<bool> resetUserAccountAsync(consolUserRecord cu)
        {
            //TODO: resets a consolidator user credential
            bool bln = false;
            TConsolUsr tconsol = null;

            try
            {
                UserService usrservice = new UserService();
                var enc = await usrservice.GetMD5EncryptedPasswordAsync(new SingleParam() { stringValue = cu.userCredentials.password });

                using (config)
                {
                    tconsol = await config.TConsolUsrs.Where(x => x.Usrname == cu.userCredentials.username).FirstOrDefaultAsync();
                    if(tconsol != null)
                    {
                        tconsol.Usrpwd = enc.data.ToString();
                        await config.SaveChangesAsync();

                        bln = true;
                    }

                    return bln;
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<consolUserRecord>> getConsolidatorUserListAsync(int consolidatorID)
        {
            //TODO: gets the list of consolidator users
            List<consolUserRecord> userList = null;

            try
            {
                var q = (from c in config.TConsolUsrs
                         join u in config.Tusrs on c.CreatedBy equals u.UsrId
                         where c.ConsolId == consolidatorID

                         select new
                         {
                             id = c.Id,
                             sname = c.Sname,
                             fname = c.Fname,
                             othernames = c.Onames,
                             clientBusinessName = c.ClientBusinessName,
                             username = c.Usrname,
                             password = c.Usrpwd,
                             consolID = c.ConsolId,
                             isAdministrator = c.IsAdmin,
                             isLogged = c.IsLogged,
                             isActive = c.IsActive,
                             loggedAttempt = c.LogAttempt,
                             failedAttempt = c.FailedAttempt,
                             createdUserId = c.CreatedBy,
                             createdUserName = u.Usrname
                         });


                var qList = await q.ToListAsync().ConfigureAwait(false);

                return userList = qList.Select(a => new consolUserRecord()
                {
                    id = a.id,
                    sname = a.sname,
                    fname = a.fname,
                    othernames = a.othernames,
                    clientBusinessName = a.clientBusinessName,
                    userCredentials = new UserInfo()
                    {
                        username = a.username,
                        password = a.password
                    },
                    consolID = (int) a.consolID,
                    isAdministrator = (int) a.isAdministrator,
                    isLogged = (int) a.isLogged,
                    isActive = (int) a.isActive,
                    loggedAttempt = (int) a.loggedAttempt,
                    failedAttempt = (int) a.failedAttempt,
                    createdBy = (int) a.createdUserId,
                    createdByName = a.createdUserName
                }).ToList();
                
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<ConsolidatorRecord>> getConsolidatorCustomersAsync(int consolidatorID)
        {
            List<ConsolidatorRecord> result = null;

            try
            {
                var q = (from c in config.TConsols
                         join cnt in config.TCountryLookups on c.ClientCountryId equals cnt.CountryId
                         join cty in config.TCities on c.ClientCityId equals cty.Id
                         join conso in config.TClients on c.ConsolId equals conso.Id

                         where c.ConsolId == consolidatorID
                         select new
                         {
                             id = c.Id,
                             nameOrcompany = c.Sname == string.Empty? c.ClientBusinessName: $"{c.Fname} {c.Middlenames} {c.Sname}",
                             mobile = c.MobileNo,
                             whatsapp = c.WhatsappNo,
                             postcode = c.ClientPostCode,
                             email = c.ClientEmailAddr,
                             email2 = c.ClientEmailAddr2,
                             address = $"{c.ClientAddress1} {c.ClientAddress2} {c.ClientAddress3}",
                             countryId = c.ClientCountryId,
                             nameOfcountry = cnt.CountryName,
                             cityId = c.ClientCityId,
                             cityName = cty.CityName,
                             consolidatorId = c.ConsolId,
                             consolidatorName = conso.Surname == string.Empty ? conso.ClientBusinessName: $"{conso.Firstname} {conso.Middlenames} {conso.Surname}"
                         });

                var qList = await q.ToListAsync().ConfigureAwait(false);

                return result = qList.Select(x => new ConsolidatorRecord()
                {
                    id = x.id,
                    businessName = x.nameOrcompany,
                    mobileNo = x.mobile,
                    whatsappNo = x.whatsapp,
                    clientAddress = x.address,
                    emailAddress1 = x.email,
                    emailAddress2 = x.email2,
                    consolidatorId = (int)x.consolidatorId,
                    consolidatorName = x.consolidatorName,
                    oCountry = new CountryLookup()
                    {
                        id = (int)x.countryId,
                        nameOfcountry = x.nameOfcountry
                    },
                    oCity = new CityLookup()
                    {
                        id = (int)x.cityId,
                        nameOfcity = x.cityName
                    }
                }).ToList();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> createCorporateConsolidatorClientAsync(CorporateConsolidatorClient item)
        {
            //todo: creates a corporate consolidator client
            bool bln = false;

            try
            {
                TConsol obj = new TConsol()
                {
                    Sname = string.Empty,
                    Fname = string.Empty,
                    Middlenames = string.Empty,

                    ClientBusinessName = item.businessName,
                    ClientPostCode = item.postCode,
                    ClientAddress1 = item.address1,
                    ClientAddress2 = item.address2,
                    ClientAddress3 = item.address3,
                    MobileNo = item.mobileNo,
                    WhatsappNo = item.whatsappNo,
                    ClientEmailAddr = item.primaryEmail,
                    ClientEmailAddr2 = item.secondaryEmail,
                    ClientCountryId = item.oCountry.id,
                    ClientCityId = item.oCity.id,
                    ConsolId = item.consolidatorID,
                    IsUk = item.isUK,
                    Inputter = item.recordInputter
                };

                await config.AddAsync(obj);
                await config.SaveChangesAsync();

                return bln = true;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> createIndividualConsolidatorClientAsync(IndividualConsolidatorClient item)
        {
            //TODO: creates an individual consolidator's client
            bool bln = false;

            try
            {
                TConsol obj = new TConsol()
                {
                    Sname = item.surname,
                    Fname = item.firstname,
                    Middlenames = item.othernames,

                    ClientBusinessName = string.Empty,
                    ClientPostCode = item.postCode,
                    ClientAddress1 = item.address1,
                    ClientAddress2 = item.address2,
                    ClientAddress3 = item.address3,
                    MobileNo = item.mobileNo,
                    WhatsappNo = item.whatsappNo,
                    ClientEmailAddr = item.primaryEmail,
                    ClientEmailAddr2 = item.secondaryEmail,
                    ClientCountryId = item.oCountry.id,
                    ClientCityId = item.oCity.id,
                    ConsolId = item.consolidatorID,
                    IsUk = item.isUK,
                    Inputter = item.recordInputter
                };

                await config.AddAsync(obj);
                await config.SaveChangesAsync();

                return bln = true;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        private async Task<int> getItemIDAsync(string itemName, int country)
        {
            //TODO: gets item from the shipping list
            int returnedID = 0;

            try
            {
                var ob = await config.TShippingItems.Where(x => x.ItemName == itemName).Where(x => x.CountryId == country).FirstOrDefaultAsync();
                return returnedID = ob != null ? ob.Id : 10000;
            }
            catch(Exception ex)
            {
                return returnedID = 10000;
            }
        }

        private async Task<string> formatConsolOrderNumber(int id)
        {
            //TODO: creates a formatted order number for consolidator order
            try
            {
                var preFix = @"PCO";
                var suffix = string.Empty;

                var sId = id.ToString().Length;
                switch (id.ToString().Length)
                {
                    case 1:
                        suffix = string.Format("{0}{1}", @"000000", id.ToString());
                        break;
                    case 2:
                        suffix = string.Format("{0}{1}", @"00000", id.ToString());
                        break;
                    case 3:
                        suffix = string.Format("{0}{1}", @"0000", id.ToString());
                        break;
                    case 4:
                        suffix = string.Format("{0}{1}", @"000", id.ToString());
                        break;
                    case 5:
                        suffix = string.Format("{0}{1}", @"00", id.ToString());
                        break;
                    case 6:
                        suffix = string.Format("{0}{1}", @"0", id.ToString());
                        break;
                    default:
                        suffix = string.Format("{0}", id.ToString());
                        break;
                }

                return string.Format("{0}-{1}", preFix, suffix);
            }
            catch(Exception ex)
            {
                return string.Empty;
            }
        }

        public async Task<bool> CreateOrderAsync(clsConsolidatorOrder record)
        {
            bool bln = false;
            int consolOrderID = 0;
            int _Id = 0;

            try
            {
                using (var cfg = new swContext())
                {
                    _Id = config.TConsolOrders.Max(u => (int)u.Id);
                }
                
                using var transaction = config.Database.BeginTransaction();

                try
                {
                    TConsolOrder objOrder = new TConsolOrder()
                    {
                        ConsolId = record.consolID,
                        ArrivalcountryId = record.arrivalcountryId,
                        ArrivalPortId = record.arrivalPortId,
                        RecipientId = record.recipientId,
                        OrderInputBy = record.recordInputter,
                        OrderInputDate = DateTime.Now,
                        OrderNote = record.orderNote,
                        StatusId = 1,  //PENDING
                        OrderconvertedBy = null,
                        OrderconvertedDate = null,
                        ConsolOrderNo = await formatConsolOrderNumber(_Id + 1)
                    };

                    await config.AddAsync(objOrder);
                    await config.SaveChangesAsync();

                    consolOrderID = objOrder.Id;
                    record.orderNo = objOrder.ConsolOrderNo;

                    foreach (var item in record.items)
                    {
                        TConsolOrderItem objItem = new TConsolOrderItem()
                        {
                            ConsolOrderId = consolOrderID,
                            Qty = item.quantity,
                            ItemId = await getItemIDAsync(item.item,(int)record.arrivalcountryId),
                            Describ = item.description,
                            ItemWgt = item.itemWeight,
                            ItemVol = item.itemVolume,
                            Marks = item.marks,
                            Hscode = item.hsCode == null ? string.Empty : item.hsCode
                        };

                        await config.AddAsync(objItem);
                        await config.SaveChangesAsync();
                    }

                    await transaction.CommitAsync();
                    return bln = true;
                }
                catch(Exception x)
                {
                    await transaction.RollbackAsync();
                    return bln;
                }    
                
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        #endregion

    }

    public class clsConsolidatorOrder
    {
        public int id { get; set; }
        public int? arrivalcountryId { get; set; }
        public int? arrivalPortId { get; set; }
        public int? consolID { get; set; }
        public string? orderNote { get; set; } = string.Empty;
        public int? recipientId { get; set; }
        public int? recordInputter { get; set; }
        public string? orderNo { get; set; }
        public clsConsolidatorOrderItem[] items { get; set; }

    }

    public class clsConsolidatorOrderItem
    {
        public int id { get; set; }
        public int? consolOrderId { get; set; }

        public int quantity { get; set; }
        public string? item { get; set; }
        public int? itemId { get; set; }
        public string? description { get; set; }
        public decimal? itemWeight { get; set; }
        public decimal? itemVolume { get; set; }
        public string? marks { get; set; } = string.Empty;
        public string? hsCode { get; set; } = string.Empty;

        

    }

}
