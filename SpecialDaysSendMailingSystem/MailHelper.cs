using Microsoft.Exchange.WebServices.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpecialDaysSendMailingSystem
{
    public class MailHelper
    {
        // Note: To send email you need to add actual email id and credential so that it will work as expected
        private static readonly string EMAIL_SENDER = "bilet@asiselektronik.com.tr"; // change it to actual sender email id or get it from UI input
        private static readonly string EMAIL_USER = "bilet"; // change it to actual sender email id or get it from UI input
        private static readonly string EMAIL_CREDENTIALS = "Qwe12345!"; // Provide credentials
        private static readonly string SMTP_CLIENT = "mail.asiselektronik.com.tr"; // as we are using outlook so we have provided smtp-mail.outlook.com
        public static bool SendEMail(string[] recipient, string subject, string message)
        {
            try
            {
                var service = new ExchangeService(ExchangeVersion.Exchange2013)
                {
                    Credentials =
                    new WebCredentials(EMAIL_USER, EMAIL_CREDENTIALS),
                    TraceEnabled = true,
                    TraceFlags = TraceFlags.All
                };
                service.AutodiscoverUrl(EMAIL_SENDER,
                    RedirectionCallback);
                EmailMessage email = new EmailMessage(service);

                if (recipient != null)
                    foreach (string item in recipient)
                        email.ToRecipients.Add(item);

                email.Subject = subject;
                email.Body = new MessageBody(message);
                email.Body.BodyType = BodyType.HTML;
                email.Send();
                return true;
            }
            catch (Exception ex)
            {
                String exMessage = ex.Message;
                return false;
            }
        }
        public static bool RedirectionCallback(string redirectionUrl)
        {
            var redirectionUri = new Uri(redirectionUrl);
            var result = redirectionUri.Scheme == "https";
            return result;
        }
    }
}
