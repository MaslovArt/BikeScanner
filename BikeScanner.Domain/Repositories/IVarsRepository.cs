using System;
using System.Threading.Tasks;

namespace BikeScanner.Domain.Repositories
{
    public interface IVarsRepository
    {
        Task<DateTime?> GetLastIndexingTime();
        Task SetLastIndexingTime(DateTime time);
    }
}
