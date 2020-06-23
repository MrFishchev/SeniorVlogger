using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SeniorVlogger.DataAccess.Data;
using SeniorVlogger.DataAccess.Repository.IRepository;
using SeniorVlogger.Models.DTO;

namespace SeniorVlogger.DataAccess.Repository
{
    public class SubscriptionRepository : Repository<SubscriptionDto>, ISubscriptionRepository
    {
        private readonly ApplicationDbContext _db;

        public SubscriptionRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task Update(SubscriptionDto subscription)
        {
            var objDb = await _db.Subscriptions
                .FirstOrDefaultAsync(i => i.Id == subscription.Id);
            
            if(objDb == null) return;

            if (objDb.IsSubscribed && !subscription.IsSubscribed)
            {
                //unsubscribe
                objDb.IsSubscribed = false;
                objDb.UnsubscribeDate = DateTime.UtcNow;
            }
            else if (!objDb.IsSubscribed && !subscription.IsSubscribed)
            {
                //subscribe one more time
                //TODO HelloAgainEmail ? 
                objDb.IsSubscribed = true;
            } 
        }
    }
}
