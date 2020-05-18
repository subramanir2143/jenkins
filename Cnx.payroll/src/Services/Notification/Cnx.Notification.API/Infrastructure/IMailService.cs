using Cnx.Notification.API.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cnx.Notification.API.Infrastructure
{
    public interface IMailService
    {
        void Execute(SendEmailEvent sendEmailEvent);
    }
}
