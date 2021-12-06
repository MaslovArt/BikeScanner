using Microsoft.Extensions.Logging;
using BikeScanner.Infrastructure.VK.Api.Abstraction;
using BikeScanner.Infrastructure.VK.Models;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace BikeScanner.Infrastructure.VK.Api
{
    /// <summary>
    /// Make request to vk api
    /// </summary>
    internal class VkApiExecutor
    {
        private readonly ILogger _logger;
        private readonly HttpClient _client;
        private readonly SemaphoreSlim _concurrencySemaphore;

        public VkApiExecutor(ILogger logger, int requestPerSecondLimit)
        {
            _logger = logger;
            _client = new HttpClient();
            _concurrencySemaphore = new SemaphoreSlim(requestPerSecondLimit);
        }

        /// <summary>
        /// Execute method
        /// </summary>
        /// <typeparam name="T">Responce Type</typeparam>
        /// <param name="vkApiMethod">Executed method</param>
        /// <returns></returns>
        public async Task<VkResponse<T>> Execute<T>(IVkApiMethod vkApiMethod)
        {
            await _concurrencySemaphore.WaitAsync();

            var requestId = Guid.NewGuid().ToString().Substring(0, 6);
            var requestUrl = vkApiMethod.GetRequestString();
            _logger.LogDebug($"Vk api request ({requestId}): {requestUrl}");

            var responseJson = await _client
                .GetStringAsync(requestUrl)
                .ContinueWith(t => 
                {
                    //ToDo think about max api request per second. Maybe custom task sheduler.
                    if (_concurrencySemaphore.CurrentCount == 0) Thread.Sleep(1000);

                    _concurrencySemaphore.Release();
                    return t.Result;
                });

            var result = new VkResponse<T>();
            if (responseJson.StartsWith("{\"error\":{"))
            {
                var errorResponse = JsonSerializer.Deserialize<VkErrorResponse>(responseJson);
                result.Error = errorResponse.Error;
                _logger.LogWarning($"Vk api request ({requestId}) failed. {errorResponse.Error}");
            }
            else
            {
                _logger.LogInformation($"Vk api request ({requestId}) completed");
                result = JsonSerializer.Deserialize<VkResponse<T>>(responseJson);
            }

            return result;
        }
    }
}
