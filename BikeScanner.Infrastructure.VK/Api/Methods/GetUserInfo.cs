using BikeScanner.Infrastructure.VK.Api.Abstraction;
using BikeScanner.Infrastructure.VK.Config;

namespace BikeScanner.Infrastructure.VK.Api
{
    internal class GetUserInfo : BaseVkApiMethod
    {
        public override string Method => "users.get";

        /// <summary>
        /// Group short name
        /// </summary>
        [VkParameter("user_ids")]
        public string UserShortName { get; set; }

        public GetUserInfo(VkSettings settings, string userShortName)
            : base(settings)
        {
            UserShortName = UserShortName;
        }
    }
}
