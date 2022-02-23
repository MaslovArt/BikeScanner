using BikeScanner.Infrastructure.VK.Config;
using System;
using System.Linq;
using System.Text;

namespace BikeScanner.Infrastructure.VK.Api.Abstraction
{
    /// <summary>
    /// Base class for vk api methods
    /// </summary>
    internal abstract class BaseVkApiMethod : IVkApiMethod
    {
        private readonly string _apiVersion,
                                _accessToken;

        public BaseVkApiMethod(VkApiAccessConfig settings)
        {
            _accessToken = settings.ApiKey 
                ?? throw new ArgumentNullException(nameof(settings.ApiKey));
            _apiVersion = settings.Version
                ?? throw new ArgumentNullException(nameof(settings.Version));
        }

        /// <summary>
        /// Method name.
        /// </summary>
        public abstract string Method { get; }

        /// <summary>
        /// Return request url.
        /// </summary>
        /// <returns>Uri</returns>
        public string GetRequestString()
        {
            var parameters = new StringBuilder();

            var properties = GetType()
                .GetProperties()
                .Select(p => new
                {
                    prop = p,
                    vkAttrs = p.GetCustomAttributes(typeof(VkParameterAttribute), true)
                })
                .Where(pi => pi.vkAttrs.Length > 0)
                .Select(pi => new
                {
                    value = pi.prop.GetValue(this),
                    name = (pi.vkAttrs.First() as VkParameterAttribute).Name
                })
                .Where(p => p.value != default);

            foreach (var property in properties)
            {
                parameters.Append($"&{property.name}={property.value}");
            }

            return string.Format("https://api.vk.com/method/{0}?access_token={1}{2}&v={3}",
                Method,
                _accessToken,
                parameters.ToString(),
                _apiVersion);
        }
    }
}
