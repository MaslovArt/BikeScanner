using BikeScanner.Domain.NotificationsQueue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikeScanner.Data.Postgre.Repositories
{
    public class NotificationsQueueRepository : BaseRepository<NotificationQueueEntity>, INotificationsQueueRepository
    {
        public NotificationsQueueRepository(BikeScannerContext context)
            : base(context)
        { }
    }
}
