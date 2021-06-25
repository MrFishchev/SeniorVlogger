using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
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

        private string GetPostImagesDirectory(string slug)
        {
            return Path.Combine(_hostEnvironment.WebRootPath, $"{Uploads}/{slug}");
        }
        
        public string ParseAndSaveImages(string slug, string content)
        {
            var sb = new StringBuilder(content);

            try
            {
                var patternBase64 = "(data:image\\/[^;]+;base64[^\"]+)";
                var patternType = "((?<=\\/)\\w+(?=;)){1}";

                var regexBase64 = new Regex(patternBase64,
                    RegexOptions.Compiled | RegexOptions.IgnoreCase);
                var regexType = new Regex(patternType, RegexOptions.Compiled);

                var deltaIndex = 0;
                var matches = regexBase64.Matches(content);
                foreach (Match match in matches)
                {
                    var type = regexType.Match(match.Value)?.Value;
                    var name = $"{Guid.NewGuid()}.{type}";
                    var pathToSave = Path.Combine(GetPostImagesDirectory(slug), name);

                    SaveBase64ToImage(pathToSave, match.Value.Split(',')[1]);

                    var calculatedIndex = match.Index + deltaIndex;
                    sb.Remove(calculatedIndex, match.Length);
                    sb.Insert(calculatedIndex, $"/{Uploads}/{slug}/{name}");

                    deltaIndex = sb.Length - content.Length;
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Cannot save images from the post: {slug}");
                return null;
            }

            return sb.ToString();
        }

        public async Task<string> Upload(IFormFile file)
        {
            try
            {
                var name = Guid.NewGuid() + Path.GetExtension(file.FileName);
                var path = Path.Combine(_hostEnvironment.WebRootPath, Uploads);

                _logger.LogInformation($"Saving image to {path}");

                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                await using var stream = new FileStream(Path.Combine(path, name), FileMode.Create);
                await file.CopyToAsync(stream);

                //TODO save file link to DB
                return $"{Uploads}/{name}";
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Cannot upload file: {e.Message}");
                throw;
            }
        }

        public void Delete(string path)
        {
            var fullPath = Path.Combine(_hostEnvironment.WebRootPath, path);
            try
            {
                if (File.Exists(fullPath))
                    File.Delete(fullPath);
            }
            catch(Exception e)
            {
                _logger.LogError(e, $"Cannot delete image: {fullPath}");
            }
        }

        public void DeleteBlogImages(string slug)
        {
            var directory = Path.Combine(_hostEnvironment.WebRootPath, $"{Uploads}/{slug}");

            try
            {
                if (Directory.Exists(directory))
                    Directory.Delete(directory, true);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Cannot delete directory: {directory}");
            }
        }

        private void SaveBase64ToImage(string pathToSave, string base64Data)
        {
            var directoryName = Path.GetDirectoryName(pathToSave);
            if (!Directory.Exists(directoryName))
                Directory.CreateDirectory(directoryName);
            
            File.WriteAllBytes(pathToSave, Convert.FromBase64String(base64Data));
        }
    }
}
