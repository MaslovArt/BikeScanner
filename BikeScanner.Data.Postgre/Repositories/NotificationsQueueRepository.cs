using BikeScanner.Domain.Models;
using BikeScanner.Domain.Repositories;

namespace BikeScanner.Data.Postgre.Repositories
{
    public class NotificationsQueueRepository : BaseRepository<NotificationQueueEntity>, INotificationsQueueRepository
    {
        public NotificationsQueueRepository(BikeScannerContext context)
            : base(context)
        { }
    }
}
