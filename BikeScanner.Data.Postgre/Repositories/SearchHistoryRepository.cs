using BikeScanner.Domain.Models;
using BikeScanner.Domain.Repositories;
using System;
using System.Threading.Tasks;

namespace BikeScanner.Data.Postgre.Repositories
{
    public class SearchHistoryRepository : BaseRepository<SearchHistoryEntity>, ISearchHistoryRepository
    {
        public SearchHistoryRepository(BikeScannerContext context)
            : base(context)
        { }

        public Task WriteHistory(long userId, string searchQuery)
        {
            return Add(new SearchHistoryEntity()
            {
                UserId = userId,
                SearchQuery = searchQuery,
                Date = DateTime.Now,
            });
        }
    }
}
