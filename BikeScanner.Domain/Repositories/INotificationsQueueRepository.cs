using BikeScanner.Domain.Models;
using System.Threading.Tasks;

namespace BikeScanner.Domain.Repositories
{
    public interface INotificationsQueueRepository : IRepository<NotificationQueueEntity>
    {
        Task<NotificationQueueEntity[]> GetScheduled();
    }
}
