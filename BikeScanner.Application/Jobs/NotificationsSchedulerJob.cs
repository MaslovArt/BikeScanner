using BikeScanner.Application.Interfaces;
using BikeScanner.Domain.Models;
using BikeScanner.Domain.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace BikeScanner.Application.Jobs
{
    public class NotificationsSchedulerJob : INotificationsSchedulerJob
    {
        private readonly ILogger<NotificationsSchedulerJob>    _logger;
        private readonly IContentsRepository                _contentsRepository;
        private readonly ISubscriptionsRepository           _subsRepository;
        private readonly INotificationsQueueRepository      _notificationsQueueRepository;
        private readonly IVarsRepository                    _varsRepository;

        public NotificationsSchedulerJob(
            ILogger<NotificationsSchedulerJob> logger,
            IContentsRepository contentsRepository,
            ISubscriptionsRepository subscriptionsRepository,
            IVarsRepository varsRepository,
            INotificationsQueueRepository notificationsQueueRepository)
        {
            _logger = logger;
            _contentsRepository = contentsRepository;
            _subsRepository = subscriptionsRepository;
            _varsRepository = varsRepository;
            _notificationsQueueRepository = notificationsQueueRepository;
        }

        public async Task Execute()
        {
            _logger.LogInformation("Start scheduling.");

            var schedulerWatch = new Stopwatch();
            schedulerWatch.Start();

            try
            {
                var lastIndexing = await _varsRepository.GetLastIndexingStamp();
                if (!lastIndexing.HasValue)
                {
                    _logger.LogWarning($"Stop scheduling: no indexing stamp.");
                    return;
                }

                var lastScheduling = await _varsRepository.GetLastSchedulingStamp();
                if (lastIndexing == lastScheduling)
                {
                    _logger.LogWarning($"Stop scheduling: scheduling same stamp. {lastScheduling}");
                    return;
                }

                await Schedule(lastIndexing.Value);

                await _varsRepository.SetLastSchedulingStamp(lastIndexing.Value);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, $"scheduling error: ${ex}");
            }
            finally
            {
                schedulerWatch.Stop();
                _logger.LogInformation($"Finish scheduling in {schedulerWatch.Elapsed}");
            }
        }

        private async Task Schedule(long lastIndexingStamp)
        {
            var subscriptions = await _subsRepository.Get();
            _logger.LogInformation($"Subs count {subscriptions.Length}");

            var usersNotifications = new List<NotificationQueueEntity>();
            foreach (var sub in subscriptions)
            {
                //ToDo load all then try find
                var results = await _contentsRepository.Search(sub.SearchQuery, lastIndexingStamp);
                var userNotifications = results.Select(r => new NotificationQueueEntity()
                {
                    SearchQuery = sub.SearchQuery,
                    ContentUrl = r.Url,
                    UserId = sub.UserId,
                    Type = sub.Type
                });
                usersNotifications.AddRange(userNotifications);
            }

            await _notificationsQueueRepository.AddRange(usersNotifications);
            _logger.LogInformation($"Schedule {usersNotifications.Count} notifications");
        }
    }
}
