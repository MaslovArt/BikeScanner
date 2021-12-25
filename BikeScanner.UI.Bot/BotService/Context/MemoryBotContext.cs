using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikeScanner.UI.Bot.BotService.Context
{
    public class MemoryBotContext : IBotContext
    {
        private List<BotContext> _collection;

        public MemoryBotContext()
        {
            _collection = new List<BotContext>();
        }

        public Task<BotContext> EnsureContext(long userId)
        {
            var userContext = _collection.FirstOrDefault(el => el.UserId == userId);
            if (userContext == null)
            {
                userContext = new BotContext()
                {
                    UserId = userId
                };
                _collection.Add(userContext);
            }

            return Task.FromResult(userContext);
        }

        public Task<BotContext> GetUserContext(long userId)
        {
            var userContext = _collection.FirstOrDefault(el => el.UserId == userId);

            return Task.FromResult(userContext);
        }

        public Task Update(BotContext context)
        {
            return Task.FromResult(0);
        }
    }
}
