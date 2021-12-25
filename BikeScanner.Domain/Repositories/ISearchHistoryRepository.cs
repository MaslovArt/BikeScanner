using BikeScanner.Domain.Models;
using System.Threading.Tasks;

namespace BikeScanner.Domain.Repositories
{
    public interface ISearchHistoryRepository : IRepository<SearchHistoryEntity>
    {
        Task WriteHistory(long userId, string searchQuery);
    }
}
