using BikeScanner.Domain.Models;
using BikeScanner.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikeScanner.Data.Postgre.Repositories
{
    public abstract class BaseRepository<T> : IRepository<T>
        where T : BaseEntity
    {
        protected readonly BikeScannerContext Context;
        protected readonly DbSet<T> Set;

        public BaseRepository(BikeScannerContext context)
        {
            Context = context;
            Set = context.Set<T>();
        }

        public Task Add(T entity)
        {
            Set.Add(entity);
            return Context.SaveChangesAsync();
        }

        public Task AddRange(IEnumerable<T> entities)
        {
            Set.AddRange(entities);
            return Context.SaveChangesAsync();
        }

        public Task<T[]> Get(int? count, int? offset)
        {
            var query = Set.AsNoTracking();

            if (offset.HasValue) 
                query = query.Skip(offset.Value);
            if (count.HasValue) 
                query = query.Take(count.Value);

            return query.ToArrayAsync();
        }

        public Task Remove(T entity)
        {
            Set.Remove(entity);
            return Context.SaveChangesAsync();
        }

        public Task RemoveRange(IEnumerable<T> entities)
        {
            Set.RemoveRange(entities);
            return Context.SaveChangesAsync();
        }
    }
}
