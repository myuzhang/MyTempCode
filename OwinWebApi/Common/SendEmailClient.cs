using System;
using System.Configuration;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Common.Models;

namespace Common
{
    public class SendEmailClient
    {
        private readonly SmtpClient _client;

        public SendEmailClient()
        {
            _client = new SmtpClient
            {
                Host = ConfigurationManager.AppSettings["stmpHost"],
                Port = Int32.Parse(ConfigurationManager.AppSettings["stmpPort"]),
                EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["stmpSSL"]),
                Timeout = 60000,
                DeliveryMethod = SmtpDeliveryMethod.Network
            };
            if (_client.EnableSsl)
            {
                _client.Credentials = new System.Net.NetworkCredential(
                    ConfigurationManager.AppSettings["stmpUser"],
                    ConfigurationManager.AppSettings["stmpPass"]);
            }
        }

        public void Send(Email email)
        {
            var mm = GetMessage(email);
            _client.Send(mm);
        }

        public async Task SendAsync(Email email)
        {
            var mm = GetMessage(email);
            await _client.SendMailAsync(mm);
        }

        private MailMessage GetMessage(Email email)
        {
            MailMessage mm = new MailMessage
            {
                BodyEncoding = Encoding.UTF8,
                DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure,
                From = new MailAddress(email.From)
            };
            if (email.To != null) mm.To.Add(new MailAddress(email.To));
            if (email.ToList != null && email.ToList.Count > 0)
            {
                foreach (var to in email.ToList)
                {
                    mm.To.Add(new MailAddress(to));
                }
            }
            mm.Subject = email.Subject;
            mm.IsBodyHtml = email.IsBodyHtml;
            mm.Body = email.Body;

            return mm;
        }
    }
}
