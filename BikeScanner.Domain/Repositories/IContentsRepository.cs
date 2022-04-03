using BikeScanner.Domain.Models;
using System.Threading.Tasks;

namespace BikeScanner.Domain.Repositories
{
    public interface IContentsRepository : IRepository<ContentEntity>
    {
        Task<ContentEntity[]> Scroll(int skip, int take);
        Task<Page<ContentEntity>> Search(string query, int skip, int take);
        Task<ContentEntity[]> SearchEpoch(string query, long indexEpoch);
        Task<ContentEntity[]> GetContents(long indexEpoch);
    }
}
