using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using BikeScanner.Domain.Extensions;
using BikeScanner.Application.Interfaces;
using BikeScanner.Infrastructure.VK.Api;
using BikeScanner.Infrastructure.VK.Config;
using BikeScanner.Infrastructure.VK.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BikeScanner.Domain.Models;

namespace BikeScanner.Infrastructure.VK.Services
{
    /// <summary>
    /// Loader from users&groups albums
    /// </summary>
    public class VkAlbumsContentLoadService : IContentLoader
    {
        private readonly ILogger<VkWallsContentLoadService> _logger;
        private readonly AlbumSourceModel[]                 _defaultAlbumSources;
        private readonly int                                _maxPostsPerOwner;
        private readonly VKApi                              _vkApi;

        public VkAlbumsContentLoadService(
            ILogger<VkWallsContentLoadService> logger,
            VKApi vkApi,
            IOptions<VkSourseConfig> sourseCollection)
        {
            _logger = logger;
            _vkApi = vkApi;
            _defaultAlbumSources = sourseCollection.Value.Albums ?? Array.Empty<AlbumSourceModel>();
            _maxPostsPerOwner = sourseCollection.Value.MaxPostsPerGroup;
        }

        public async Task<ContentEntity[]> Load(DateTime loadSince)
        {
            var groupsByOwner = _defaultAlbumSources.GroupBy(s => s.OwnerId);

            var albumTasks = groupsByOwner.Select(source => LoadOwnerPhotos(source, loadSince));
            var results = await Task.WhenAll(albumTasks);

            return results
                .SelectMany(r => r)
                .ToArray();
        }

        private async Task<ContentEntity[]> LoadOwnerPhotos(IGrouping<int , AlbumSourceModel> sources, DateTime since)
        {
            var sinceStamp = since.UnixStamp();

            var albumsIds = sources.Select(a => a.Id).ToArray();
            var albumsInfo = await _vkApi.GetAlbums(sources.Key, albumsIds);

            var updatedAlbums = albumsInfo.Where(a => a.Updated > sinceStamp);
            var updatedSources = sources.Where(s => updatedAlbums.Any(ua => ua.Id == s.Id));

            var photosTasks = updatedSources.Select(s => LoadAlbumPhotos(s, sinceStamp));
            var photos = await Task.WhenAll(photosTasks);

            return photos
                .SelectMany(p => p)
                .Where(p => !string.IsNullOrEmpty(p.Text))
                .Select(p => new ContentEntity() 
                { 
                    Text = p.Text,
                    Created = DateTimeOffset.FromUnixTimeSeconds(p.DateStamp).DateTime,
                    SourceType = Consts.VkAlbumSourceType,
                    Url = p.Url
                })
                .ToArray();
        }

        //ToDo Load first comment if no photo text provided
        private async Task<PhotoModel[]> LoadAlbumPhotos(AlbumSourceModel source, long sinceStamp)
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
                                    result.Count > _maxPostsPerOwner || 
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
