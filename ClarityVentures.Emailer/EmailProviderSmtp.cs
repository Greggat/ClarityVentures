using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace ClarityVentures.Emailer
{
    public class EmailProviderSmtp : IEmailProvider
    {
        private readonly IConfiguration _config;
        private readonly SmtpClient _mailClient;
        private readonly ILogger<EmailProviderSmtp> _logger;

        public EmailProviderSmtp(IConfiguration config, ILogger<EmailProviderSmtp> logger)
        {
            _config = config;
            _logger = logger;

            var host = _config["email_host"];
            var login = _config["email_login"];
            var password = _config["email_password"];

            _mailClient = new SmtpClient(host)
            {
                UseDefaultCredentials = false,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Port = 587,
                Credentials = new NetworkCredential(login, password),
                EnableSsl = true,
                Timeout = 5000
            };

            _logger.LogInformation("SmtpClient host address set to {0}", host);
            
        }

        public async Task<bool> SendEmail(string sender, string recipient, string subject, string body)
        {
            using EmailerContext db = new();
            try
            {
                var email = new Email
                {
                    Sender = sender, //Maybe use login from config instead?
                    Recipient = recipient,
                    Subject = subject,
                    Body = body,
                    Status = SendAttemptStatus.Sending,
                    Date = DateTime.UtcNow
                };

                await db.Emails.AddAsync(email);
                await db.SaveChangesAsync();

                _logger.LogInformation("Attempting to send an email...");

                for(int i = 0; i < 3; i++)
                {
                    try
                    {
                        MailMessage message = new MailMessage()
                        {
                            From = new MailAddress(sender),
                            Subject = subject,
                            Body = body
                        };
                        message.To.Add(recipient);

                        await _mailClient.SendMailAsync(message);
                        email.Status = SendAttemptStatus.Sent;
                        await db.SaveChangesAsync();
                        return true;//Email sent successfully!
                    }
                    catch(SmtpException ex)
                    {
                        //try to send again and maybe check some statuscodes here
                        _logger.LogWarning("Attempt #{0} failed to send email...", i + 1);
                        if(i == 2)
                        {
                            _logger.LogError(ex.ToString());
                        }
                        continue;
                    }
                }

                email.Status = SendAttemptStatus.Failed;
                await db.SaveChangesAsync();
            } 
            catch (Exception ex)
            {
                //TODO: Maybe create a few catches for specific exceptions?
                //Likely a db error, lets log it
                _logger.LogError(ex.ToString());
            }
            return false;
        }
    }
}
