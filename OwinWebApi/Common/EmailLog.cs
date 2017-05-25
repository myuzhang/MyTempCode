using System;
using Common.Models;

namespace Common
{
    public class EmailLog
    {
        private static EmailLog _instance;

        private EmailLog()
        { }

        public static EmailLog Instance = _instance ?? (_instance = new EmailLog());

        public void Send(string from, string to)
        {
            if (string.IsNullOrEmpty(from) || string.IsNullOrEmpty(to))
            {
                throw new ArgumentNullException($"Sending email log should specify from:{from} and to:{to}");
            }
            var emailClient = new SendEmailClient();
            emailClient.Send(new Email
            {
                Body = Log.Instance.GetEmailLog(),
                Subject = "Log for sending email to patient",
                From = from,
                IsBodyHtml = true,
                To = to
            });
        }
    }
}
