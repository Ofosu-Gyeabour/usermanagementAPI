#nullable disable

using System.Runtime.CompilerServices;
using UserManagementAPI.Models;
using UserManagementAPI.Resources.Interfaces;
using UserManagementAPI.utils;
using Xero.NetStandard.OAuth2.Api;
using Xero.NetStandard.OAuth2.Client;
using Xero.NetStandard.OAuth2.Config;
using Xero.NetStandard.OAuth2.Model.Accounting;
using Xero.NetStandard.OAuth2.Model.Identity;

namespace UserManagementAPI.Resources.Implementations
{
    public class XeroService : IXeroService
    {
        #region API
        public async Task<XeroAPIResponse> CreateContactAsync(clsXeroContact payLoad)
        {
            XeroAPIResponse response = null;
            int attempts = 3;
            int errorAttempt = 0;

            try
            {
                Helper helper = new Helper();
                var xDbParams = await helper.getXeroConfigAsync();
                response = await helper.CreateContactAsync(payLoad,xDbParams);

                if (response.Message.Contains("Unauthorized"))
                {
                    errorAttempt += 1;  //set error count

                    while(errorAttempt < attempts)
                    {
                        //refresh the token
                        XeroConfiguration xconfig = new XeroConfiguration()
                        {
                            ClientId = xDbParams.ClientId,
                            ClientSecret = xDbParams.ClientSecret,
                            CallbackUri = new Uri(xDbParams.ReDirectUri),
                            
                            Scope = xDbParams.Scopes
                        };

                        var tok = await helper.GetXeroTokenAsync(xconfig,xDbParams);

                        //create txeroconfig and try again
                        TXeroConfig x = new TXeroConfig() { AccessToken = tok.AccessToken, RefreshToken = tok.RefreshToken };
                        response = await helper.CreateContactAsync(payLoad,x);

                        errorAttempt += 1;
                    }
                    
                }

                return response;
            }
            catch(Exception x)
            {
                return response = new XeroAPIResponse()
                {
                    Status = @"Error",
                    Message = $"error: {x.Message}"
                };
            }
        }
        public async Task<XeroAPIResponse> createXeroInvoiceAsync(clsXeroInvoice payLoad)
        {
            //TODO: creates an invoice for the xero accounting package
            XeroAPIResponse r = null;


            try
            {
                Helper helper = new Helper();
                var refreshO = await helper.RefreshTokenAsync();
                await helper.updateXeroConfigTokensInDb(refreshO);
                r = await helper.CreateInvoiceAsync(payLoad);

                var xDbParams = await helper.getXeroConfigAsync();

                var contact = new Contact()
                {
                    //ContactID = new Guid(payLoad.Contact.ContactID),
                    Name = payLoad.Contact.Name
                };

                //create line item list
                List<LineItem> lines = new List<LineItem>();
                foreach (var pl in payLoad.LineItems)
                {
                    lines.Add(new LineItem()
                    {
                        Description = pl.Description,
                        Quantity = pl.Quantity,
                        UnitAmount = pl.UnitAmount,
                        AccountCode = pl.AccountCode
                    });
                }

                //create invoice
                var invoice = new Invoice()
                {
                    Type = payLoad.Type == @"ACCREC" ? Invoice.TypeEnum.ACCREC : Invoice.TypeEnum.ACCPAY,
                    Contact = contact,
                    Date = payLoad.Date,
                    DueDate = payLoad.DueDate,
                    LineItems = lines
                };

                //create invoice list
                var invoiceList = new List<Invoice>();
                invoiceList.Add(invoice);

                var invoices = new Invoices();
                invoices._Invoices = invoiceList;

                var accountingApi = new AccountingApi();
                //var response = await accountingApi.GetInvoicesAsync(XeroConfigObject.ACCESS_TOKEN, XeroConfigObject.XERO_TENANT_ID);

                //create invoice in xero accounting app
                var response = await accountingApi.CreateInvoicesAsync(xDbParams.AccessToken, xDbParams.XeroTenantId, invoices);

                return r = new XeroAPIResponse()
                {
                    Message = @"Ok"
                };
            }
            catch (Exception x)
            {
                return r = new XeroAPIResponse()
                {
                    Status = @"Error",
                    Message = $"error: {x.Message}"
                };
            }
        }

        #endregion

        #region SDK

