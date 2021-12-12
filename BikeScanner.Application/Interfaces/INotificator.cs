using System.Threading.Tasks;

namespace BikeScanner.Application.Interfaces
{
    public interface INotificator
    {
        Task Send(long userId, string message);
        bool CanHandle(string type);
    }
}
