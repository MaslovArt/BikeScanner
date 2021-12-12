using System.Threading.Tasks;

namespace BikeScanner.Application.Interfaces
{
    public interface IContentIndexatorJob
    {
        Task Execute();
    }
}