using System.Threading.Tasks;

namespace BikeScanner.Application.Jobs
{
    /// <summary>
    /// Jobs chain: ContentIndexing => AutoSearch => Notifying
    /// </summary>
    public class ScanJobsChain
	{
        private readonly IAutoSearchJob             _autoSearchJob;
        private readonly IContentIndexatorJob       _contentIndexatorJob;
        private readonly INotificationsSenderJob    _notificationsSenderJob;

        public ScanJobsChain(
            IAutoSearchJob autoSearchJob,
            IContentIndexatorJob contentIndexatorJob,
            INotificationsSenderJob notificationsSenderJob
            )
        {
            _autoSearchJob = autoSearchJob;
            _contentIndexatorJob = contentIndexatorJob;
            _notificationsSenderJob = notificationsSenderJob;
        }

        public async Task ExecuteChain()
        {
            await _contentIndexatorJob.Execute();
            await _autoSearchJob.Execute();
            await _notificationsSenderJob.Execute();
        }
	}
}

