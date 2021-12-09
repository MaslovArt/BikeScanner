using System.Threading.Tasks;

namespace BikeScanner.Application.Interfaces
{
    public interface INotificationsScheduler
    {
        Task Execute();
    }
}