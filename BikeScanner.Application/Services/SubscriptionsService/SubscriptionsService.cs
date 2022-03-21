using BikeScanner.Domain.Exceptions;
using BikeScanner.Domain.Extentions;
using BikeScanner.Domain.Models;
using BikeScanner.Domain.Repositories;
using System;
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

        public async Task<SubscriptionEntity> AddSub(long userId, string searchQuery)
        {
            if (!searchQuery.IsMinLength(2))
                throw AppError.Validation("Недопустимый поиск. Требуется минимум 2 символа.");

            if (!await NewSubsAvailable(userId)) 
                throw AppError.TooMuchSubs;

            if (await _subscriptionsRepository.HasActiveSub(userId, searchQuery))
                throw AppError.SubAlreadyExists(searchQuery);

            var newSubscription = new SubscriptionEntity()
            {
                UserId = userId,
                SearchQuery = searchQuery,
                Created = DateTime.UtcNow,
                Status = SubscriptionStatus.Active
            };
            await _subscriptionsRepository.Add(newSubscription);

            return newSubscription;
        }

        public async Task<SubscriptionEntity> RemoveSub(int subId)
        {
            var entity = await _subscriptionsRepository.GetById(subId);
            if (entity == null) 
                throw AppError.NotExists("Подписка");

            entity.Status = SubscriptionStatus.Deleted;
            await _subscriptionsRepository.Update(entity);

            return entity;
        }

        public Task<SubscriptionEntity[]> GetActiveSubs(long userId)
        {
            return _subscriptionsRepository
                .GetUserSubs(userId, SubscriptionStatus.Active);
        }

        public async Task<SubscriptionEntity> GetSub(int subId)
        {
            var entity = await _subscriptionsRepository.GetById(subId);
            if (entity == null)
                throw AppError.NotExists("Подписка");

            return entity;
        }
    }
}
