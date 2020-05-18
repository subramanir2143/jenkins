using Cnx.Notification.API.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cnx.Notification.API.Domain.Services
{
    public interface INotificationEventSubscriberService
    {
        void Handle(SendEmailEvent sendEmailEvent);
        void Handle(SignalRNotificationEvent signalRNotificationEvent);
    }
}
