using BikeScanner.Domain.Models;
using BikeScanner.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace BikeScanner.Data.Postgre.Repositories
{
    public class NotificationsQueueRepository : BaseRepository<NotificationQueueEntity>, INotificationsQueueRepository
    {
        public NotificationsQueueRepository(BikeScannerContext context)
            : base(context)
        { }

        public Task<NotificationQueueEntity[]> GetScheduled()
        {
            return Set
                .Where(e => e.Status == NotificationStatus.Scheduled)
                .ToArrayAsync();
        }
    }
}
