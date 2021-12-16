using System.Threading.Tasks;

namespace BikeScanner.Application.Jobs
{
    public interface INotificationsSenderJob
    {
        Task Execute();
    }
}
