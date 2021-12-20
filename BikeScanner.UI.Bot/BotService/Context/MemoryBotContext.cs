using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikeScanner.UI.Bot.BotService.Context
{
    public class MemoryBotContext : IBotContext
    {
        private List<BotContextModel> _collection;

        public MemoryBotContext()
        {
            _collection = new List<BotContextModel>();
        }

        public Task<BotContextModel> EnsureContext(long userId)
        {
            var userContext = _collection.FirstOrDefault(el => el.UserId == userId);
            if (userContext == null)
            {
                userContext = new BotContextModel()
                {
                    UserId = userId
                };
                _collection.Add(userContext);
            }

            return Task.FromResult(userContext);
        }

        public Task<BotContextModel> GetUserContext(long userId)
        {
            var userContext = _collection.FirstOrDefault(el => el.UserId == userId);

            return Task.FromResult(userContext);
        }

        public Task Update(BotContextModel context)
        {
            return Task.FromResult(0);
        }
    }
}
