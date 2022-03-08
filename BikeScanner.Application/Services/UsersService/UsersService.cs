using System.Threading.Tasks;
using BikeScanner.Domain.Exceptions;
using BikeScanner.Domain.Models;
using BikeScanner.Domain.Repositories;

namespace BikeScanner.Application.Services.UsersService
{
    public class UsersService : IUsersService
    {
        private readonly IUsersRepository _usersRepository;

        public UsersService(IUsersRepository usersRepository)
        {
            _usersRepository = usersRepository;
        }

        public async Task<UserEntity> EnsureUser(long userId)
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

            return user;
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

