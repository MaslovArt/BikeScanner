using BikeScanner.Application.Types;
using System.Threading.Tasks;

namespace BikeScanner.Application.Services.SubscriptionsService
{
    /// <summary>
    /// Subs service
    /// </summary>
    public interface ISubscriptionsService
    {
        /// <summary>
        /// Check if user can add more subs
        /// </summary>
        /// <param name="userId">User id</param>
        /// <returns></returns>
        Task<bool> NewSubsAvailable(long userId);

        /// <summary>
        /// Add new user sub
        /// </summary>
        /// <param name="sub"></param>
        /// <returns></returns>
        Task<int> AddSub(Subscription sub);

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
    }
}