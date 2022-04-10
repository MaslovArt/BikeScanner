using System;
using System.Threading.Tasks;

namespace BikeScanner.Domain.Repositories
{
    public interface IVarsRepository
    {
        Task<DateTime?> GetLastCrawlingTime();
        Task SetLastCrawlingTime(DateTime time);

        Task<DateTime?> GetLastScheduleEpoch();
        Task SetLastScheduleEpoch(DateTime time);
    }
}
