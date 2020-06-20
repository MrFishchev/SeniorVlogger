using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace SeniorVlogger.Web.Services
{
    public class UploadsService
    {
        private const string Uploads = "uploads";
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly ILogger<UploadsService> _logger;

        public UploadsService(IWebHostEnvironment hostEnvironment, ILogger<UploadsService> logger)
        {
            _hostEnvironment = hostEnvironment;
            _logger = logger;
        }

        public async Task<string> Upload(IFormFile file)
        {
            try
            {
                var name = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                var path = Path.Combine(_hostEnvironment.WebRootPath, Uploads);

                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                await using var stream = new FileStream(Path.Combine(path, name), FileMode.Create);
                await file.CopyToAsync(stream);

                //TODO save file link to DB
                return $"{Uploads}/{name}";
            }
            catch(Exception e)
            {
                _logger.LogError(e, $"Cannot upload file: {e.Message}");
                throw;
            }
        }

        public Task Delete(string path)
        {
            try
            {
                var fullPath = Path.Combine(_hostEnvironment.WebRootPath, path);
                if(File.Exists(fullPath))
                    File.Delete(fullPath);
            }
            catch
            {
                //ignore
            }

            return Task.CompletedTask;
        }


    }
}
