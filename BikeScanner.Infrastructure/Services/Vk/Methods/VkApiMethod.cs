using System;
using BikeScanner.Infrastructure.Services.Api;
using BikeScanner.Infrastructure.Configs.Vk;

namespace BikeScanner.Infrastructure.Services.Vk.Methods
{
    /// <summary>
    /// Base class for vk api methods
    /// </summary>
    internal abstract class VkApiMethod : IApiMethod
    {
        private readonly string _apiVersion,
                                _accessToken;

        public VkApiMethod(VkApiAccessConfig settings)
        {
            _accessToken = settings.ApiKey
                ?? throw new ArgumentNullException(nameof(settings.ApiKey));
            _apiVersion = settings.Version
                ?? throw new ArgumentNullException(nameof(settings.Version));
        }

        /// <summary>
        /// Method name
        /// </summary>
        public abstract string Method { get; }

        /// <summary>
        /// Method url format params
        /// </summary>
        public abstract string Params { get; }

        public string GetRequestString() =>
            $"https://api.vk.com/method/{Method}?access_token={_accessToken}&{Params}&v={_apiVersion}";
    }
}

