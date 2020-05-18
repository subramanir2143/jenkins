
using Cnx.Notification.API.Domain.Events;
using Cnx.Notification.API.Domain.Services;
using Cnx.Notification.API.Infrastructure;
using Cnx.Notification.API.Infrastructure.SignalRHub;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Net;
using System.Net.Mail;
using Xunit;

namespace Cnx.Notification.API.UnitTests
{
    public class NotificationTest
    {
        private readonly Mock<ISmtpClient> _isMptpClient;
        private readonly Mock<IConfiguration> _iConfiguration;
        private readonly Mock<IHubContext<MessageHub>> _hubcontext;
        private readonly Mock<INotificationEventSubscriberService> _notificationEvent;
        // private readonly Mock<ILogger<EmployeeController>> _loggerMock;

        public NotificationTest()
        {
            _isMptpClient = new Mock<ISmtpClient>();
            _iConfiguration = new Mock<IConfiguration>();
            _hubcontext = new Mock<IHubContext<MessageHub>>();
            _notificationEvent = new Mock<INotificationEventSubscriberService>();
        }
        [Fact]
        public void MailSendTest()
        {

            var obj = _isMptpClient.Object;
            MailMessage mailMessage = new MailMessage();
            _iConfiguration.Setup(c => c.GetSection(It.IsAny<String>())).Returns(new Mock<IConfigurationSection>().Object);
            SendEmailEvent sendEmailEvent = new SendEmailEvent();
            sendEmailEvent.Body = "test";
            sendEmailEvent.Subject = "test";
            sendEmailEvent.ToAddress = new System.Collections.Generic.List<string> { "test@test.com" };
            IMailService mailService = new MailService(_isMptpClient.Object, _iConfiguration.Object);
            mailService.Execute(sendEmailEvent);
        }

        [Fact]

        public void MailSendWithoutToAddressTest()
        {

            var obj = _isMptpClient.Object;
            MailMessage mailMessage = new MailMessage();
            _iConfiguration.Setup(c => c.GetSection(It.IsAny<String>())).Returns(new Mock<IConfigurationSection>().Object);
            SendEmailEvent sendEmailEvent = new SendEmailEvent();
            sendEmailEvent.Body = "test";
            sendEmailEvent.Subject = "test";

            IMailService mailService = new MailService(_isMptpClient.Object, _iConfiguration.Object);
            Assert.Throws<ArgumentNullException>(() => mailService.Execute(sendEmailEvent));
        }
        [Fact]
        public void SignalRNotificationEventTest()
        {
            Mock<SignalRNotificationEvent> signalRNotificationEvent = new Mock<SignalRNotificationEvent>();
           
            _notificationEvent.Object.Handle(signalRNotificationEvent.Object);

        }
    }
}
