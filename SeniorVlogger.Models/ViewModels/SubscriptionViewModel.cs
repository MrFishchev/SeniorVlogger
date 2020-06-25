using System;

namespace SeniorVlogger.Models.ViewModels
{
    public class SubscriptionViewModel
    {
        public int Id { get; set; }

        public string Email { get; set; }

        public bool IsSubscribed { get; set; }

        public DateTime SubscribeDate { get; set; }

        public DateTime? UnsubscribeDate { get; set; }
    }
}
