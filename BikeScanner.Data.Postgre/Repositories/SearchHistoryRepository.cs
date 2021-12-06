using BikeScanner.Domain.SearchHistory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikeScanner.Data.Postgre.Repositories
{
    public class SearchHistoryRepository : BaseRepository<SearchHistoryEntity>, ISearchHistoryRepository
    {
        public SearchHistoryRepository(BikeScannerContext context)
            : base(context)
        { }
    }
}
