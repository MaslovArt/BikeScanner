using BikeScanner.Application.Interfaces;
using BikeScanner.Domain.Models;
using BikeScanner.Domain.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace BikeScanner.Application.Services
{
    public class ContentIndexator : IContentIndexator
    {
        private readonly ILogger<ContentIndexator>  _logger;
        private readonly IContentLoader[]           _contentLoaders;
        private readonly IContentsRepository        _contentsRepository;
        private readonly IVarsRepository            _varsRepository;

        public ContentIndexator(
            ILogger<ContentIndexator> logger,
            IEnumerable<IContentLoader> contentLoaders,
            IContentsRepository contentsRepository,
            IVarsRepository varsRepository)
        {
            _logger = logger;
            _contentLoaders = contentLoaders.ToArray();
            _contentsRepository = contentsRepository;
            _varsRepository = varsRepository;
        }

        public async Task Execute(DateTime loadSince)
        {
            var indexingWath = new Stopwatch();
            indexingWath.Start();

            var indexTime = DateTime.Now;
            _logger.LogInformation($"Starting indexing ({_contentLoaders.Length} providers)");

            try
            {
                var loadedContents = await LoadContents(loadSince);

                var currentContents = await _contentsRepository.Get();
                await RemoveOutdatedContent(currentContents, loadedContents, DateTime.Now.AddDays(-30));
                await AddNewContent(currentContents, loadedContents, indexTime);

                await _varsRepository.SetLastIndexingTime(indexTime);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            finally
            {
                indexingWath.Stop();
                _logger.LogInformation($"Finish indexing in {indexingWath.Elapsed}");
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

        private Task RemoveOutdatedContent(
            IEnumerable<ContentEntity> current, 
            IEnumerable<ContentEntity> loaded,
            DateTime expired)
        {
            var currentUrls = current.Select(m => m.Url);
            var loadedUrls = loaded.Select(c => c.Url);

            var deletedUrls = currentUrls.Except(loadedUrls);
            var deleted = current
                .Where(m => deletedUrls.Contains(m.Url) || 
                            m.Published.Date < expired.Date)
                .Select(m => new ContentEntity() { Id = m.Id });

            _logger.LogInformation($"Remove {deleted.Count()} records.");

            return _contentsRepository.RemoveRange(deleted);
        }

        private Task AddNewContent(
            IEnumerable<ContentEntity> current,
            IEnumerable<ContentEntity> loaded,
            DateTime stamp)
        {
            var currentUrls = current.Select(c => c.Url);
            var loadedUrls = loaded.Select(c => c.Url);

            var newUrls = loadedUrls.Except(currentUrls);
            var newEntities = loaded.Where(c => newUrls.Contains(c.Url));

            foreach (var newEntity in newEntities)
            {
                newEntity.IndexTime = stamp;
                newEntity.Text = newEntity.Text.ToUpper();
            }

            _logger.LogInformation($"Add {newEntities.Count()} records.");

            return _contentsRepository.AddRange(newEntities);
        }
    }
}
