using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using BikeScanner.Infrastructure.VK.Api.Abstraction;
using BikeScanner.Infrastructure.VK.Api.Methods;
using BikeScanner.Infrastructure.VK.Config;
using BikeScanner.Infrastructure.VK.Models;
using System;
using System.Threading.Tasks;

namespace BikeScanner.Infrastructure.VK.Api
{
    /// <summary>
    /// Vk api
    /// </summary>
    public class VKApi
    {
        private readonly VkApiAccessConfig _settings;
        private readonly VkApiExecutor _executor;

        public VKApi(IOptions<VkApiAccessConfig> settings, ILogger<VKApi> logger)
        {
            if (settings.Value.MaxApiRequestsPerSecond < 0)
                throw new ArgumentException($"{nameof(settings.Value.MaxApiRequestsPerSecond)} min value is 1!");

            _settings = settings.Value;
            _executor = new VkApiExecutor(logger, settings.Value.MaxApiRequestsPerSecond);
        }

        /// <summary>
        /// Get albums
        /// </summary>
        /// <param name="id"></param>
        /// <param name="albumsIds"></param>
        /// <returns>Albums or empty array</returns>
        public async Task<AlbumModel[]> GetAlbums(int id, int[] albumsIds = null)
        {
            var getGroupMethod = new GetAlbums(_settings, id)
            {
                AlbumsIds = new ParamsCollection<int>(albumsIds)
            };
            var response = await _executor.Execute<VkArray<AlbumModel>>(getGroupMethod);

            return response.Succeed
                ? response.Response.Items
                : Array.Empty<AlbumModel>();
        }

        /// <summary>
        /// Get group info
        /// </summary>
        /// <param name="group"></param>
        /// <returns>Group info or null</returns>
        public async Task<GroupModel> GetGroupInfo(string group)
        {
            var getGroupMethod = new GetGroupInfo(_settings, group);
            var response = await _executor.Execute<GroupModel[]>(getGroupMethod);

            return response.Succeed && response.Response[0] != null
                ? response.Response[0]
                : null;
        }

        /// <summary>
        /// Get user info
        /// </summary>
        /// <param name="user"></param>
        /// <returns>User info or null</returns>
        public async Task<UserModel> GetUserInfo(string user)
        {
            var getGroupMethod = new GetUserInfo(_settings, user);
            var response = await _executor.Execute<UserModel[]>(getGroupMethod);

            return response.Succeed && response.Response[0] != null
                ? response.Response[0]
                : null;
        }

        /// <summary>
        /// Get wall posts
        /// </summary>
        /// <param name="ownerId"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <returns>Posts or empty array</returns>
        public async Task<WallModel[]> GetWallPosts(int ownerId, int offset, int count)
        {
            var getWallPostsMethod = new GetWallPosts(_settings, ownerId)
            {
                Offset = offset,
                Count = count
            };
            var response = await _executor.Execute<VkArray<WallModel>>(getWallPostsMethod);

            return response.Succeed
                ? response.Response.Items
                : Array.Empty<WallModel>();
        }

        /// <summary>
        /// Get photos
        /// </summary>
        /// <param name="ownerId"></param>
        /// <param name="albumId"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <returns>Photos or empty array</returns>
        public async Task<PhotoModel[]> GetPhotos(int ownerId, int albumId, int offset, int count)
        {
            var getPhotosMethod = new GetPhotos(_settings, ownerId, albumId)
            {
                Offset = offset,
                Count = count,
                DateDesc = true
            };
            var response = await _executor.Execute<VkArray<PhotoModel>>(getPhotosMethod);

            return response.Succeed
                ? response.Response.Items
                : Array.Empty<PhotoModel>();
        }

        /// <summary>
        /// Get photo comments
        /// </summary>
        /// <param name="albumOwnerId"></param>
        /// <param name="photoId"></param>
        /// <returns>Comments or empty array</returns>
        public async Task<CommentModel[]> GetPhotoComments(int albumOwnerId, int photoId)
        {
            var getPhotosMethod = new GetPhotoComments(_settings, albumOwnerId, photoId);
            var response = await _executor.Execute<VkArray<CommentModel>>(getPhotosMethod);

            return response.Succeed
                ? response.Response.Items
                : Array.Empty<CommentModel>();
        }
    }
}
