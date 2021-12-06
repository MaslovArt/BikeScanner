using BikeScanner.Infrastructure.VK.Api.Abstraction;
using BikeScanner.Infrastructure.VK.Config;
using System;

namespace BikeScanner.Infrastructure.VK.Api.Methods
{
    internal class GetPhotos : BaseVkApiMethod
    {
        private int _offset;
        private int _count;

        public override string Method => "photos.get";

        [VkParameter("owner_id")]
        public int OwnerId { get; set; }

        [VkParameter("album_id")]
        public int AlbumId { get; set; }

        [VkParameter("rev")]
        public bool DateDesc { get; set; }

        [VkParameter("offset")]
        public int Offset
        {
            get => _offset;
            set
            {
                if (value < 0)
                    throw new ArgumentException($"{nameof(Offset)} must be more than 0");

                _offset = value;
            }
        }

        [VkParameter("count")]
        public int Count
        {
            get => _count;
            set
            {
                if (value < 1 || value > 100)
                    throw new ArgumentException($"{nameof(Count)} must be in range (1, 100)");

                _count = value;
            }
        }

        public GetPhotos(VkSettings settings, int ownerId, int albumId) 
            : base(settings)
        {
            OwnerId = ownerId;
            AlbumId = albumId;
            Offset = 0;
            Count = 100;
        }
    }
}
