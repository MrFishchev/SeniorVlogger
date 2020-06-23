using System.Threading.Tasks;
using SeniorVlogger.Models.DTO;

namespace SeniorVlogger.DataAccess.Repository.IRepository
{
    public interface ICategoryRepository : IRepository<CategoryDto>
    {
        Task Update(CategoryDto category);
    }
}
