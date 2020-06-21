using System;
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
                PublishDate = DateTime.Parse(post.PublishDate),
                Tags = string.Join(',', post.Tags)
            };
        }

        public static BlogPost ToViewModel(this BlogPostDto objDb)
        {
            var viewModel = new BlogPost
            {
                Id = objDb.Id,
                ImageUrl = objDb.ImageUrl,
                Tags = objDb.Tags.Split(','),
                Title = objDb.Title,
                Slug = objDb.Slug,
                Content = objDb.Content,
                Description = objDb.Description,
                Mailed = objDb.Mailed,
                Scratch = objDb.Scratch,
                PublishDate = objDb.PublishDate.Date.ToString("dd.MM.yyyy")
            };

            if (objDb.Author != null)
            {
                viewModel.Author = new User
                {
                    Id = objDb.Author.Id,
                    Email = objDb.Author.Email,
                    Name = objDb.Author.UserName
                };
            }

            if (objDb.Category != null)
            {
                viewModel.Category = new Category
                {
                    Id = objDb.Category.Id,
                    Name = objDb.Category.Name
                };
            }

            return viewModel;
        }
    }
}
