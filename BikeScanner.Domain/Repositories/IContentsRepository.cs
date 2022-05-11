using BikeScanner.Domain.Models;
using System;
using System.Threading.Tasks;

namespace BikeScanner.Domain.Repositories
{
    public interface IContentsRepository : IRepository<ContentEntity>
    {
        Task<ContentEntity[]> Scroll(int skip, int take);
        Task<Page<ContentEntity>> Search(string query, int skip, int take, DateTime? since = null);
        Task<int> CountSearch(string query);
        Task<ContentEntity[]> GetContents(DateTime createdSince);
        Task<string[]> GetAllUrls();
    }
}
