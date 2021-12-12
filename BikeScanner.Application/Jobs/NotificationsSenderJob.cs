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
    public class NotificationsSenderJob : INotificationsSenderJob
    {
        private readonly ILogger<NotificationsSenderJob>    _logger;
        private readonly INotificationsQueueRepository      _notificationsQueueRepository;
        private readonly INotificatorFactory                _notificatorFactory;
        private readonly int                                _notificationChunkSize = 50;

        public NotificationsSenderJob(
            ILogger<NotificationsSenderJob> logger,
            INotificationsQueueRepository notificationsQueueRepository,
            INotificatorFactory notificatorFactory
            )
        {
            _logger = logger;
            _notificationsQueueRepository = notificationsQueueRepository;
            _notificatorFactory = notificatorFactory;
        }

        public async Task Execute()
        {
            var notifyWatch = new Stopwatch();
            notifyWatch.Start();

            _logger.LogInformation("Start notifying");
            try
            {
                var notifications = await _notificationsQueueRepository.Get();
                var typeNotifications = notifications.GroupBy(q => q.Type);

                foreach (var group in typeNotifications)
                {
                    await SendNotifications(group.Key, group);
                }

                await _notificationsQueueRepository.RemoveRange(notifications);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, $"Notifying error: {ex.Message}");
            }
            finally
            {
                notifyWatch.Stop();
                _logger.LogInformation($"Finish notifying in {notifyWatch.Elapsed}");
            }
        }

        private async Task SendNotifications(string type, IEnumerable<NotificationQueueEntity> notifications)
        {
            var notificator = _notificatorFactory.Resolve(type);
            _logger.LogInformation($"[{type}] notification count {notifications.Count()}");

            var notificationsChunks = notifications
                .Select((n, ind) => new 
                { 
                    Notification = n, 
                    ChunkNum = ind / _notificationChunkSize 
                })
                .GroupBy(n => n.ChunkNum);

            foreach (var chunk in notificationsChunks)
            {
                var notificationTasks = chunk
                    .Select(n => notificator.Send(n.Notification.UserId, n.Notification.ContentUrl));
                await Task.WhenAll(notificationTasks);
            }
        }
    }
}
