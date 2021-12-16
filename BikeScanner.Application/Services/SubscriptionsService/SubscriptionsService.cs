using BikeScanner.Application.Models;
using BikeScanner.Domain.Exceptions;
using BikeScanner.Domain.Models;
using BikeScanner.Domain.Repositories;
using System.Linq;
using System.Threading.Tasks;

namespace BikeScanner.Application.Services.SubscriptionsService
{
    public class SubscriptionsService : ISubscriptionsService
    {
        private readonly ISubscriptionsRepository _subscriptionsRepository;

        public SubscriptionsService(ISubscriptionsRepository subscriptionsRepository)
        {
            _subscriptionsRepository = subscriptionsRepository;
        }

        //ToDo Add restriction
        public Task<bool> NewSubsAvailable(long userId)
        {
            return Task.FromResult(true);
        }

        public async Task<int> AddSub(SubscriptionModel sub)
        {
            if (!await NewSubsAvailable(sub.UserId)) 
                throw AppError.TooMuchSubs;

            if (await _subscriptionsRepository.SubExists(sub.UserId, sub.SearchQuery))
                throw AppError.SubAlreadyExists;

            var entity = new SubscriptionEntity()
            {
                UserId = sub.UserId,
                SearchQuery = sub.SearchQuery,
                NotificationType = sub.SubscriptionType,
                Status = SubscriptionStatus.Active
            };
            await _subscriptionsRepository.Add(entity);

            return entity.Id;
        }

        public async Task RemoveSub(int subId)
        {
            var entity = await _subscriptionsRepository.GetById(subId);
            if (entity == null) 
                throw AppError.NotExists("Подписка");

            entity.Status = SubscriptionStatus.Deleted;

            await _subscriptionsRepository.Update(entity);
        }

        public async Task<SubscriptionModel[]> GetActiveSubs(long userId)
        {
            var result = await _subscriptionsRepository.GetUserSubs(userId, SubscriptionStatus.Active);

            return result
                .Select(r => new SubscriptionModel()
                {
                    Id = r.Id,
                    SearchQuery = r.SearchQuery,
                    SubscriptionType = r.NotificationType,
                    UserId = r.UserId
                })
                .ToArray();
        }
    }
}
