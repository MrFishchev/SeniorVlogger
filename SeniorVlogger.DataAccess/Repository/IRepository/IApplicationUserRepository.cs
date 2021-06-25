using System.Threading.Tasks;
using SeniorVlogger.Models.DTO;

namespace SeniorVlogger.DataAccess.Repository.IRepository
{
    public interface IApplicationUserRepository : IRepository<ApplicationUser>
    {
        Task Update(ApplicationUser user);
    }
}
