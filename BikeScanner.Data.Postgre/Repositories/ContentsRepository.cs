using BikeScanner.Domain.Models;
using BikeScanner.Domain.Repositories;

namespace BikeScanner.Data.Postgre.Repositories
{
    public class ContentsRepository : BaseRepository<ContentEntity>, IContentsRepository
    {
        public ContentsRepository(BikeScannerContext context)
            : base(context)
        { }
    }
}
