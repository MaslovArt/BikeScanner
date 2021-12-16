using BikeScanner.Domain.Models;
using System.Threading.Tasks;

namespace BikeScanner.Domain.Repositories
{
    public interface IContentsRepository : IRepository<ContentEntity>
    {
        public Task<ContentEntity[]> Search(string query);
        public Task<ContentEntity[]> Search(string query, long indexEpoch);
        public Task<ContentEntity[]> GetContents(long indexEpoch);
    }
}
