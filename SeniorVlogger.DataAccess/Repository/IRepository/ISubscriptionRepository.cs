using System.Threading.Tasks;
using SeniorVlogger.Models.DTO;

namespace SeniorVlogger.DataAccess.Repository.IRepository
{
    public interface ISubscriptionRepository : IRepository<SubscriptionDto>
    {
        Task Update(SubscriptionDto subscription);
    }
}
