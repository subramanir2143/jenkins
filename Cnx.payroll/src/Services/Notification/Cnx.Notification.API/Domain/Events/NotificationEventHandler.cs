using Cnx.Notification.API.Domain.Services;
using Cnx.Notification.API.Infrastructure;
using Cnx.Notification.API.Infrastructure.SignalRHub;
using DotNetCore.CAP;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cnx.Notification.API.Domain.Events
{
    public class NotificationEventHandler : ICapSubscribe, INotificationEventSubscriberService
    {
        private readonly IMailService _mailService;
        private readonly IHubContext<MessageHub> _hubcontext;
        public NotificationEventHandler(IMailService mailService, IHubContext<MessageHub> hubcontext)
        {
            _mailService = mailService;
            _hubcontext = hubcontext;
        }

        [CapSubscribe(nameof(SendEmailEvent))]
        public void Handle(SendEmailEvent sendEmailEvent)
        {
            _mailService.Execute(sendEmailEvent);
        }

        [CapSubscribe(nameof(SignalRNotificationEvent))]
        public async void Handle(SignalRNotificationEvent signalRNotificationEvent)
        {
            await _hubcontext.Clients.Clients(signalRNotificationEvent.ConnectionIds).
                SendAsync(signalRNotificationEvent.MethodToCall, signalRNotificationEvent.Arguement1, signalRNotificationEvent.Arguement2);
        }
    }
}
