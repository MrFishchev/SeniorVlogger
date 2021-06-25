using SeniorVlogger.Common.Enums;

namespace SeniorVlogger.Models.ViewModels
{
    public class UserViewModel
    {
        public string Id { get; set; }

        public string Email { get; set; }

        public bool EmailConfirmed { get; set; }
        
        public bool IsLocked { get; set; }

        public string Role { get; set; }
    }
}
