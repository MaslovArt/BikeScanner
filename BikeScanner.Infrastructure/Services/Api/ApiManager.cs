using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace BikeScanner.Infrastructure.Services.Api
{
	internal class ApiManager
	{
        private readonly ILogger _logger;
        private readonly HttpClient _client;
        private readonly string _sourceType;
        private readonly SemaphoreSlim _concurrencySemaphore;

        public ApiManager(ILogger logger, int requestPerSecondLimit)
        {
            _logger = logger;
            _client = new HttpClient();
            _concurrencySemaphore = new SemaphoreSlim(requestPerSecondLimit);
        }

        public async Task<ApiResult<TResult, TError>> Request<TResult, TError>(IApiMethod api)
        {
            await _concurrencySemaphore.WaitAsync();

            var requestId = Guid.NewGuid().ToString().Substring(0, 6);
            var requestUrl = api.GetRequestString();
            _logger.LogDebug($"{_sourceType} request ({requestId}): {requestUrl}");

            var responseJson = await _client
                .GetStringAsync(requestUrl)
                .ContinueWith(t =>
                {
                    //ToDo think about max api request per second. Maybe custom task sheduler.
                    if (_concurrencySemaphore.CurrentCount == 0) Thread.Sleep(1000);

                    _concurrencySemaphore.Release();
                    return t.Result;
                });

            var result = new ApiResult<TResult, TError>();
            try
            {
                result.Result = JsonSerializer.Deserialize<TResult>(responseJson);
                _logger.LogInformation($"{_sourceType} request ({requestId}) completed");
            }
            catch
            {
                result.Error = JsonSerializer.Deserialize<TError>(responseJson);
                _logger.LogWarning($"{_sourceType} request ({requestId}) failed. {result.Error}");
            }

            return result;
        }
    }
}

