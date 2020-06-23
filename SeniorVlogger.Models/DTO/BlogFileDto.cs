using System.ComponentModel.DataAnnotations;

namespace SeniorVlogger.Models.DTO
{
    public class BlogFileDto
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Filename { get; set; }

        [MaxLength(30)]
        public string MimeType { get; set; }

        [MaxLength(30)]
        public string Encoding { get; set; }

        [Required]
        public string Url { get; set; }
    }
}
