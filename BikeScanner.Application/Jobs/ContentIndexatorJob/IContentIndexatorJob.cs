using System.Threading.Tasks;

namespace BikeScanner.Application.Jobs
{
    public interface IContentIndexatorJob
    {
        Task Execute();
    }
}