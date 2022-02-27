using BikeScanner.Application.Services.SearchService;
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
    public class AutoSearchJob : IAutoSearchJob
    {
        private readonly ILogger<AutoSearchJob> _logger;
        private readonly ISearchService                     _searchService;
        private readonly ISubscriptionsRepository           _subsRepository;
        private readonly INotificationsQueueRepository      _notificationsQueueRepository;
        private readonly IVarsRepository                    _varsRepository;

        public AutoSearchJob(
            ILogger<AutoSearchJob> logger,
            ISearchService searchService,
            ISubscriptionsRepository subscriptionsRepository,
            IVarsRepository varsRepository,
            INotificationsQueueRepository notificationsQueueRepository)
        {
            _logger = logger;
            _searchService = searchService;
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
                var indexEpoch = await _varsRepository.GetLastIndexEpoch();
                if (!indexEpoch.HasValue)
                {
                    _logger.LogWarning($"Stop scheduling: no index epoch.");
                    return;
                }

                var lastScheduling = await _varsRepository.GetLastScheduleEpoch();
                if (indexEpoch == lastScheduling)
                {
                    _logger.LogWarning($"Stop scheduling: scheduling same index epoch. {lastScheduling}");
                    return;
                }

                await Schedule(indexEpoch.Value);

                await _varsRepository.SetLastScheduleEpoch(indexEpoch.Value);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, $"scheduling error: ${ex}");
                throw;
            }
            finally
            {
                schedulerWatch.Stop();
                _logger.LogInformation($"Finish scheduling in {schedulerWatch.Elapsed}");
            }
        }

        private async Task Schedule(long indexEpoch)
        {
            var subscriptions = await _subsRepository.GetActiveSubs();
            _logger.LogInformation($"Subs count {subscriptions.Length}");

            var usersNotifications = new List<NotificationQueueEntity>();
            foreach (var sub in subscriptions)
            {
                var results = await _searchService.SearchEpoch(sub.UserId, sub.SearchQuery, indexEpoch);
                var userNotifications = results.Select(r => new NotificationQueueEntity()
                {
                    SearchQuery = sub.SearchQuery,
                    AdUrl = r.AdUrl,
                    UserId = sub.UserId,
                    NotificationType = sub.NotificationType
                });
                usersNotifications.AddRange(userNotifications);
            }

            await _notificationsQueueRepository.AddRange(usersNotifications);
            _logger.LogInformation($"Schedule {usersNotifications.Count} notifications");
        }
    }
}
