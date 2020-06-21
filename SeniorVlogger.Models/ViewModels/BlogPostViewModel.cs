using System.Collections.Generic;

namespace SeniorVlogger.Models.ViewModels
{
    public class BlogPostViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Slug { get; set; }

        public IEnumerable<string> Tags { get; set; }

        public string ImageUrl { get; set; }

        public string Content { get; set; }

        public string Description { get; set; }

        public CategoryViewModel Category { get; set; }

        public string PublishDate { get; set; }

        public bool Mailed { get; set; }

        public bool Scratch { get; set; }

        public BlogPostViewModel Next { get; set; }

        public BlogPostViewModel Previous { get; set; }

        public UserViewModel Author { get; set; }
    }
}
