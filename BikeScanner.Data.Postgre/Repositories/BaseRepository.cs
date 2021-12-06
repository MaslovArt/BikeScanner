using BikeScanner.Domain.Base;

namespace BikeScanner.Data.Postgre.Repositories
{
    public abstract class BaseRepository<T> : IRepository<T>
        where T : BaseEntity
    {
        protected readonly BikeScannerContext Context;

        public BaseRepository(BikeScannerContext context)
        {
            Context = context;
        }
    }
}
