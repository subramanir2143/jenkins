using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using Microsoft.Extensions.Configuration;

namespace Cnx.EarningsAndDeductions.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class DownloadController : ControllerBase
    {

        private IConfiguration _configuration;
        public DownloadController(IConfiguration Configuration)
        {
            _configuration = Configuration;
        }
    

        [Route("DownloadTemplate")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DownloadTemplate()
        {
            string filename = _configuration["TemplateName"];

            if (filename == null)
                return NotFound("filename not present");
           
                var path = Path.Combine(
                               Directory.GetCurrentDirectory(),
                               "Template", filename);

                var memory = new MemoryStream();
                using (var stream = new FileStream(path, FileMode.Open))
                {
                    await stream.CopyToAsync(memory);
                }

                memory.Position = 0;
                return File(memory, GetContentType(path), Path.GetFileName(path));
           
        }

        private string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }

        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"},
                //{".xlsx", "application/officedocument.spreadsheetml.sheet"},
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"}
            };
        }
    }
}