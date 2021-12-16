using BikeScanner.Application.Models;
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
        Task<SearchResultModel[]> Search(long userId, string query);

        /// <summary>
        /// Check if user can search
        /// </summary>
        /// <param name="userId">User id</param>
        /// <returns></returns>
        public Task<bool> CanSearch(long userId);

        /// <summary>
        /// Search only last indexed ads
        /// </summary>
        /// <param name="userId">Search initiator user id</param>
        /// <param name="query">Search query</param>
        /// <returns></returns>
        Task<SearchResultModel[]> SearchEpoch(long userId, string query, long indexingStamp);
    }
}