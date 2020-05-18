using Cnx.Notification.API.Domain.Events;
using System.Net.Mail;

namespace Cnx.Notification.API.Infrastructure
{
    public interface ISmtpClient
    {
        void Send(MailMessage message);

        void EnableSSLForEmail();
    }
}
