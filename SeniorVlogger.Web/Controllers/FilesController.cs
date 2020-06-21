using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SeniorVlogger.Models.Requests;
using SeniorVlogger.Web.Services;

namespace SeniorVlogger.Web.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class FilesController : Controller
    {
        #region Fields

        private readonly ILogger<FilesController> _logger;
        private readonly UploadsService _uploadsService;

        #endregion

        #region Constructor

        public FilesController(UploadsService uploadsService, ILogger<FilesController> logger)
        {
            _uploadsService = uploadsService;
            _logger = logger;
        }

        #endregion

        #region Actions

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> SaveImage([FromForm] IFormCollection form)
        {
            if (!form.Files.Any()) return BadRequest();

            var file = form.Files.First();
            var result = await _uploadsService.Upload(file);

            if (string.IsNullOrEmpty(result)) return BadRequest();

            return Json(result);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteImage([FromBody] DeleteImageRequest request)
        {
            try
            {
                await _uploadsService.Delete(request.Path);
                return Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        #endregion
    }
}
