using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SeniorVlogger.Common.Email.IEmail;
using SeniorVlogger.DataAccess.Repository.IRepository;
using SeniorVlogger.Models.DTO;
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
        private readonly IEmailService _emailService;

        #endregion

        #region Constructor

        public BlogController(IUnitOfWork unitOfWork, UploadsService uploadsService, 
            IEmailService emailService, ILogger<BlogController> logger)
        {
            _unitOfWork = unitOfWork;
            _uploadsService = uploadsService;
            _logger = logger;
            _emailService = emailService;
        }

        #endregion

        #region Actions

        [HttpGet]
        [AllowAnonymous]
        public async Task<IEnumerable<BlogPostViewModel>> GetAll()
        {
            var validUser = await ValidateUser();
            var posts = await _unitOfWork.BlogPosts.GetAll(includeProperties: "Category,Author");

            return await GetPostsWithScratchIfUserValid(posts);
        }

        [HttpGet("slug/{slug}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetBySlug(string slug)
        {
            var post = await _unitOfWork.BlogPosts.GetFirstOrDefault(p => p.Slug == slug, includeProperties: "Category,Author,Next,Previous");
            var result = await GetPostsWithScratchIfUserValid(post);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpGet("category/{id}")]
        [AllowAnonymous]
        public async Task<IEnumerable<BlogPostViewModel>> GetByCategory(int id)
        {
            var posts = await _unitOfWork.BlogPosts.GetAll(p => p.Category.Id == id, includeProperties: "Category,Author");
            return await GetPostsWithScratchIfUserValid(posts);
        }

        [HttpGet("tag/{tag}")]
        [AllowAnonymous]
        public async Task<IEnumerable<BlogPostViewModel>> GetByTag(string tag)
        {
            var posts = await _unitOfWork.BlogPosts.GetAll(p => p.Tags.Contains(tag), includeProperties: "Category,Author");
            return await GetPostsWithScratchIfUserValid(posts);
        }

        [HttpGet("short")]
        public async Task<IEnumerable<ShortPostViewModel>> GetSlugs()
        {
            var posts = await _unitOfWork.BlogPosts.GetAll();
            return posts.Select(p => new ShortPostViewModel(p.Id, p.Slug, p.Title));
        }

        [HttpGet("{id}")]
        public async Task<BlogPostViewModel> Get(int id)
        {
            var post = await _unitOfWork.BlogPosts.GetFirstOrDefault(p => p.Id == id, includeProperties: "Category,Author,Next,Previous");
            return post?.ToViewModel();
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

                if (post.Mailed && !post.Scratch)
                    await SendNotificationForSubscribers(post);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Cannot create blog post: {e.Message}");
                return Problem();
            }

            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] BlogPostViewModel post)
        {
            try
            {
                var oldPost = await _unitOfWork.BlogPosts.GetFirstOrDefault(p => p.Id == post.Id);
                if (oldPost == null) return NotFound();

                //TODO update publish date or no?
                post.PublishDate = DateTime.Now.ToString();
                var objDb = post.ToDto();
                objDb.Slug = post.Title.GenerateSlug();

                await _unitOfWork.BlogPosts.Update(objDb);
                await _unitOfWork.Save();

                if(oldPost.Scratch && !post.Scratch && post.Mailed)
                    await SendNotificationForSubscribers(post);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Cannot update blog post: {e.Message}");
                return Problem();
            }

            return Ok();
        }

        [HttpDelete("{id}")]
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

        #region Private Methods

        private async Task SendNotificationForSubscribers(BlogPostViewModel post)
        {
            var subscribers = await _unitOfWork.Subscriptions
                .GetAll(s => s.IsSubscribed);

            foreach (var email in subscribers.Select(s => s.Email))
            {
                _emailService.SendNewPostAvailableAsync(email,
                    $"https://seniorvlogger.com/blog/{post.Slug}",
                    post.Title, post.Description, $"https://seniorvlogger.com/{post.ImageUrl}");
            }
        }

        private async Task<bool> ValidateUser()
        {
            var email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _unitOfWork.ApplicationUsers
                .GetFirstOrDefault(u => u.Email == email);
            return user != null;
        }

        private async Task<BlogPostViewModel> GetPostsWithScratchIfUserValid(BlogPostDto post)
        {
            var result = await GetPostsWithScratchIfUserValid(new[] {post});
            return result.FirstOrDefault();
        }

        private async Task<IEnumerable<BlogPostViewModel>> GetPostsWithScratchIfUserValid
            (IEnumerable<BlogPostDto> posts)
        {
            if (await ValidateUser())
                return posts?.Select(p => p.ToViewModel());

            return posts?.Where(p => !p.Scratch)?.Select(p => p.ToViewModel());
        }

        #endregion
    }
}
