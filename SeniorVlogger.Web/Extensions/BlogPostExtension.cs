using System;
using SeniorVlogger.Models.DTO;
using SeniorVlogger.Models.ViewModels;

namespace SeniorVlogger.Web.Extensions
{
    public static class BlogPostExtension
    {
        public static BlogPostDto ToDto(this BlogPostViewModel vm)
        {
            return new BlogPostDto
            {
                Id = vm.Id,
                CategoryId = vm.Category.Id,
                NextId = vm?.Next?.Id,
                PreviousId = vm?.Previous?.Id,
                Title = vm.Title,
                ImageUrl = vm.ImageUrl,
                Content = vm.Content,
                Description = vm.Description,
                Mailed = vm.Mailed,
                Scratch = vm.Scratch,
                PublishDate = vm.PublishDate != null ?  DateTime.Parse(vm.PublishDate) : DateTime.MinValue,
                Tags = string.Join(',', vm.Tags)
            };
        }

        public static BlogPostViewModel ToViewModel(this BlogPostDto objDb)
        {
            var viewModel = new BlogPostViewModel
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
                viewModel.Author = new UserViewModel
                {
                    Id = objDb.Author.Id,
                    Email = objDb.Author.Email,
                    Name = objDb.Author.UserName
                };
            }

            if (objDb.Category != null)
            {
                viewModel.Category = new CategoryViewModel
                {
                    Id = objDb.Category.Id,
                    Name = objDb.Category.Name
                };
            }

            return viewModel;
        }
    }
}
