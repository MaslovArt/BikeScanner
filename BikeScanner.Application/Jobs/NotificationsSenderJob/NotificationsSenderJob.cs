using BikeScanner.Application.Interfaces;
using BikeScanner.Domain.Models;
using BikeScanner.Domain.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace BikeScanner.Application.Jobs
{
    public class NotificationsSenderJob : INotificationsSenderJob
    {
        private readonly ILogger<NotificationsSenderJob>    _logger;
        private readonly INotificationsQueueRepository      _notificationsQueueRepository;
        private readonly INotificator                       _notificator;
        private readonly int                                _notificationBacthSize = 30;

        public NotificationsSenderJob(
            ILogger<NotificationsSenderJob> logger,
            INotificationsQueueRepository notificationsQueueRepository,
            INotificator notificator
            )
        {
            _logger = logger;
            _notificationsQueueRepository = notificationsQueueRepository;
            _notificator = notificator;
        }

        public async Task Execute()
        {
            var notifyWatch = new Stopwatch();
            notifyWatch.Start();

            _logger.LogInformation("Start notifying");
            try
            {
                var notifications = await _notificationsQueueRepository.GetScheduled();
                _logger.LogInformation($"Notifications count {notifications.Count()}");

                var batches = (int)Math.Ceiling((double)notifications.Count() / _notificationBacthSize);

                for (int i = 0; i < batches; i++)
                {
                    var batchNotifications = notifications
                        .Skip(i * _notificationBacthSize)
                        .Take(_notificationBacthSize);
                    var notificationTasks = notifications
                        .Select(async notification =>
                        {
                            try
                            {
                                await _notificator.Send(notification.UserId, notification.Text);
                                notification.SendTime = DateTime.UtcNow;
                                notification.Status = NotificationStatus.Sended;
                            }
                            catch (Exception ex)
                            {
                                notification.SendTime = DateTime.UtcNow;
                                notification.Status = NotificationStatus.Error;
                                _logger.LogError(ex, $"User [{notification.UserId}] notification err: {ex.Message}");
                            }
                        });

                    await Task.WhenAll(notificationTasks);
                }
                await _notificationsQueueRepository.UpdateRange(notifications);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, $"Notifying error: {ex.Message}");
                throw;
            }
            finally
            {
                notifyWatch.Stop();
                _logger.LogInformation($"Finish notifying in {notifyWatch.Elapsed}");
            }
        }
    }
}
