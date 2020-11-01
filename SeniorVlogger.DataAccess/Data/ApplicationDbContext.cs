using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SeniorVlogger.Models.DTO;

namespace SeniorVlogger.DataAccess.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<BlogPostDto> BlogPosts { get; set; }
        public DbSet<CategoryDto> Categories { get; set; }
        public DbSet<SubscriptionDto> Subscriptions { get; set; }
    }
}
