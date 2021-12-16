using BikeScanner.Application.Interfaces;
using BikeScanner.Domain.Exceptions;
using System.Collections.Generic;
using System.Linq;

namespace BikeScanner.Application.Services.NotificationFactory
{
    public class NotificatorFactory : INotificatorFactory
    {
        private readonly INotificator[] _notificators;

        public NotificatorFactory(IEnumerable<INotificator> notificators)
        {
            _notificators = notificators.ToArray();
        }

        public INotificator Resolve(string type)
        {
            var notificator = _notificators
                .FirstOrDefault(n => n.CanHandle(type));

            return notificator ?? throw new NotificatorTypeException(type);
        }
    }
}
