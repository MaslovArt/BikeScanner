using BikeScanner.Domain.Subscriptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikeScanner.Data.Postgre.Repositories
{
    public class SubscriptionsRepository : BaseRepository<SubscriptionEntity>, ISubscriptionsRepository
    {
        public SubscriptionsRepository(BikeScannerContext context)
            : base(context)
        { }
    }
}
