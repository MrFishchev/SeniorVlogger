using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore.Internal;
using SeniorVlogger.Models;
using SeniorVlogger.Models.DTO;

namespace SeniorVlogger.Web.Extensions
{
    public static class BlogPostExtension
    {
        public static BlogPostDto ToDto(this BlogPost post)
        {
            return new BlogPostDto
            {
                CategoryId = post.Category.Id,
                NextId = post?.Next?.Id,
                PreviousId = post?.Previous?.Id,
                Title = post.Title,
                ImageUrl = post.ImageUrl,
                Content = post.Content,
                Description = post.Description,
                Mailed = post.Mailed,
                Scratch = post.Scratch,
                PublishDate = post.PublishDate,
                Tags = string.Join(',', post.Tags)
            };
        }
    }
}
