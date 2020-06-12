using SeniorVlogger.DataAccess.Data;
using SeniorVlogger.DataAccess.Repository.IRepository;
using SeniorVlogger.Models.DTO;

namespace SeniorVlogger.DataAccess.Repository
{
    public class BlogFileRepository : Repository<BlogFile>, IBlogFileRepository
    {
        private readonly ApplicationDbContext _db;

        public BlogFileRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
    }
}
