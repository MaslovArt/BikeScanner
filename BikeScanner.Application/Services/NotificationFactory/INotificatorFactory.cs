using BikeScanner.Application.Interfaces;

namespace BikeScanner.Application.Services.NotificationFactory
{
    public interface INotificatorFactory
    {
        INotificator Resolve(string type);
    }
}