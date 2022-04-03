using System.Threading.Tasks;
using AutoMapper;
using BikeScanner.Application.Models.Users;
using BikeScanner.Domain.Exceptions;
using BikeScanner.Domain.Models;
using BikeScanner.Domain.Repositories;

namespace BikeScanner.Application.Services.UsersService
{
    public class UsersService : IUsersService
    {
        private readonly IUsersRepository _usersRepository;
        private readonly IMapper          _mapper;

        public UsersService(IUsersRepository usersRepository, IMapper mapper)
        {
            _usersRepository = usersRepository;
            _mapper = mapper;
        }

        public async Task<User> EnsureUser(long userId)
        {
            var user = await _usersRepository.FindUser(userId);
            if (user == null)
            {
                user = new UserEntity()
                {
                    UserId = userId,
                    AccountStatus = AccountStatus.Active
                };
                await _usersRepository.Add(user);
            }

            return _mapper.Map<User>(user);
        }

        public async Task<User> EnsureActiveUser(long userId)
        {
            var user = await _usersRepository.FindUser(userId);
            if (user == null)
            {
                user = new UserEntity()
                {
                    UserId = userId,
                    AccountStatus = AccountStatus.Active
                };
                await _usersRepository.Add(user);
            }

            else if (user.AccountStatus == AccountStatus.Ban)
            {
                throw AppError.Forbidden("Доступ ограничен.");
            }
            else if (user.AccountStatus == AccountStatus.Inactive)
            {
                user.AccountStatus = AccountStatus.Active;
                await _usersRepository.Update(user);
            }

            return _mapper.Map<User>(user);
        }

        public async Task ActivateUser(long userId)
        {
            var user = await _usersRepository.FindUser(userId)
                ?? throw AppError.NotExists($"Пользователь {userId}]");

            user.AccountStatus = AccountStatus.Active;
            await _usersRepository.Update(user);
        }

        public async Task DiactivateUser(long userId)
        {
            var user = await _usersRepository.FindUser(userId)
                ?? throw AppError.NotExists($"Пользователь {userId}]");

            user.AccountStatus = AccountStatus.Inactive;
            await _usersRepository.Update(user);
        }
    }
}

