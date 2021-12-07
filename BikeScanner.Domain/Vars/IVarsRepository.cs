using System;
using System.Threading.Tasks;

namespace BikeScanner.Domain.Vars
{
    public interface IVarsRepository
    {
        Task<DateTime?> GetLastIndexingTime();
        Task SetLastIndexingTime(DateTime time);
    }
}
