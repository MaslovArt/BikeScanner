using BikeScanner.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikeScanner.Domain.Subscriptions
{
    public interface ISubscriptionsRepository : IRepository<SubscriptionEntity>
    {
    }
}
