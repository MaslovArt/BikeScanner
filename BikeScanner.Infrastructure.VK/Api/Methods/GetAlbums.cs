using BikeScanner.Infrastructure.VK.Api.Abstraction;
using BikeScanner.Infrastructure.VK.Config;

namespace BikeScanner.Infrastructure.VK.Api.Methods
{
    internal class GetAlbums : BaseVkApiMethod
    {
        public override string Method => "photos.getAlbums";

        [VkParameter("owner_id")]
        public int OwnerId { get; set; }

        [VkParameter("album_ids")]
        public ParamsCollection<int> AlbumsIds { get; set; }

        public GetAlbums(VkApiAccessConfig settings, int ownerId)
            : base(settings)
        {
            OwnerId = ownerId; 
        }
    }
}
