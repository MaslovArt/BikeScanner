using BikeScanner.Domain.Models;
using BikeScanner.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace BikeScanner.Data.Postgre.Repositories
{
    public class SubscriptionsRepository : BaseRepository<SubscriptionEntity>, ISubscriptionsRepository
    {
        public SubscriptionsRepository(BikeScannerContext context)
            : base(context)
        { }

        public Task<SubscriptionEntity[]> GetUserSubs(long userId, SubscriptionStatus status)
        {
            return Set
                .Where(e => e.UserId == userId &&
                            e.Status == status)
                .ToArrayAsync();
        }

        public Task<bool> HasActiveSub(long userId, string query)
        {
            return Set
                .AnyAsync(e => e.UserId == userId && 
                               e.SearchQuery == query &&
                               e.Status == SubscriptionStatus.Active);
        }
    }
}
