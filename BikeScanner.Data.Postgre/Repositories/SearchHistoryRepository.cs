using BikeScanner.Domain.Models;
using BikeScanner.Domain.Repositories;

namespace BikeScanner.Data.Postgre.Repositories
{
    public class SearchHistoryRepository : BaseRepository<SearchHistoryEntity>, ISearchHistoryRepository
    {
        public SearchHistoryRepository(BikeScannerContext context)
            : base(context)
        { }
    }
}
