using BikeScanner.Domain.Models;
using BikeScanner.Domain.Repositories;

namespace BikeScanner.Data.Postgre.Repositories
{
    public class SubscriptionsRepository : BaseRepository<SubscriptionEntity>, ISubscriptionsRepository
    {
        public SubscriptionsRepository(BikeScannerContext context)
            : base(context)
        { }
    }
}
