using BikeScanner.Application.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace BikeScanner.Application.Services
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
            return _notificators.Single(n => n.CanHandle(type));
        }
    }
}
