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
    public class VKApi
    {
        private readonly VkSettings _settings;
        private readonly VkApiExecutor _executor;

        public VKApi(IOptions<VkSettings> settings, ILogger<VKApi> logger)
        {
            _settings = settings.Value;
            _executor = new VkApiExecutor(logger, settings.Value.MaxApiRequestsPerSecond);
        }

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

        public async Task<GroupModel> GetGroupInfo(string group)
        {
            var getGroupMethod = new GetGroupInfo(_settings, group);
            var response = await _executor.Execute<GroupModel[]>(getGroupMethod);

            return response.Succeed && response.Response[0] != null
                ? response.Response[0]
                : null;
        }

        public async Task<UserModel> GetUserInfo(string user)
        {
            var getGroupMethod = new GetUserInfo(_settings, user);
            var response = await _executor.Execute<UserModel[]>(getGroupMethod);

            return response.Succeed && response.Response[0] != null
                ? response.Response[0]
                : null;
        }

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
