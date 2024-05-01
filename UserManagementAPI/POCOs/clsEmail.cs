#nullable disable
using System.Net;
using System.Net.Mail;

namespace UserManagementAPI.POCOs
{
    public class clsEmail
    {
        public clsEmail()
        {

        }

        #region properties

        public int id { get; set; }
        public CompanyLookup oCompany { get; set; }
        public string msgFrom { get; set; } = string.Empty;
        public string msgTo { get; set; } = string.Empty;
        public string msgCC { get; set; } = string.Empty;
        public string msgBlindCC { get; set; } = string.Empty;
        public string host { get; set; } = string.Empty;
        public int? port { get; set; }
        public string userCredential { get; set; } = string.Empty;
        public string userPassword { get; set; } = string.Empty;


        public string logoUrl { get; set; } = string.Empty;
        public string bifaUrl { get; set; } = string.Empty;
        public string bifaSign { get; set; } = string.Empty;
        public string subjectColor { get; set; } = string.Empty;


        #endregion

        #region private methods

        private async Task<swContext> returnDatabaseContextAsync()
        {
            swContext context = null;

            try
            {
                context = new swContext();
                return context;
            }
            catch(Exception x)
            {
                return context;
            }
        }

        #endregion

        #region public methods

        public async Task<IEnumerable<clsEmail>> ListEmailConfigurationsAsync()
        {
            //TODO: lists all email configurations as found in the data store
            List<clsEmail> list = null;

            try
            {
                var config = await returnDatabaseContextAsync();
                if (config != null)
                {
                    using (config)
                    {
                        try
                        {
                            var dta = await config.TemailConfigs.Where(c => c.IsActive == 1).ToListAsync();
                            if (dta != null)
                            {
                                list = dta.Select(a => new clsEmail()
                                {
                                    id = a.Id,
                                    oCompany = new CompanyLookup()
                                    {
                                        id = (int) a.CompanyId
                                    },
                                    msgFrom = a.Mfrom,
                                    msgTo = a.MTo,
                                    msgCC = a.MCc,
                                    msgBlindCC = a.MBcc,
                                    host = a.Host,
                                    port = (int) a.Port,
                                    userCredential = a.UsrCredential,
                                    userPassword = a.UsrPassword,
                                    logoUrl = a.LogoUrl,
                                    bifaUrl = a.BifaUrl,
                                    subjectColor = a.SubjectColor,
                                    bifaSign = a.BifaSign
                                }).ToList();
                            }
                        }
                        catch(Exception configExc)
                        {
                            throw configExc;
                        }
                    }
                }

                return list;
            }
            catch(Exception x)
            {
                return list;
            }
        }

        public async Task<clsEmail> getEmailConfigurationAsync()
        {
            //TODO: method uses the name of a company to find a singular email configuration object
            clsEmail obj = null;

            try
            {
                var config = await returnDatabaseContextAsync();
                if (config != null)
                {
                    using (config)
                    {
                        try
                        {
                            var rec = await config.TemailConfigs.Where(c => c.CompanyId == oCompany.id).FirstOrDefaultAsync();
                            if (rec != null)
                            {
                                obj = new clsEmail() {
                                    id = rec.Id,
                                    oCompany = new CompanyLookup()
                                    {
                                        id = (int)rec.CompanyId,
                                        nameOfcompany = this.oCompany.nameOfcompany
                                    },
                                    msgFrom = rec.Mfrom,
                                    msgTo = rec.MTo,
                                    msgCC = rec.MCc,
                                    msgBlindCC = rec.MBcc,
                                    host = rec.Host,
                                    port = (int)rec.Port,
                                    userCredential = rec.UsrCredential.Trim(),
                                    userPassword = rec.UsrPassword.Trim(),
                                    logoUrl = rec.LogoUrl,
                                    bifaUrl = rec.BifaUrl,
                                    subjectColor = rec.SubjectColor,
                                    bifaSign = rec.BifaSign
                                };
                            }
                        }
                        catch(Exception configErr)
                        {
                            throw configErr;
                        }
                    }
                }

                return obj;
            }
            catch(Exception x)
            {
                return obj;
            }
        }

        public async Task<bool> SendMailAsync(string sbject, string sbody, string sreceipient, string scopied, string sblindedcc)
        {
            //TODO: method is used in sending mails
            bool bln = false;

            try
            {
                var smtpClient = new SmtpClient(host)
                {
                    Port = (int)port,
                    Credentials = new NetworkCredential(this.userCredential, this.userPassword),
                    EnableSsl = true
                };

                var mailMessage = new MailMessage() { 
                    From = new MailAddress(this.msgFrom),
                    Subject = sbject,
                    Body = sbody,
                    IsBodyHtml = true
                };

                mailMessage.To.Add(sreceipient);
                mailMessage.CC.Add(scopied);
                mailMessage.Bcc.Add(sblindedcc);

                await smtpClient.SendMailAsync(mailMessage);

                return bln = true;
            }
            catch(Exception x)
            {
                return bln;
            }
        }

        #endregion



    }
}
