using BikeScanner.Infrastructure.VK.Api.Abstraction;
using BikeScanner.Infrastructure.VK.Config;

namespace BikeScanner.Infrastructure.VK.Api.Methods
{
    internal class GetPhotoComments : BaseVkApiMethod
    {
        public override string Method => "photos.getComments";

        [VkParameter("owner_id")]
        public int AlbumOwnerId { get; set; }

        [VkParameter("photo_id")]
        public int PhotoId { get; set; }

        public GetPhotoComments(VkApiAccessConfig settings, int albumOwnerId, int photoId) 
            : base(settings)
        {
            AlbumOwnerId = albumOwnerId;
            PhotoId = photoId;
        }
    }
}
