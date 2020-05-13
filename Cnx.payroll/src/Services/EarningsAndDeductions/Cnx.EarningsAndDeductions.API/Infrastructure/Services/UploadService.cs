using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;

namespace Cnx.EarningsAndDeductions.API.Infrastructure.Services
{
    public class UploadService
    {
        public async Task UploadFileAsync(IFormFile file, string rootPath)
        {
            string path = Path.Combine(rootPath, file.FileName);
            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
        }
    }
}
