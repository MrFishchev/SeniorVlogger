using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SeniorVlogger.DataAccess.Data;
using SeniorVlogger.DataAccess.Repository.IRepository;
using SeniorVlogger.Models.DTO;

namespace SeniorVlogger.DataAccess.Repository
{
    public class ApplicationUserRepository : Repository<ApplicationUser>, IApplicationUserRepository
    {
        private readonly ApplicationDbContext _db;

        public ApplicationUserRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task Update(ApplicationUser user)
        {
            var objDb = await _db.ApplicationUsers
                .FirstOrDefaultAsync(c => c.Id == user.Id);

            if(objDb == null) return;
            objDb.Role = user.Role;
        }
    }
}
