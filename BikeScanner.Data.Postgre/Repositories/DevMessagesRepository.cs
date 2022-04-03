using BikeScanner.Domain.Models;
using BikeScanner.Domain.Repositories;

namespace BikeScanner.Data.Postgre.Repositories
{
	public class DevMessagesRepository : BaseRepository<DevMessageEntity>, IDevMessagesRepository
	{
		public DevMessagesRepository(BikeScannerContext context)
			: base(context)
		{ }
	}
}

