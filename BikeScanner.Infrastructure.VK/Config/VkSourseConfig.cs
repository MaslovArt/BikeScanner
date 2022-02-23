using BikeScanner.Infrastructure.VK.Models;

namespace BikeScanner.Infrastructure.VK.Config
{
    /// <summary>
    /// Vk source configuration
    /// </summary>
    public class VkSourseConfig
    {
        /// <summary>
        /// Users or groups walls list
        /// </summary>
        public WallSourceModel[] Walls { get; set; }

        /// <summary>
        /// Users or groups albums list
        /// </summary>
        public AlbumSourceModel[] Albums { get; set; }

        /// <summary>
        /// Maximum number of download items per user or group 
        /// </summary>
        public int MaxPostsPerGroup { get; set; }
    }
}
