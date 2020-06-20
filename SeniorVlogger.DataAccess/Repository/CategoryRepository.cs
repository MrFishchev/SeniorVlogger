using System.Threading.Tasks;
using SeniorVlogger.DataAccess.Data;
using SeniorVlogger.DataAccess.Repository.IRepository;
using SeniorVlogger.Models.DTO;

namespace SeniorVlogger.DataAccess.Repository
{
    public class CategoryRepository : Repository<CategoryDto>, ICategoryRepository
    {
        private readonly ApplicationDbContext _db;

        public CategoryRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public Task Update(CategoryDto category)
        {
            _db.Categories.Update(category);
            return Task.CompletedTask;
        }
    }
}
