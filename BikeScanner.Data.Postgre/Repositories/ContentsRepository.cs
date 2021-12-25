using BikeScanner.Domain.Models;
using BikeScanner.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace BikeScanner.Data.Postgre.Repositories
{
    public class ContentsRepository : BaseRepository<ContentEntity>, IContentsRepository
    {
        public ContentsRepository(BikeScannerContext context)
            : base(context)
        { }

        public async Task<PagedEntities<ContentEntity>> Search(string query, int skip, int take)
        {
            var queryable = Set
                .AsNoTracking()
                .Where(c => c.Text.ToUpper().Contains(query.ToUpper()))
                .OrderByDescending(c => c.Published);

            var entities = await queryable
                .Skip(skip)
                .Take(take)
                .ToArrayAsync();

            var total = entities.Length == take
                ? await queryable.CountAsync()
                : skip + entities.Length;

            return new PagedEntities<ContentEntity>()
            {
                Entities = entities,
                Total = total
            };
        }

        public Task<ContentEntity[]> SearchEpoch(string query, long indexingStamp)
        {
            return Set
                .AsNoTracking()
                .Where(c => c.IndexEpoch == indexingStamp)
                .Where(c => c.Text.ToUpper().Contains(query.ToUpper()))
                .ToArrayAsync();
        }

        public Task<ContentEntity[]> GetContents(long indexingStamp)
        {
            return Set
                .AsNoTracking()
                .Where(c => c.IndexEpoch == indexingStamp)
                .ToArrayAsync();
        }
    }
}
