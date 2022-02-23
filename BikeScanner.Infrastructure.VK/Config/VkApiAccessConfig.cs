namespace BikeScanner.Infrastructure.VK.Config
{
    public class VkApiAccessConfig
    {
        public string ApiKey { get; set; }

        public string Version { get; set; }

        public int MaxApiRequestsPerSecond { get; set; }
    }
}
