using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Cnx.EarningsAndDeductions.API.Application.Commands
{
    [DataContract]
    public class EDUploadCommand: IRequest<Unit>
    {   
        public string FileName { get; protected set; }

        public string Uploader { get; protected set; }

        public Guid Id { get; set; }   
        
        public EDUploadCommand(string fileName, string uploader)
        {
            FileName = fileName;
            Uploader = uploader;
            Id = Guid.NewGuid();
        }
    }
}
