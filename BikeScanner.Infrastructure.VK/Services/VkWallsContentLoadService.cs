using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using BikeScanner.Domain.Extensions;
using BikeScanner.Infrastructure.VK.Api;
using BikeScanner.Infrastructure.VK.Config;
using BikeScanner.Infrastructure.VK.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BikeScanner.Application.Interfaces;
using BikeScanner.Domain.Models;

namespace BikeScanner.Infrastructure.VK.Services
{
    /// <summary>
    /// Loader from vk users&groups walls
    /// </summary>
    public class VkWallsContentLoadService : IContentLoader
    {
        private readonly ILogger<VkWallsContentLoadService> _logger;
        private readonly WallSourceModel[]                  _defaultWallSources;
        private readonly int                                _maxPostsPerOwner;
        private readonly VKApi                              _vkApi;

        public VkWallsContentLoadService(
            ILogger<VkWallsContentLoadService> logger,
            VKApi vkApi,
            IOptions<VkSourseConfig> sourseCollection)
        {
            _logger = logger;
            _vkApi = vkApi;
            _defaultWallSources = sourseCollection.Value.Walls ?? Array.Empty<WallSourceModel>();
            _maxPostsPerOwner = sourseCollection.Value.MaxPostsPerGroup;
        }

        public async Task<ContentEntity[]> Load(DateTime loadSince)
        {
            var downloadTasks = _defaultWallSources.Select(source => LoadWallItems(source, loadSince));
            var results = await Task.WhenAll(downloadTasks);

            return results.SelectMany(r => r).ToArray();
        }

        private async Task<ContentEntity[]> LoadWallItems(WallSourceModel source, DateTime since)
        {
            var sinceStamp = since.UnixStamp();
            var result = new List<WallModel>();
            var offset = 0;
            var count = 100;
            var requestsCounter = 0;

            while (true)
            {
                var posts = await _vkApi.GetWallPosts(source.OwnerId, offset, count);
                foreach (var post in posts.Where(p => p.IsPinned))
                    post.DateStamp = DateTime.Now.AddDays(-1).UnixStamp();

                var validPosts = posts.Where(p => p.DateStamp > sinceStamp);
                result.AddRange(validPosts);

                requestsCounter++;

                if (validPosts.Count() < count || result.Count > _maxPostsPerOwner || result.Count == 0)
                {
                    _logger.LogInformation($"Download {result.Count} posts from [{source.OwnerName}] ({requestsCounter} request)");
                    return result
                        .Where(r => !string.IsNullOrEmpty(r.Text))
                        .Select(r => new ContentEntity() 
                        { 
                            Text = r.Text,
                            Published = DateTimeOffset.FromUnixTimeSeconds(r.DateStamp).DateTime,
                            SourceType = Consts.VkWallSourceType,
                            AdUrl = r.Url
                        })
                        .ToArray();
                }

                offset += count;
            }
        }
    }
}
