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
    /// Download users/groups photos info from vk
    /// </summary>
    public class VkPhotosCrawler : ICrawler
    {
        private readonly VkApiAccessConfig          _apiConfig;
        private readonly VkSourseConfig             _sourceConfig;
        private readonly VkApi                      _vkApi;
        private readonly ILogger<VkPhotosCrawler>   _logger;

        public VkPhotosCrawler(
            ILogger<VkPhotosCrawler> logger,
            IOptions<VkApiAccessConfig> apiOptions,
            IOptions<VkSourseConfig> sourceOptions
            )
        {
            _apiConfig = apiOptions.Value;
            _sourceConfig = sourceOptions.Value;
            _logger = logger;
            _vkApi = new VkApi(_logger, _apiConfig);
        }

        public async Task<AdItem[]> Get(DateTime loadSince)
        {
            var groupsByOwner = _sourceConfig
                .Albums
                .GroupBy(s => s.OwnerId);

            var albumTasks = groupsByOwner
                .Select(source => GetPhotos(source, loadSince));
            var results = await Task.WhenAll(albumTasks);

            return results
                .SelectMany(r => r)
                .ToArray();
        }

        private async Task<AdItem[]> GetPhotos(IGrouping<int, AlbumSourceConfig> sources, DateTime since)
        {
            var sinceStamp = since.UnixStamp();

            var albumsIds = sources
                .Select(a => a.Id)
                .ToArray();
            var albumsInfo = await _vkApi.GetAlbums(sources.Key, albumsIds);

            var updatedAlbums = albumsInfo
                .Where(a => a.Updated > sinceStamp);
            var updatedSources = sources
                .Where(s => updatedAlbums.Any(ua => ua.Id == s.Id));

            var photosTasks = updatedSources
                .Select(s => LoadAlbumPhotos(s, sinceStamp));
            var photos = await Task.WhenAll(photosTasks);

            return photos
                .SelectMany(p => p)
                .Where(p => !string.IsNullOrEmpty(p.Text))
                .Select(p => new AdItem()
                {
                    Text = p.Text,
                    Created = DateTimeOffset.FromUnixTimeSeconds(p.DateStamp).DateTime,
                    SourceType = CrawlerType.VkAlbum.ToString(),
                    Url = p.Url
                })
                .ToArray();
        }

        //ToDo Load first comment if no photo text provided
        private async Task<PhotoModel[]> LoadAlbumPhotos(AlbumSourceConfig source, long sinceStamp)
        {
            var result = new List<PhotoModel>();
            var offset = 0;
            var count = 50;
            var requestsCounter = 0;

            while (true)
            {
                var photos = await _vkApi.GetPhotos(source.OwnerId, source.Id, offset, count);
                var validPhotos = photos.Where(p => p.DateStamp > sinceStamp);
                result.AddRange(validPhotos);

                requestsCounter++;

                if (photos.Any(p => p.DateStamp < sinceStamp) ||
                                    result.Count > _sourceConfig.MaxPostsPerGroup ||
                                    result.Count == 0)
                {
                    _logger.LogInformation($"Download {result.Count} photos from [{source.OwnerName}:{source.Title}] ({requestsCounter} request)");
                    return result.ToArray();
                }

                offset += count;
            }
        }
    }
}

