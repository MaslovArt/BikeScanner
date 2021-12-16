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

        public Task<T> GetById(int id)
        {
            return Set
                .FirstOrDefaultAsync(e => e.Id == id);
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

        public Task Update(T entity)
        {
            if (!Set.Local.Any(e => e == entity))
            {
                Set.Update(entity);
            }

            return Context.SaveChangesAsync();
        }

        public async Task UpdateRange(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                await Update(entity);
            }
        }
    }
}
