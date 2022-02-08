using BikeScanner.Domain.Models;
using BikeScanner.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BikeScanner.Data.Postgre.Repositories
{
    public class SearchHistoryRepository : BaseRepository<SearchHistoryEntity>, ISearchHistoryRepository
    {
        public SearchHistoryRepository(BikeScannerContext context)
            : base(context)
        { }

        public Task<SearchHistoryEntity> GetLast(long userId)
        {
            return Set
                .AsNoTracking()
                .OrderByDescending(e => e.Date)
                .FirstOrDefaultAsync();
        }
    }
}
