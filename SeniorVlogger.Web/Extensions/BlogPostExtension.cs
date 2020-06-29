using System;
using SeniorVlogger.Models.DTO;
using SeniorVlogger.Models.ViewModels;

namespace SeniorVlogger.Web.Extensions
{
    public static class BlogPostExtension
    {
        public static BlogPostDto ToDto(this BlogPostViewModel vm)
        {
            var previousId = vm?.Previous?.Id;
            var nextId = vm?.Next?.Id;
            return new BlogPostDto
            {
                Id = vm.Id,
                CategoryId = vm.Category.Id,
                NextId = nextId == 0 ? null : nextId,
                PreviousId =  previousId == 0 ? null : previousId,
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
                PublishDate = objDb.PublishDate.Date.ToString("dd.MM.yyyy"),
                Next = new BlogPostViewModel
                {
                    Id = objDb.Next?.Id ?? 0, 
                    Title = objDb.Next?.Title,
                    Slug = objDb.Next?.Slug
                },
                Previous = new BlogPostViewModel
                {
                    Id = objDb.Previous?.Id ?? 0, 
                    Title = objDb.Previous?.Title,
                    Slug = objDb.Previous?.Slug
                }
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
