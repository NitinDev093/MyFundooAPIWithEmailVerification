using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace UtilityLayer
{
    public class EmailHelper
    {
        private readonly IConfiguration _config;
        public EmailHelper(IConfiguration configuration)
        {
            _config = configuration;
        }

        public void SendEmail(string toEmail, string subject, string body)
        {
            try
            {
                var emailData = _config.GetSection("Emailsettings");
                string host = emailData["Host"];
                string password = emailData["Password"];
                string fromemail = emailData["FromEmail"];
                int port = Convert.ToInt32(emailData["Port"]);
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(fromemail);
                mail.To.Add(toEmail);
                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = true;

                using (SmtpClient smtp = new SmtpClient(host, port))
                {
                    smtp.Credentials = new NetworkCredential(fromemail, password);
                    smtp.EnableSsl = true;
                    smtp.Send(mail);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while sending email: " + ex.Message);
            }
        }

    }
}

