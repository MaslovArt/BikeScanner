using System.Threading.Tasks;

namespace BikeScanner.Application.Jobs
{
    public interface IAutoSearchJob
    {
        Task Execute();
    }
}