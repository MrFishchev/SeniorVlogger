using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SeniorVlogger.DataAccess.Data;
using SeniorVlogger.DataAccess.Repository.IRepository;
using SeniorVlogger.Models.DTO;

namespace SeniorVlogger.DataAccess.Repository
{
    public class BlogPostRepository : Repository<BlogPostDto>, IBlogPostRepository
    {
        private readonly ApplicationDbContext _db;

        public BlogPostRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task Update(BlogPostDto blogPost)
        {
            var objDb = await _db.BlogPosts
                .FirstOrDefaultAsync(d => d.Id == blogPost.Id);

            if(objDb == null) return;
            objDb.Title = blogPost.Title;
            objDb.Slug = blogPost.Slug;
            objDb.Tags = blogPost.Tags;
            objDb.Content = blogPost.Content;
            objDb.Description = blogPost.Description;
            objDb.Category = blogPost.Category;
            objDb.PublishDate = blogPost.PublishDate;
            objDb.Mailed = blogPost.Mailed;
            objDb.Scratch = blogPost.Scratch;
            objDb.NextId = blogPost.NextId;
            objDb.PreviousId = blogPost.PreviousId;

            if (objDb.ImageUrl != blogPost.ImageUrl)
            {
                //TODO delete old image and load new
                objDb.ImageUrl = blogPost.ImageUrl;
            }
        }
    }
}
