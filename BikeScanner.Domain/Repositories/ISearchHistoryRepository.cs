using BikeScanner.Domain.Models;
using System.Threading.Tasks;

namespace BikeScanner.Domain.Repositories
{
    public interface ISearchHistoryRepository : IRepository<SearchHistoryEntity>
    {
        /// <summary>
        /// Get last user search
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<SearchHistoryEntity> GetLast(long userId);
    }
}
