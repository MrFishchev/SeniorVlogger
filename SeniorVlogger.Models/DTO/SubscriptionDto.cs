using System;
using System.ComponentModel.DataAnnotations;

namespace SeniorVlogger.Models.DTO
{
    public class SubscriptionDto
    {
        public int Id { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public bool IsSubscribed { get; set; }

        [Required]
        public DateTime SubscribeDate { get; set; }

        public DateTime? UnsubscribeDate { get; set; }
    }
}
