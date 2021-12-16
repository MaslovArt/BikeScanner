using BikeScanner.Application.Services.NotificationFactory;
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
                var notifications = await _notificationsQueueRepository.GetNotSended();
                var typeNotifications = notifications.GroupBy(q => q.NotificationType);

                foreach (var group in typeNotifications)
                {
                    await SendNotifications(group.Key, group);
                }

                await _notificationsQueueRepository.UpdateRange(notifications);
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
            _logger.LogInformation($"[{type}] notifications count {notifications.Count()}");

            var notificationTasks = notifications
                .Select(notification => 
                    //ToDO Task.Run???
                    Task.Run(() => notificator.Send(notification.UserId, notification.AdUrl))
                    .ContinueWith(notificationTask =>
                    {
                        if (notificationTask.IsCompletedSuccessfully)
                        {
                            notification.SendTime = DateTime.Now;
                            notification.Status = NotificationStatus.Sended;
                        }
                        else if (notificationTask.IsFaulted)
                        {
                            notification.SendTime = DateTime.Now;
                            notification.Status = NotificationStatus.Error;

                            var ex = notificationTask.Exception;
                            _logger.LogError(ex, $"User [{notification.UserId}] notification({notification.NotificationType}) err: {ex.Message}");
                        }
                    }));

            await Task.WhenAll(notificationTasks);
        }
    }
}
