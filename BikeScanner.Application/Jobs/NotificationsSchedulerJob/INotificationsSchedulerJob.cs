using System.Threading.Tasks;

namespace BikeScanner.Application.Jobs
{
    public interface INotificationsSchedulerJob
    {
        Task Execute();
    }
}