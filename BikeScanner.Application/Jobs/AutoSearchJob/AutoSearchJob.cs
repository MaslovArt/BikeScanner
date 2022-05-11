using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using BikeScanner.Domain.Models;
using BikeScanner.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace BikeScanner.Application.Jobs
{
    public class AutoSearchJob : IAutoSearchJob
    {
        private readonly ILogger<AutoSearchJob> _logger;
        private readonly IContentsRepository _contentsRepository;
        private readonly ISubscriptionsRepository _subscriptionsRepository;
        private readonly INotificationsQueueRepository _notificationsQueueRepository;
        private readonly IVarsRepository _varsRepository;

        public AutoSearchJob(
            ILogger<AutoSearchJob> logger,
            IContentsRepository contentsRepository,
            ISubscriptionsRepository subscriptionsRepository,
            IVarsRepository varsRepository,
            INotificationsQueueRepository notificationsQueueRepository
            )
        {
            _logger = logger;
            _contentsRepository = contentsRepository;
            _subscriptionsRepository = subscriptionsRepository;
            _varsRepository = varsRepository;
            _notificationsQueueRepository = notificationsQueueRepository;
        }

        public async Task Execute()
        {
            var watch = new Stopwatch();
            watch.Start();

            try
            {
                var lastExecuteTime = (await _varsRepository.GetLastAutoSearchTime())
                    ?? new DateTime();

                _logger.LogInformation($"Starting auto search");

                var saveSearchQueries = await _subscriptionsRepository.GetSubs();
                if (saveSearchQueries.Length == 0)
                {
                    _logger.LogInformation($"No subscriptions. Skip search");
                    return;
                }

                var notifications = new List<NotificationQueueEntity>();
                foreach (var search in saveSearchQueries)
                {
                    var result = await _contentsRepository.Search(search.SearchQuery, 0, 100, lastExecuteTime);
                    var searchNotifications = result.Items.Select(c => new NotificationQueueEntity()
                    {
                        Text = $"Новый результат поиска '{search.SearchQuery}'\n\n{c.Url}",
                        UserId = search.UserId,
                        Status = NotificationStatus.Scheduled
                    });

                    notifications.AddRange(searchNotifications);
                }

                await _notificationsQueueRepository.AddRange(notifications);
                await _varsRepository.SetLastAutoSearchTime(DateTime.Now);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, $"Auth search error: {ex.Message}");
                throw;
            }
            finally
            {
                watch.Stop();
                _logger.LogInformation($"Finish auto search in {watch.Elapsed}");
            }
        }
    }
}

