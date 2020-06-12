using System.Threading.Tasks;
using SeniorVlogger.Models.DTO;

namespace SeniorVlogger.DataAccess.Repository.IRepository
{
    public interface IBlogPostRepository : IRepository<BlogPost>
    {
        Task Update(BlogPost blogPost);
    }
}
