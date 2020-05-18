using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using Cnx.Notification.API.Domain.Events;
using Microsoft.Extensions.Configuration;

namespace Cnx.Notification.API.Infrastructure
{
    public class MailService : IMailService
    {
        private readonly ISmtpClient _smtpClient;
        private readonly IConfiguration _configuration;
        public MailService(ISmtpClient smtpClient, IConfiguration configuration)
        {
            _smtpClient = smtpClient;
            _configuration = configuration;
        }
        public void Execute(SendEmailEvent sendEmailEvent)
        {
            var mailMessage = new MailMessage
            {
                From = new MailAddress(_configuration.GetValue<string>("SmtpUsername", "test@email.com"))
            };
            if (sendEmailEvent.ToAddress != null)
            {
                foreach (var toAddress in sendEmailEvent.ToAddress)
                {
                    mailMessage.To.Add(toAddress);
                }
            }
            else
            {
                throw new ArgumentNullException("To Address can't be blank");
            }

            if (sendEmailEvent.BccAddress != null)
                foreach (var bccAddress in sendEmailEvent.BccAddress)
                {
                    mailMessage.Bcc.Add(bccAddress);
                }
            if (sendEmailEvent.CCAddress != null)
                foreach (var ccAddress in sendEmailEvent.CCAddress)
                {
                    mailMessage.Bcc.Add(ccAddress);
                }

            mailMessage.Body = sendEmailEvent.Body;
            mailMessage.Subject = sendEmailEvent.Subject;
            mailMessage.IsBodyHtml = true;
            _smtpClient.EnableSSLForEmail();
            _smtpClient.Send(mailMessage);
        }
    }
}
