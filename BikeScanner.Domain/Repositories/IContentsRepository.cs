using BikeScanner.Domain.Models;
using System.Threading.Tasks;

namespace BikeScanner.Domain.Repositories
{
    public interface IContentsRepository : IRepository<ContentEntity>
    {
        public Task<PagedEntities<ContentEntity>> Search(string query, int skip, int take);
        public Task<ContentEntity[]> SearchEpoch(string query, long indexEpoch);
        public Task<ContentEntity[]> GetContents(long indexEpoch);
    }
}
