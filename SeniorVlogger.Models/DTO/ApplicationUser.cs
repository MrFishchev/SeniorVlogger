using SeniorVlogger.Common.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace SeniorVlogger.Models.DTO
{
    public class ApplicationUser : IdentityUser
    {
        [NotMapped] 
        public Role Role { get; set; }
    }
}
