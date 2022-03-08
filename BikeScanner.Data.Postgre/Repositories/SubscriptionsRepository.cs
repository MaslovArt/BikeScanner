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

        public Task<SubscriptionEntity[]> GetActiveSubs()
        {
            return Set
                .Join(
                    Context.Users,
                    n => n.UserId,
                    u => u.UserId,
                    (n, u) => new
                    {
                        Id = n.Id,
                        UserId = n.UserId,
                        Created = n.Created,
                        SearchQuery = n.SearchQuery,
                        Status = n.Status,
                        UserAccountStatus = u.AccountStatus
                    })
                .Where(e => e.Status == SubscriptionStatus.Active &&
                            e.UserAccountStatus == AccountStatus.Active)
                .Select(e => new SubscriptionEntity()
                {
                    Id = e.Id,
                    UserId = e.UserId,
                    Created = e.Created,
                    SearchQuery = e.SearchQuery,
                    Status = e.Status,
                })
                .ToArrayAsync();
        }

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
