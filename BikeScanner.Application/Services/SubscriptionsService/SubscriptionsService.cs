using AutoMapper;
using BikeScanner.Application.Models.Subs;
using BikeScanner.Domain.Exceptions;
using BikeScanner.Domain.Extensions;
using BikeScanner.Domain.Models;
using BikeScanner.Domain.Repositories;
using System;
using System.Threading.Tasks;

namespace BikeScanner.Application.Services.SubscriptionsService
{
    public class SubscriptionsService : ISubscriptionsService
    {
        private readonly ISubscriptionsRepository _subscriptionsRepository;
        private readonly IActionsRepository       _actionsRepository;
        private readonly IMapper                  _mapper;

        public SubscriptionsService(
            ISubscriptionsRepository subscriptionsRepository,
            IActionsRepository actionsRepository,
            IMapper mapper
            )
        {
            _subscriptionsRepository = subscriptionsRepository;
            _actionsRepository = actionsRepository;
            _mapper = mapper;
        }

        public async Task<Subscription> AddSub(long userId, string searchQuery)
        {
            if (!searchQuery.IsMinLength(2))
                throw AppError.Validation("Недопустимый поиск. Требуется минимум 2 символа.");

            if (await _subscriptionsRepository.IsSubExists(userId, searchQuery))
                throw AppError.SubAlreadyExists(searchQuery);

            var subscription = new SubscriptionEntity()
            {
                UserId = userId,
                SearchQuery = searchQuery,
                Created = DateTime.UtcNow
            };
            await _subscriptionsRepository.Add(subscription);
            await _actionsRepository.LogSubscription(userId, searchQuery);

            return _mapper.Map<Subscription>(subscription);
        }

        public async Task<Subscription> RemoveSub(int subId)
        {
            var subscription = await _subscriptionsRepository.GetById(subId);
            if (subscription == null) 
                throw AppError.NotExists("Подписка");

            await _subscriptionsRepository.Remove(subscription);

            return _mapper.Map<Subscription>(subscription); ;
        }

        public async Task<Subscription[]> GetSubs(long userId)
        {
            var subscriptions = await _subscriptionsRepository.GetSubs(userId);

            return _mapper.Map<Subscription[]>(subscriptions);
        }

        public async Task<Subscription> GetSub(int subId)
        {
            var subscription = await _subscriptionsRepository.GetById(subId);
            if (subscription == null)
                throw AppError.NotExists("Подписка");

            return _mapper.Map<Subscription>(subscription);
        }
    }
}
