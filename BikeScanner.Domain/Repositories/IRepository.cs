using BikeScanner.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BikeScanner.Domain.Repositories
{
    public interface IRepository<T> where T : BaseEntity
    {
        /// <summary>
        /// Get entities
        /// </summary>
        /// <param name="count">Entities count (all if null)</param>
        /// <param name="offset">Offset</param>
        /// <returns></returns>
        Task<T[]> Get(int? count = null, int? offset = null);

        Task<T> GetById(int id);

        Task Remove(T entity);

        Task RemoveRange(IEnumerable<T> entities);

        Task Add(T entity);

        Task AddRange(IEnumerable<T> entities);

        Task Update(T entity);

        Task UpdateRange(IEnumerable<T> entities);
    }
}
