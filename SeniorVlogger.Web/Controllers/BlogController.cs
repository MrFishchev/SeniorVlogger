using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SeniorVlogger.DataAccess.Repository.IRepository;
using SeniorVlogger.Models;
using SeniorVlogger.Models.DTO;
using SeniorVlogger.Web.Extensions;
using SeniorVlogger.Web.Services;
using SlugGenerator;

namespace SeniorVlogger.Web.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class BlogController : Controller
    {
        private readonly ILogger<BlogController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UploadsService _uploadsService;

        public BlogController(IUnitOfWork unitOfWork, UploadsService uploadsService, ILogger<BlogController> logger)
        {
            _unitOfWork = unitOfWork;
            _uploadsService = uploadsService;
            _logger = logger;
        }

        [HttpPost("image")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> SaveImage([FromForm] IFormCollection form)
        {
            if (!form.Files.Any()) return BadRequest();

            var file = form.Files.First();
            var result = await _uploadsService.Upload(file);

            if (string.IsNullOrEmpty(result)) return BadRequest();

            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> SavePost([FromBody] BlogPost post)
        {
            try
            {
                var email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var user = await _unitOfWork.ApplicationUsers
                    .GetFirstOrDefault(u => u.Email == email);

                var objDb = post.ToDto();
                objDb.AuthorId = user.Id;
                objDb.Slug = post.Title.GenerateSlug();
                //TODO Generate unique slug by DB

                await _unitOfWork.BlogPosts.Add(objDb);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Cannot create blog post: {e.Message}");
                return Problem();
            }

            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var blogPost = await _unitOfWork.BlogPosts.Get(id);
            if (blogPost == null) return NotFound();

            //TODO DeleteOldImage
            await _unitOfWork.BlogPosts.Remove(blogPost);
            await _unitOfWork.Save();

            return Ok();
        }
    }
}
