using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SeniorVlogger.Common.Email.IEmail;
using SeniorVlogger.DataAccess.Repository.IRepository;
using SeniorVlogger.Models;
using SeniorVlogger.Models.DTO;
using SeniorVlogger.Models.ViewModels;
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
        private readonly IMapper _mapper;

        #endregion

        #region Constructor

        public BlogController(IUnitOfWork unitOfWork, UploadsService uploadsService, 
            IEmailService emailService, ILogger<BlogController> logger,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _uploadsService = uploadsService;
            _logger = logger;
            _emailService = emailService;
            _mapper = mapper;
        }

        #endregion

        #region Actions

        [HttpGet]
        [AllowAnonymous]
        public async Task<IEnumerable<BlogPostShortViewModel>> GetAll()
        {
            var posts = await _unitOfWork.BlogPosts.GetAll(includeProperties: "Category,Author");
            return await GetPostsWithScratchIfUserValid<BlogPostShortViewModel>(posts.OrderByDescending(x=> x.PublishDate));
        }

        [HttpGet("slug/{slug}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetBySlug(string slug)
        {
            var post = await _unitOfWork.BlogPosts.GetFirstOrDefault(p => p.Slug == slug, includeProperties: "Category,Author,Next,Previous");
            var result = await GetPostsWithScratchIfUserValid<BlogPostViewModel>(post);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpGet("category/{id}")]
        [AllowAnonymous]
        public async Task<IEnumerable<BlogPostShortViewModel>> GetByCategory(int id)
        {
            var posts = await _unitOfWork.BlogPosts.GetAll(p => p.Category.Id == id, includeProperties: "Category,Author");
            return await GetPostsWithScratchIfUserValid<BlogPostShortViewModel>(posts);
        }

        [HttpGet("tag/{tag}")]
        [AllowAnonymous]
        public async Task<IEnumerable<BlogPostShortViewModel>> GetByTag(string tag)
        {
            var posts = await _unitOfWork.BlogPosts.GetAll(p => p.Tags.Contains(tag), includeProperties: "Category,Author");
            return await GetPostsWithScratchIfUserValid<BlogPostShortViewModel>(posts);
        }

        [HttpGet("{id}")]
        public async Task<BlogPostViewModel> Get(int id)
        {
            var post = await _unitOfWork.BlogPosts.GetFirstOrDefault(p => p.Id == id, includeProperties: "Category,Author,Next,Previous");
            return _mapper.Map<BlogPostViewModel>(post);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BlogPostViewModel post)
        {
            var objDb = _mapper.Map<BlogPostDto>(post);

            try
            {
                var email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var user = await _unitOfWork.ApplicationUsers
                    .GetFirstOrDefault(u => u.Email == email);

                objDb.AuthorId = user.Id;
                objDb.Slug = post.Title.GenerateSlug();
                objDb.PublishDate = DateTime.UtcNow;
                objDb.Content = _uploadsService.ParseAndSaveImages(objDb.Slug, post.Content);

                SetPropertiesFromModel(objDb, post);

                await _unitOfWork.BlogPosts.Add(objDb);
                await _unitOfWork.Save();

                if (post.Mailed && !post.Scratch)
                    await SendNotificationForSubscribers(post);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Cannot create blog post: {e.Message}");
                _uploadsService.Delete(objDb.ImageUrl);
                _uploadsService.DeleteBlogImages(objDb.Slug);
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
                var wasScratch = oldPost.Scratch;

                var objDb = _mapper.Map<BlogPostDto>(post);
                objDb.Slug = post.Title.GenerateSlug();
                objDb.Content = _uploadsService.ParseAndSaveImages(objDb.Slug, post.Content);

                SetPropertiesFromModel(objDb, post);

                await _unitOfWork.BlogPosts.Update(objDb);
                await _unitOfWork.Save();

                if(wasScratch && !post.Scratch && post.Mailed)
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

            _uploadsService.Delete(blogPost.ImageUrl);
            _uploadsService.DeleteBlogImages(blogPost.Slug);
            await _unitOfWork.BlogPosts.Remove(blogPost);
            await _unitOfWork.Save();

            return Ok();
        }

        #endregion

        #region Private Methods

        private void SetPropertiesFromModel(BlogPostDto objDb, BlogPostViewModel post)
        {
            var previousId = post?.Previous?.Id;
            var nextId = post?.Next?.Id;
            objDb.PreviousId = previousId == 0 ? null : previousId;
            objDb.NextId = nextId == 0 ? null : nextId;
            objDb.CategoryId = post.Category.Id;
            objDb.Previous = null;
            objDb.Next = null;
            objDb.Category = null;
            objDb.Tags = string.Join(',', post.Tags);
        }

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

        private async Task<T> GetPostsWithScratchIfUserValid<T>(BlogPostDto post)
        where T : class, IBlogPost
        {
            var result = await GetPostsWithScratchIfUserValid<T>(new[] {post});
            return result.FirstOrDefault();
        }

        private async Task<IEnumerable<T>> GetPostsWithScratchIfUserValid<T>
            (IEnumerable<BlogPostDto> posts)
        where T : class, IBlogPost
        {
            if (await ValidateUser())
                return posts?.Select(p => _mapper.Map<T>(p));

            return posts?.Where(p => !p.Scratch)?.Select(p => _mapper.Map<T>(p));
        }

        #endregion
    }
}
