using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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

        public async Task Update(CategoryDto category)
        {
            var objDb = await _db.Categories
                .FirstOrDefaultAsync(c => c.Id == category.Id);

            if(objDb == null) return;
            objDb.Name = category.Name;
        }
    }
}
