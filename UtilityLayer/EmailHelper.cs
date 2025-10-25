using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;

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
                throw ex;
            }
        }
        public string emailbodyTemplate(string name, string url)
        {
            return $@"

                             <!DOCTYPE html>
                             <html>
                             <head><meta charset='UTF-8'></head>
                             <body style='font-family: Arial, sans-serif; background-color: #f4f4f4; margin: 0; padding: 0;'>
                             <div style='max-width: 600px; margin: 30px auto; background-color: #ffffff; border-radius: 10px; box-shadow: 0 4px 8px rgba(0,0,0,0.1); overflow: hidden;'>
                             <div style='background-color: #343a40; color: #ffffff; padding: 20px; text-align: center;'>
                             <h2 style='margin: 0;'>Welcome to Fundoo Users</h2>
                             </div>

                            <div style='padding: 30px; text-align: center; color: #333;'>
                            <p style='font-size: 16px;'>Hello <strong>{name}</strong>,</p>
                            <p style='font-size: 15px;'>Thank you for signing up! Please verify your account by clicking the button below:</p>

                            <a href='{url}' style='display: inline-block; background-color: #212529; color: #ffffff; text-decoration: none; padding: 12px 30px; border-radius: 6px; font-size: 16px; margin: 20px 0;'>
                            Verify Now
                            </a>

                            <p style='font-size: 13px; color: #555;'>If the button doesn't work, copy and paste this link into your browser:</p>
                            <p style='font-size: 13px; word-wrap: break-word;'><a href='{url}' style='color: #007bff;'>{url}</a></p>

                            <p style='font-size: 12px; color: #888; margin-top: 30px;'>This link will expire in 24 hours.</p>
                            </div>

                            <div style='background-color: #f1f1f1; text-align: center; padding: 15px; font-size: 12px; color: #777;'>
                            &copy; {DateTime.Now.Year} Fundoo Notes. All rights reserved.
                            </div>
                            </div>
                            </body>
                            </html>";

        }
        public string emailSuccessfullTemplate(string Firstname)
        {
            return $@"
    <!DOCTYPE html>
    <html>
    <head><meta charset='UTF-8'></head>
    <body style='font-family: Arial, sans-serif; background-color: #f4f4f4; margin: 0; padding: 0;'>
      <div style='max-width: 600px; margin: 30px auto; background-color: #ffffff; border-radius: 10px; box-shadow: 0 4px 8px rgba(0,0,0,0.1); overflow: hidden;'>
        <div style='background-color: #28a745; color: #ffffff; padding: 20px; text-align: center;'>
          <h2 style='margin: 0;'>Email Verified Successfully</h2>
        </div>

        <div style='padding: 30px; text-align: center; color: #333;'>
          <p style='font-size: 16px;'>Hello <strong>{Firstname}</strong>,</p>
          <p style='font-size: 15px;'>Congratulations! Your email has been successfully verified.</p>

          <p style='font-size: 15px; color: #555;'>You can now access all the features of Fundoo Notes.</p>

        <div style='background-color: #f1f1f1; text-align: center; padding: 15px; font-size: 12px; color: #777;'>
          &copy; {DateTime.Now.Year} Fundoo Notes. All rights reserved.
        </div>
      </div>
    </body>
    </html>";
        }

        public string emailFailureTemplate(string Firstname)
        {
            return $@"
    <!DOCTYPE html>
    <html>
    <head><meta charset='UTF-8'></head>
    <body style='font-family: Arial, sans-serif; background-color: #f4f4f4; margin: 0; padding: 0;'>
      <div style='max-width: 600px; margin: 30px auto; background-color: #ffffff; border-radius: 10px; box-shadow: 0 4px 8px rgba(0,0,0,0.1); overflow: hidden;'>
        <div style='background-color: #dc3545; color: #ffffff; padding: 20px; text-align: center;'>
          <h2 style='margin: 0;'>Email Verification Failed</h2>
        </div>

        <div style='padding: 30px; text-align: center; color: #333;'>
          <p style='font-size: 16px;'>Hello <strong>{Firstname}</strong>,</p>
          <p style='font-size: 15px;'>We’re sorry, but your email verification link has expired or is invalid.</p>

          <p style='font-size: 15px; color: #555;'>Please click the button below to request a new verification email:</p>

          <p style='font-size: 13px; color: #666;'>If you did not create an account, you can safely ignore this message.</p>
        </div>

        <div style='background-color: #f1f1f1; text-align: center; padding: 15px; font-size: 12px; color: #777;'>
          &copy; {DateTime.Now.Year} Fundoo Notes. All rights reserved.
        </div>
      </div>
    </body>
    </html>";
        }

        public string resetPasswordTemplates(string resetUrl)//this body will work for reset and forgot password
        {
            return $@"

            ";
        }
    }
}

