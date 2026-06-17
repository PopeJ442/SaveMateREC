using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Savemate.Application.Common.Extensions
{
    public class EmailHelper
    {
        private readonly EmailSettings _settings;
        public EmailHelper(IOptions<EmailSettings>option)
        {
            _settings = option.Value;
        }
        public bool SendEmailTwoFactorCode(string userEmail, string code)
        {
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(_settings.SenderEmail);
            mailMessage.To.Add(new MailAddress(userEmail));
            mailMessage.Subject = "Two Factor Code";
            mailMessage.IsBodyHtml = true;
            mailMessage.Body = $"<p>Your authentication code is: <strong>{code}</strong></p>";

            using (SmtpClient client = new SmtpClient(_settings.SmtpHost, _settings.SmtpPort))
            {
                client.Credentials = new NetworkCredential(_settings.SenderEmail, _settings.SenderPassword);
                client.EnableSsl = true;  

                try
                {
                    client.Send(mailMessage);
                    return true;
                }
                catch (Exception  )
                {
                    // log ex.Message
                    return false;
                }
            }
        }

    }
}
