#nullable disable

using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace UserManagementAPI.utils
{
    public class Sender
    {
        public async Task<bool> SendMailAsync(Message message)
        {
            try
            {
                SmtpClient client = new SmtpClient(message.smtp, message.smtpPort);
                client.EnableSsl = true;

                client.Timeout = 20000;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(message.smtpUserCredential, message.smtpPassword);

                //mail message
                var msg = new MailMessage();
                msg.To.Add(message.receipient);
                msg.CC.Add(message.msgCopy);
                msg.Bcc.Add(message.msgBlindCopy);
                msg.From = new MailAddress(message.smtpUserCredential);
                msg.Subject = message.subject;
                msg.Body = message.msgBody;

                ServicePointManager.ServerCertificateValidationCallback = delegate (object s,
                                                            X509Certificate certificate, X509Chain chain,
                                                            SslPolicyErrors sslPolicyErrors)
                                                            { return true; };

                await client.SendMailAsync(msg);
                return true;
            }
            catch(Exception x)
            {
                return false;
            }
        }
    }

    public class Message
    {
        public string receipient { get; set; } = string.Empty;
        public string subject { get; set; } = string.Empty;
        public string msgBody { get; set; } = string.Empty;
        public string msgCopy { get; set; } = string.Empty;
        public string msgBlindCopy { get; set; } = string.Empty;

        public string smtp { get; set; }
        public int smtpPort { get; set; }
        public string smtpUserCredential { get; set; } = string.Empty;
        public string smtpPassword { get; set; } = string.Empty;
    }

}
