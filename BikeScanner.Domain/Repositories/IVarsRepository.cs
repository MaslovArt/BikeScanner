using System.Threading.Tasks;

namespace BikeScanner.Domain.Repositories
{
    public interface IVarsRepository
    {
        Task<long?> GetLastIndexingStamp();
        Task SetLastIndexingStamp(long stamp);

        Task<long?> GetLastSchedulingStamp();
        Task SetLastSchedulingStamp(long stamp);
    }
}