        public async Task<XeroAPIResponse> CreateContactSDKAsync(clsXeroContact payLoad)
        {
            //uses the sdk to save contact details
            XeroAPIResponse response = new XeroAPIResponse();
            var summarizeErrors = true;
            //var idempotencyKey = "x39449";

            try
            {
                Helper helper = new Helper();
                //var xdbParams = await helper.getXeroConfigAsync();
                var refreshO = await helper.RefreshTokenAsync();
                await helper.updateXeroConfigTokensInDb(refreshO);

                var apiInstance = new AccountingApi();

                var mobile = new Phone();
                mobile.PhoneNumber = payLoad.mobileNo.Length > 3 ? payLoad.mobileNo.Substring(3, payLoad.mobileNo.Length - 3) : string.Empty;
                mobile.PhoneCountryCode = payLoad.mobileNo.Length > 3 ? payLoad.mobileNo.Substring(0, 3) : string.Empty;
                //mobile.PhoneAreaCode = payLoad.mobileNo.Substring(0, 3);
                mobile.PhoneType = Phone.PhoneTypeEnum.MOBILE;

                var whatsapp = new Phone();
                whatsapp.PhoneNumber = payLoad.whatsappNo.Length > 3 ? payLoad.whatsappNo.Substring(3, payLoad.whatsappNo.Length - 3) : string.Empty;
                mobile.PhoneCountryCode = payLoad.whatsappNo.Length > 3 ? payLoad.whatsappNo.Substring(0, 3) : string.Empty;
                whatsapp.PhoneType = Phone.PhoneTypeEnum.OFFICE;

                var workTel = new Phone();
                workTel.PhoneNumber = payLoad.workTelephone.Length > 3 ? payLoad.workTelephone.Substring(3, payLoad.workTelephone.Length - 3) : string.Empty;
                workTel.PhoneCountryCode = payLoad.workTelephone.Length > 3 ? payLoad.workTelephone.Substring(0, 3) : string.Empty;
                workTel.PhoneType = Phone.PhoneTypeEnum.DDI;

                var phoneList = new List<Phone>();
                phoneList.Add(mobile);
                phoneList.Add(whatsapp);
                phoneList.Add(workTel);
                
                Address address = new Address();
                address.AddressLine1 = payLoad.AddressLine1;
                address.AddressLine2 = payLoad.AddressLine2;
                address.AddressLine3 = payLoad.AddressLine3;
                
                address.City = payLoad.City;
                address.Country = payLoad.Country;
                address.PostalCode = payLoad.PostalCode;

                var addressList = new List<Address>();
                addressList.Add(address);

                var contactPersons = new List<ContactPerson>();

                foreach(var item in payLoad.ContactPersons)
                {
                    ContactPerson cPerson = new ContactPerson();
                    cPerson.FirstName = item.FirstName;
                    cPerson.LastName = item.LastName;
                    cPerson.EmailAddress = item.EmailAddress;
                    cPerson.IncludeInEmails = item.IncludeInEmails == @"true" ? true: false;

                    contactPersons.Add(cPerson);
                }

                var contact = new Contact();
                contact.Name = payLoad.BusinessName != null ? $"{payLoad.BusinessName}-{payLoad.AccountNumber}" : $"{payLoad.FirstName} {payLoad.Middlename} {payLoad.LastName}-{payLoad.AccountNumber}";
                contact.FirstName = payLoad.FirstName.Trim();
                contact.LastName = payLoad.Middlename != null ? $"{payLoad.Middlename} {payLoad.LastName}" : payLoad.LastName;
                contact.EmailAddress = payLoad.EmailAddress.Trim();
                contact.ContactStatus = Contact.ContactStatusEnum.ACTIVE;
                
                contact.AccountNumber = payLoad.AccountNumber;
                contact.Addresses = addressList;
                contact.Phones = phoneList;
                contact.IsCustomer = true;
                contact.DefaultCurrency = payLoad.DefaultCurrency == @"GBP" ? CurrencyCode.GBP : CurrencyCode.EUR;
                

                var contacts = new Contacts();
                var contactList = new List<Contact>();
                contactList.Add(contact);
                contacts._Contacts = contactList;
                
                try
                {
                    var result = await apiInstance.CreateContactsAsync(refreshO.access_token,XeroConfigObject.XERO_TENANT, contacts, summarizeErrors);
                    //response.data = result;
                    return response = new XeroAPIResponse()
                    {
                        Status = @"Ok",
                        data = result,
                        Message = result._Contacts[0].ContactID.ToString()
                    };
                }
                catch(Exception e)
                {
                    Console.WriteLine("Exception when calling apiInstance.CreateContacts: " + e.Message);
                    return response = new XeroAPIResponse()
                    {
                        Status = @"Error"
                    };
                }
            }
            catch(Exception x)
            {
                return response = new XeroAPIResponse()
                {
                    Status = @"Error",
                    Message = $"error: {x.Message}"
                };
            }
        }

        #endregion

    }
}
