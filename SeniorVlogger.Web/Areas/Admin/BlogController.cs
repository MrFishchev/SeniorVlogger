using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SeniorVlogger.DataAccess.Repository.IRepository;
using SeniorVlogger.Models.DTO;

namespace SeniorVlogger.Web.Areas.Admin
{
    [Area("Admin")]
    [Authorize]
    public class BlogController : Controller
    {
        private readonly ILogger<BlogController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;

        public BlogController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment, ILogger<BlogController> logger)
        {
            _unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var allObj = await _unitOfWork.BlogPosts
                .GetAll(includeProperties: "Next,Previous");
            return Json(new { data = allObj });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var blogPost = await _unitOfWork.BlogPosts.Get(id);
            if (blogPost == null) return Json(new { success = false, message = "Cannot delete product" });

            //TODO DeleteOldImage
            await _unitOfWork.BlogPosts.Remove(blogPost);
            await _unitOfWork.Save();

            return Json(new { success = true, message = "Cannot delete product" });
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody]BlogPost blogPost)
        {
            await Task.Delay(10);
            return Json(new {data = blogPost});
        }
    }
}
