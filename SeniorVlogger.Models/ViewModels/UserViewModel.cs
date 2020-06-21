using SeniorVlogger.Common.Enums;

namespace SeniorVlogger.Models.ViewModels
{
    public class UserViewModel
    {
        public string Id { get; set; }

        public string Email { get; set; }

        public string Name { get; set; }

        public Role Role { get; set; }
    }
}
