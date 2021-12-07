using BikeScanner.Domain.Content;
using BikeScanner.Domain.Subscriptions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikeScanner.Application.Services
{
    public class NotificationsScheduler
    {
        private readonly ILogger<NotificationsScheduler>    _logger;
        private readonly IContentsRepository                _contentsRepository;
        private readonly ISubscriptionsRepository           _subscriptionsRepository;

        public NotificationsScheduler(
            ILogger<NotificationsScheduler> logger,
            IContentsRepository contentsRepository,
            ISubscriptionsRepository subscriptionsRepository)
        {
            _logger = logger;
            _contentsRepository = contentsRepository;
            _subscriptionsRepository = subscriptionsRepository;
        }

        public async Task Execute()
        {
            return;
        }
    }
}
