using BikeScanner.Application.Models.Search;
using BikeScanner.Domain.Models;
using System.Threading.Tasks;

namespace BikeScanner.Application.Services.SearchService
{
    /// <summary>
    /// Ads search service
    /// </summary>
    public interface ISearchService
    {
        /// <summary>
        /// Full search 
        /// </summary>
        /// <param name="userId">Search initiator user id</param>
        /// <param name="query">Search query</param>
        /// <returns></returns>
        Task<Page<SearchResult>> Search(long userId, string query, int skip, int take);

        /// <summary>
        /// Search only last indexed ads
        /// </summary>
        /// <param name="userId">Search initiator user id</param>
        /// <param name="query">Search query</param>
        /// <returns></returns>
        Task<SearchResult[]> SearchEpoch(long userId, string query, long indexingStamp);
    }
}