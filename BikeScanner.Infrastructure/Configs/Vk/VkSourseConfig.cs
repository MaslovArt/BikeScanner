using BikeScanner.Infrastructure.Services.Vk.Models;

namespace BikeScanner.Infrastructure.Configs.Vk
{
    /// <summary>
    /// Vk source configuration
    /// </summary>
    public class VkSourseConfig
    {
        /// <summary>
        /// Users or groups walls list
        /// </summary>
        public WallSourceConfig[] Walls { get; set; }

        /// <summary>
        /// Users or groups albums list
        /// </summary>
        public AlbumSourceConfig[] Albums { get; set; }

        /// <summary>
        /// Maximum number of download items per user or group 
        /// </summary>
        public int MaxPostsPerGroup { get; set; }
    }
}
