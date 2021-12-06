using BikeScanner.Application.Interfaces;
using BikeScanner.Domain.Content;
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
        private readonly ILogger<ContentIndexator> _logger;
        private readonly IContentLoader[] _contentLoaders;
        private readonly IContentsRepository _contentsRepository;

        public ContentIndexator(
            ILogger<ContentIndexator> logger,
            IEnumerable<IContentLoader> contentLoaders,
            IContentsRepository contentsRepository)
        {
            _logger = logger;
            _contentLoaders = contentLoaders.ToArray();
            _contentsRepository = contentsRepository;
        }

        public async Task Execute(DateTime loadSince)
        {
            _logger.LogInformation($"Starting indexing ({_contentLoaders.Length} providers)");

            var timer = new Stopwatch();
            timer.Start();

            var loadTasks = _contentLoaders.Select(l => l.Load(loadSince));
            var tasksResults = await Task.WhenAll(loadTasks);
            var results = tasksResults.SelectMany(r => r);
            var downloadTime = timer.Elapsed;
            _logger.LogInformation($"Download {results.Count()} records in {downloadTime}");

            var changeCount = await _contentsRepository.Refresh(results, loadSince);
            var updateTime = timer.Elapsed - downloadTime;
            _logger.LogInformation($"Update {changeCount} db items in {updateTime}");

            timer.Stop();
            _logger.LogInformation($"Finish indexing in {timer.Elapsed}");
        }
    }
}
