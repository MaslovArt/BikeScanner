using System.Threading.Tasks;
using BikeScanner.Domain.Models;

namespace BikeScanner.Application.Services.UsersService
{
    public interface IUsersService
    {
        Task ActivateUser(long userId);
        Task DiactivateUser(long userId);
        Task<UserEntity> EnsureUser(long userId);
    }
}