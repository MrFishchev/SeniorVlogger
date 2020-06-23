using System.Threading.Tasks;
using SeniorVlogger.Models.DTO;

namespace SeniorVlogger.DataAccess.Repository.IRepository
{
    public interface IBlogPostRepository : IRepository<BlogPostDto>
    {
        Task Update(BlogPostDto blogPost);
    }
}
