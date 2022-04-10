using System.Threading.Tasks;

namespace BikeScanner.Application.Jobs
{
    public interface IAdditionalCrawlingJob
    {
        Task Execute();
    }
}