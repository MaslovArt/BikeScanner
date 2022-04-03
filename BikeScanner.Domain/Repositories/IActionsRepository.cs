using System.Threading.Tasks;

namespace BikeScanner.Domain.Repositories
{
	public interface IActionsRepository
	{
		public Task LogSearch(long userId, string searchQuery);
		public Task LogSubscription(long userId, string searchQuery);
	}
}

