using BikeScanner.Domain.Models;
using System.Threading.Tasks;

namespace BikeScanner.Domain.Repositories
{
    public interface ISubscriptionsRepository : IRepository<SubscriptionEntity>
    {
        Task<SubscriptionEntity[]> GetSubs(long userId);
        Task<SubscriptionEntity[]> GetSubs();
        Task<bool> IsSubExists(long userId, string query);
    }
}
