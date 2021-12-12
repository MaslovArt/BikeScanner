using System.Threading.Tasks;

namespace BikeScanner.Application.Interfaces
{
    public interface INotificationsSchedulerJob
    {
        Task Execute();
    }
}