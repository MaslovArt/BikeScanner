using BikeScanner.Infrastructure.VK.Api.Abstraction;
using BikeScanner.Infrastructure.VK.Config;

namespace BikeScanner.Infrastructure.VK.Api
{
    /// <summary>
    /// Get group info by short name
    /// </summary>
    internal class GetGroupInfo : BaseVkApiMethod
    {

        public override string Method => "groups.getById";

        /// <summary>
        /// Group short name
        /// </summary>
        [VkParameter("group_id")]
        public string GroupShortName { get; set; }

        public GetGroupInfo(VkApiAccessConfig settings, string groupShortName)
            : base(settings) 
        {
            GroupShortName = groupShortName;
        }
    }
}
