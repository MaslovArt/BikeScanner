using BikeScanner.Application.Interfaces;
using BikeScanner.Domain.Configs;
using BikeScanner.Domain.Extentions;
using BikeScanner.Domain.Models;
using BikeScanner.Domain.Repositories;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace BikeScanner.Application.Jobs
{
    public class ContentIndexatorJob : IContentIndexatorJob
    {
        private readonly ILogger<ContentIndexatorJob>  _logger;
        private readonly IContentLoader[]           _contentLoaders;
        private readonly IContentsRepository        _contentsRepository;
        private readonly IVarsRepository            _varsRepository;
        private readonly int                        _actualDays;

        public ContentIndexatorJob(
            ILogger<ContentIndexatorJob> logger,
            IEnumerable<IContentLoader> contentLoaders,
            IContentsRepository contentsRepository,
            IVarsRepository varsRepository,
            IOptions<BSSettings> options)
        {
            _logger = logger;
            _contentLoaders = contentLoaders.ToArray();
            _contentsRepository = contentsRepository;
            _varsRepository = varsRepository;
            _actualDays = options.Value.ActualDays;
        }

        public async Task Execute()
        {
            var loadSince = DateTime.Now.AddDays(-1 * _actualDays);

            var indexingWatch = new Stopwatch();
            indexingWatch.Start();

            var indexStamp = DateTime.Now.UnixStamp();
            _logger.LogInformation($"Starting indexing ({_contentLoaders.Length} providers)");

            try
            {
                var loadedContents = await LoadContents(loadSince);

                var currentContents = await _contentsRepository.Get();
                await RemoveOutdatedContent(currentContents, loadedContents, loadSince);
                await AddNewContent(currentContents, loadedContents, indexStamp);

                await _varsRepository.SetLastIndexingStamp(indexStamp);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, $"Indexing error: {ex.Message}");
            }
            finally
            {
                indexingWatch.Stop();
                _logger.LogInformation($"Finish indexing in {indexingWatch.Elapsed}");
            }
        }

        private async Task<ContentEntity[]> LoadContents(DateTime loadSince)
        {
            var timer = new Stopwatch();
            timer.Start();

            var loadTasks = _contentLoaders.Select(l => l.Load(loadSince));
            var tasksResults = await Task.WhenAll(loadTasks);
            var results = tasksResults.SelectMany(r => r).ToArray();

            var downloadTime = timer.Elapsed;
            _logger.LogInformation($"Download {results.Length} items in {downloadTime}");

            return results;
        }

        private async Task RemoveOutdatedContent(
            IEnumerable<ContentEntity> current, 
            IEnumerable<ContentEntity> loaded,
            DateTime expired)
        {
            var currentUrls = current.Select(m => m.Url);
            var loadedUrls = loaded.Select(c => c.Url);

            var deletedUrls = currentUrls.Except(loadedUrls);
            var deleted = current.Where(m => deletedUrls.Contains(m.Url) ||
                                             m.Published.Date < expired.Date);

            await _contentsRepository.RemoveRange(deleted);

            _logger.LogInformation($"Remove {deleted.Count()} records.");
        }

        private async Task AddNewContent(
            IEnumerable<ContentEntity> current,
            IEnumerable<ContentEntity> loaded,
            long stamp)
        {
            var currentUrls = current.Select(c => c.Url);
            var loadedUrls = loaded.Select(c => c.Url);

            var newUrls = loadedUrls.Except(currentUrls);
            var newEntities = loaded.Where(c => newUrls.Contains(c.Url));

            foreach (var newEntity in newEntities)
            {
                newEntity.IndexingStamp = stamp;
            }

            await _contentsRepository.AddRange(newEntities);

            _logger.LogInformation($"Add {newEntities.Count()} records.");
        }
    }
}
