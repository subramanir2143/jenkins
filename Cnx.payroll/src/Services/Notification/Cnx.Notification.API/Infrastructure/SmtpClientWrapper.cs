﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Cnx.Notification.API.Infrastructure
{
    public class SmtpClientWrapper : SmtpClient, ISmtpClient
    {
        public void EnableSSLForEmail()
        {
            EnableSsl = true;
        }
    }
}
