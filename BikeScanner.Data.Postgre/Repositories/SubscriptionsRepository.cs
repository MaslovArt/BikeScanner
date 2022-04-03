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

        public Task<SubscriptionEntity[]> GetSubs()
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
                        UserAccountStatus = u.AccountStatus
                    })
                .Where(e => e.UserAccountStatus == AccountStatus.Active)
                .Select(e => new SubscriptionEntity()
                {
                    Id = e.Id,
                    UserId = e.UserId,
                    Created = e.Created,
                    SearchQuery = e.SearchQuery
                })
                .ToArrayAsync();
        }

        public Task<SubscriptionEntity[]> GetSubs(long userId)
        {
            return Set
                .Where(e => e.UserId == userId)
                .ToArrayAsync();
        }

        public Task<bool> IsSubExists(long userId, string query)
        {
            return Set
                .AnyAsync(e => e.UserId == userId && 
                               e.SearchQuery == query);
        }
    }
}
