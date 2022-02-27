using BikeScanner.Application.Types;
using BikeScanner.Domain.Exceptions;
using BikeScanner.Domain.Extentions;
using BikeScanner.Domain.Models;
using BikeScanner.Domain.Repositories;
using System;
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

        public async Task<int> AddSub(Subscription sub)
        {
            if (!sub.SearchQuery.IsMinLength(2))
                throw AppError.Validation("Недопустимый поиск. Требуется минимум 2 символа.");

            if (!await NewSubsAvailable(sub.UserId)) 
                throw AppError.TooMuchSubs;

            if (await _subscriptionsRepository.HasActiveSub(sub.UserId, sub.SearchQuery))
                throw AppError.SubAlreadyExists(sub.SearchQuery);

            var entity = new SubscriptionEntity()
            {
                UserId = sub.UserId,
                SearchQuery = sub.SearchQuery,
                Created = DateTime.UtcNow,
                Status = SubscriptionStatus.Active
            };
            await _subscriptionsRepository.Add(entity);

            return entity.Id;
        }

        public async Task<Subscription> RemoveSub(int subId)
        {
            var entity = await _subscriptionsRepository.GetById(subId);
            if (entity == null) 
                throw AppError.NotExists("Подписка");

            entity.Status = SubscriptionStatus.Deleted;

            await _subscriptionsRepository.Update(entity);

            return new Subscription()
            {
                Id = entity.Id,
                SearchQuery = entity.SearchQuery,
                UserId = entity.UserId
            };
        }

        public async Task<Subscription[]> GetActiveSubs(long userId)
        {
            var result = await _subscriptionsRepository.GetUserSubs(userId, SubscriptionStatus.Active);

            return result
                .Select(r => new Subscription()
                {
                    Id = r.Id,
                    SearchQuery = r.SearchQuery,
                    UserId = r.UserId
                })
                .ToArray();
        }
    }
}
