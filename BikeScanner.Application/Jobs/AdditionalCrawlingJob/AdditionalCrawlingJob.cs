using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BikeScanner.Application.Configs;
using BikeScanner.Application.Interfaces;
using BikeScanner.Application.Models;
using BikeScanner.Domain.Models;
using BikeScanner.Domain.Repositories;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BikeScanner.Application.Jobs
{
    public class AdditionalCrawlingJob : IAdditionalCrawlingJob
    {
        private readonly ILogger<AdditionalCrawlingJob> _logger;
        private readonly ICrawler[] _crawlers;
        private readonly IContentsRepository _contentsRepository;
        private readonly IVarsRepository _varsRepository;
        private readonly ContentConfig _contentConfig;
        private readonly IMapper _mapper;

        public AdditionalCrawlingJob(
            ILogger<AdditionalCrawlingJob> logger,
            IEnumerable<ICrawler> crawlers,
            IContentsRepository contentsRepository,
            IVarsRepository varsRepository,
            IOptions<ContentConfig> contentOptions,
            IMapper mapper
            )
        {
            _logger = logger;
            _crawlers = crawlers.ToArray();
            _contentsRepository = contentsRepository;
            _varsRepository = varsRepository;
            _contentConfig = contentOptions.Value;
            _mapper = mapper;
        }

        public async Task Execute()
        {
            var lastAdditionalCrawlingExecTime = await _varsRepository.GetLastCrawlingTime() ??
                DateTime.Now.AddDays(-1 * _contentConfig.ActualTime);

            var crawlingWatch = new Stopwatch();
            crawlingWatch.Start();

            _logger.LogInformation($"Starting crawling ({_crawlers.Length} providers)");

            try
            {
                var currentContents = await _contentsRepository.GetAllUrls();
                var content = await GetContent(lastAdditionalCrawlingExecTime);
                content = content
                    .Where(c => !currentContents.Contains(c.Url))
                    .ToArray();
                foreach (var c in content)
                    c.Published = new DateTime(c.Published.Ticks, DateTimeKind.Utc);

                var entities = _mapper.Map<ContentEntity[]>(content);
                await _contentsRepository.AddRange(entities);

                await _varsRepository.SetLastCrawlingTime(DateTime.Now);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, $"Crawling error: {ex.Message}");
                throw;
            }
            finally
            {
                crawlingWatch.Stop();
                _logger.LogInformation($"Finish crawling in {crawlingWatch.Elapsed}");
            }
        }

        private async Task<AdItem[]> GetContent(DateTime since)
        {
            var timer = new Stopwatch();

            timer.Start();
            var tasks = _crawlers.Select(l => l.Get(since));
            var tasksResults = await Task.WhenAll(tasks);
            timer.Stop();

            var results = tasksResults
                .SelectMany(r => r)
                .ToArray();

            var downloadTime = timer.Elapsed;
            _logger.LogInformation($"Download {results.Length} items in {downloadTime}");

            return results;
        }
    }
}

