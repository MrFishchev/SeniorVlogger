﻿using System;
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
using SeniorVlogger.Models.Requests;
using SeniorVlogger.Models.ViewModels;
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
        #region Fields

        private readonly ILogger<BlogController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UploadsService _uploadsService;

        #endregion

        #region Constructor

        public BlogController(IUnitOfWork unitOfWork, UploadsService uploadsService, ILogger<BlogController> logger)
        {
            _unitOfWork = unitOfWork;
            _uploadsService = uploadsService;
            _logger = logger;
        }

        #endregion

        #region Actions

        [HttpGet]
        public async Task<IEnumerable<BlogPostViewModel>> GetAll()
        {
            var posts = await _unitOfWork.BlogPosts.GetAll(includeProperties: "Category,Author");
            return posts.Select(i => i.ToViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BlogPostViewModel post)
        {
            try
            {
                var email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var user = await _unitOfWork.ApplicationUsers
                    .GetFirstOrDefault(u => u.Email == email);

                var objDb = post.ToDto();
                objDb.AuthorId = user.Id;
                objDb.Slug = post.Title.GenerateSlug();
                objDb.PublishDate = DateTime.UtcNow;
                //TODO Generate unique slug by DB

                await _unitOfWork.BlogPosts.Add(objDb);
                await _unitOfWork.Save();
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

            await _uploadsService.Delete(blogPost.ImageUrl);
            await _unitOfWork.BlogPosts.Remove(blogPost);
            await _unitOfWork.Save();

            return Ok();
        }

        #endregion
    }
}
