using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BikeScanner.Application.Models.Users;
using BikeScanner.Domain.Exceptions;
using BikeScanner.Domain.Models;
using BikeScanner.Domain.Repositories;
using Microsoft.Extensions.Caching.Memory;

namespace BikeScanner.Application.Services.UsersService
{
    public class UsersService : IUsersService
    {
        private readonly IUsersRepository _usersRepository;
        private readonly IMapper          _mapper;
        private readonly IMemoryCache     _cache;

        public UsersService(
            IUsersRepository usersRepository,
            IMapper mapper,
            IMemoryCache cache)
        {
            _usersRepository = usersRepository;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<User> EnsureUser(long userId)
        {
            var user = await _usersRepository.FindUser(userId);
            if (user == null)
            {
                user = new UserEntity()
                {
                    UserId = userId,
                    State = AccountState.Active
                };
                await _usersRepository.Add(user);
            }

            return _mapper.Map<User>(user);
        }

        public async Task ActivateUser(long userId)
        {
            var user = await _usersRepository.FindUser(userId)
                ?? throw AppError.NotExists($"Пользователь {userId}]");

            user.State = AccountState.Active;
            await _usersRepository.Update(user);
        }

        public async Task DiactivateUser(long userId)
        {
            var user = await _usersRepository.FindUser(userId)
                ?? throw AppError.NotExists($"Пользователь {userId}]");

            user.State = AccountState.Inactive;
            await _usersRepository.Update(user);
        }

        public async Task<bool> IsBanned(long userId)
        {
            var blackListUsers = _cache.Get<long[]>("black_list");
            if (blackListUsers == null)
            {
                var blackList = await _usersRepository.GetBlackList();
                blackListUsers = blackList.Select(l => l.UserId).ToArray();
            }

            return blackListUsers.Any(u => u == userId);
        }
    }
}

