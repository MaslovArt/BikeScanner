using BikeScanner.Application.Interfaces;

namespace BikeScanner.Application.Interfaces
{
    public interface INotificatorFactory
    {
        INotificator Resolve(string type);
    }
}