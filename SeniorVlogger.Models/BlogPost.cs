using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SeniorVlogger.Models
{
    public class BlogPost
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Slug { get; set; }

        public IEnumerable<string> Tags { get; set; }

        public string ImageUrl { get; set; }

        public string Content { get; set; }

        public string Description { get; set; }

        public Category Category { get; set; }

        public string PublishDate { get; set; }

        public bool Mailed { get; set; }

        public bool Scratch { get; set; }

        public BlogPost Next { get; set; }

        public BlogPost Previous { get; set; }

        public User Author { get; set; }
    }
}
