using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SeniorVlogger.Models.DTO
{
    public class BlogPost
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; }

        [Required]
        [MaxLength(50)]
        public string Slug { get; set; }

        public string Tags { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [MaxLength(50)]
        public string Category { get; set; }

        [Required]
        public DateTime PublishDate { get; set; }

        public bool Mailed { get; set; }

        public bool Scratch { get; set; }

        public int? NextId { get; set; }
        [ForeignKey(nameof(NextId))]
        public BlogPost Next { get; set; }

        public int? PreviousId { get; set; }
        [ForeignKey(nameof(PreviousId))]
        public BlogPost Previous { get; set; }

        [Required]
        public string AuthorId { get; set; }
        [ForeignKey(nameof(AuthorId))]
        public ApplicationUser Author { get; set; }

    }
}
