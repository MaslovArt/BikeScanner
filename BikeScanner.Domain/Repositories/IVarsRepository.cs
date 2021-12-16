using System.Threading.Tasks;

namespace BikeScanner.Domain.Repositories
{
    public interface IVarsRepository
    {
        Task<long?> GetLastIndexEpoch();
        Task SetLastIndexEpoch(long stamp);

        Task<long?> GetLastScheduleEpoch();
        Task SetLastScheduleEpoch(long stamp);
    }
}
