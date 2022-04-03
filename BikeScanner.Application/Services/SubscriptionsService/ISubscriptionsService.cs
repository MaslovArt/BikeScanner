using BikeScanner.Application.Models.Subs;
using System.Threading.Tasks;

namespace BikeScanner.Application.Services.SubscriptionsService
{
    /// <summary>
    /// Subs service
    /// </summary>
    public interface ISubscriptionsService
    {
        /// <summary>
        /// Add new user sub
        /// </summary>
        /// <param name="sub"></param>
        /// <returns></returns>
        Task<Subscription> AddSub(long userId, string searchQuery);

        /// <summary>
        /// Get user active subs
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<Subscription[]> GetActiveSubs(long userId);

        /// <summary>
        /// Remove user sub
        /// </summary>
        /// <param name="subId"></param>
        /// <returns></returns>
        Task<Subscription> RemoveSub(int subId);

        /// <summary>
        /// Get user sub
        /// </summary>
        /// <param name="subId"></param>
        /// <returns></returns>
        Task<Subscription> GetSub(int subId);
    }
}