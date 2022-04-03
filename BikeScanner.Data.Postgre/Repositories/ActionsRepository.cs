using System.Threading.Tasks;
using BikeScanner.Domain.Models;
using BikeScanner.Domain.Repositories;

namespace BikeScanner.Data.Postgre.Repositories
{
	public class ActionsRepository : IActionsRepository
	{
        private readonly BikeScannerContext _context;

		public ActionsRepository(BikeScannerContext context)
		{
            _context = context;
		}

        public Task LogSearch(long userId, string searchQuery)
        {
            var newSearch = new ActionEntity(userId, searchQuery, ActionType.Search);
            _context.Add(newSearch);
            return _context.SaveChangesAsync();
        }

        public Task LogSubscription(long userId, string searchQuery)
        {
            var newSearch = new ActionEntity(userId, searchQuery, ActionType.Subscription);
            _context.Add(newSearch);
            return _context.SaveChangesAsync();
        }
    }
}

