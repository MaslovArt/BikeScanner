using System.Threading.Tasks;

namespace BikeScanner.Application.Jobs
{
	/// <summary>
    /// Crawling => AutoSearch => Notifications
    /// </summary>
	public class CSNJob
	{
		private readonly IAdditionalCrawlingJob		_additionalCrawlingJob;
		private readonly IAutoSearchJob				_autoSearchJob;
		private readonly INotificationsSenderJob	_notificationsSenderJob;

		public CSNJob(
			IAdditionalCrawlingJob additionalCrawlingJob,
			IAutoSearchJob autoSearchJob,
			INotificationsSenderJob notificationsSenderJob
			)
		{
			_additionalCrawlingJob = additionalCrawlingJob;
			_autoSearchJob = autoSearchJob;
			_notificationsSenderJob = notificationsSenderJob;
		}

		public async Task Execute()
        {
			await _additionalCrawlingJob.Execute();
			await _autoSearchJob.Execute();
			await _notificationsSenderJob.Execute();
        }
	}
}

