using BikeScanner.Domain.Models;
using BikeScanner.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BikeScanner.Data.Postgre.Repositories
{
    public class ContentsRepository : BaseRepository<ContentEntity>, IContentsRepository
    {
        public ContentsRepository(BikeScannerContext context)
            : base(context)
        { }

        public Task<ContentEntity[]> Search(string query)
        {
            return Set
                .AsNoTracking()
                .Where(c => c.Text.ToUpper().Contains(query.ToUpper()))
                .ToArrayAsync();
        }

        public Task<ContentEntity[]> Search(string query, long indexingStamp)
        {
            return Set
                .AsNoTracking()
                .Where(c => c.IndexingStamp == indexingStamp)
                .Where(c => c.Text.ToUpper().Contains(query.ToUpper()))
                .ToArrayAsync();
        }
    }
}
