using System.Threading.Tasks;

namespace BikeScanner.Application.Interfaces
{
    public interface INotificationsSenderJob
    {
        Task Execute();
    }
}
