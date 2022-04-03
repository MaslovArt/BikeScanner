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

        public Task<ContentEntity[]> Scroll(int afterId, int take)
        {
            return Set
                .AsNoTracking()
                .Where(c => c.Id > afterId)
                .Take(take)
                .OrderBy(c => c.Id)
                .ToArrayAsync();
        }

        public async Task<Page<ContentEntity>> Search(string query, int skip, int take)
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

            return new Page<ContentEntity>()
            {
                Items = entities,
                Total = total,
                Offset = skip + entities.Length
            };
        }

        public Task<int> CountSearch(string query)
        {
            return Set
                .Where(c => c.Text.ToUpper().Contains(query.ToUpper()))
                .CountAsync();
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
