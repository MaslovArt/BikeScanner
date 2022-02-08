using BikeScanner.Domain.Models;
using System.Threading.Tasks;

namespace BikeScanner.Domain.Repositories
{
    public interface ISubscriptionsRepository : IRepository<SubscriptionEntity>
    {
        Task<SubscriptionEntity[]> GetUserSubs(long userId, SubscriptionStatus status);

        Task<bool> HasActiveSub(long userId, string query);
    }
}
