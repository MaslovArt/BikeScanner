using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;

namespace BikeScanner.UI.Bot.BotService.Context
{
    public class MemoryBotContext : IBotContext
    {
        private readonly IMemoryCache _cache;
        private readonly MemoryCacheEntryOptions _cacheOptions;

        public MemoryBotContext(IMemoryCache cache)
        {
            _cache = cache;
            _cacheOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
            };
        }

        public Task<BotContext> EnsureContext(long userId)
        {
            BotContext context = null;
            if (!_cache.TryGetValue(userId, out context))
            {
                context = new BotContext()
                {
                    UserId = userId
                };
                _cache.Set(userId, context, _cacheOptions);
            }

            return Task.FromResult(context);
        }

        public Task<BotContext> GetUserContext(long userId)
        {
            var context = _cache.Get<BotContext>(userId);

            return Task.FromResult(context);
        }

        public Task Update(BotContext context)
        {
            _cache.Remove(context.UserId);
            _cache.Set(context.UserId, context, _cacheOptions);

            return Task.FromResult(0);
        }
    }
}
