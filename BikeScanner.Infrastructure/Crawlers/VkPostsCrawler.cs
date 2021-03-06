using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BikeScanner.Application.Interfaces;
using BikeScanner.Application.Models;
using BikeScanner.Domain.Extensions;
using BikeScanner.Infrastructure.Configs.Vk;
using BikeScanner.Infrastructure.Services.Vk;
using BikeScanner.Infrastructure.Services.Vk.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BikeScanner.Infrastructure.Crawlers
{
    /// <summary>
    /// Download users/groups posts from vk
    /// </summary>
	public class VkPostsCrawler : ICrawler
	{
        private readonly VkApiAccessConfig          _apiConfig;
        private readonly VkSourseConfig             _sourceConfig;
        private readonly VkApi                      _vkApi;
        private readonly ILogger<VkPostsCrawler>    _logger;

		public VkPostsCrawler(
            ILogger<VkPostsCrawler> logger,
            IOptions<VkApiAccessConfig> apiOptions,
            IOptions<VkSourseConfig> sourceOptions
            )
		{
            _apiConfig = apiOptions.Value;
            _sourceConfig = sourceOptions.Value;
            _logger = logger;
            _vkApi = new VkApi(_logger, _apiConfig);
		}

        public async Task<AdItem[]> Get(DateTime since)
        {
            var downloadTasks = _sourceConfig
                .Walls
                .Select(source => GetPosts(source, since));
            var results = await Task.WhenAll(downloadTasks);

            return results
                .SelectMany(x => x)
                .ToArray();
        }

        private async Task<AdItem[]> GetPosts(WallSourceConfig source, DateTime since)
        {
            var sinceStamp = since.UnixStamp();
            var result = new List<PostModel>();
            var offset = 0;
            var count = 100;
            var requestsCounter = 0;

            while (true)
            {
                var posts = await _vkApi.GetWallPosts(source.OwnerId, offset, count);
                //foreach (var post in posts.Where(p => p.IsPinned))
                //    post.DateStamp = DateTime.Now.AddDays(-1).UnixStamp();

                var validPosts = posts.Where(p => p.DateStamp > sinceStamp);
                result.AddRange(validPosts);

                requestsCounter++;

                if (validPosts.Count() < count ||
                    result.Count > _sourceConfig.MaxPostsPerGroup ||
                    result.Count == 0)
                {
                    _logger.LogInformation($"Download {result.Count} posts from [{source.OwnerName}] ({requestsCounter} requests)");
                    return result
                        .Where(r => !string.IsNullOrEmpty(r.Text))
                        .Select(r => new AdItem()
                        {
                            Text = r.Text,
                            Published = DateTimeOffset.FromUnixTimeSeconds(r.DateStamp).DateTime,
                            SourceType = CrawlerType.VkWall.ToString(),
                            Url = r.Url
                        })
                        .ToArray();
                }

                offset += count;
            }
        }
    }
}

