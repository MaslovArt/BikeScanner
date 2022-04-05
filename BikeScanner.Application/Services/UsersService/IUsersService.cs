using System.Threading.Tasks;
using BikeScanner.Application.Models.Users;

namespace BikeScanner.Application.Services.UsersService
{
    public interface IUsersService
    {
        Task ActivateUser(long userId);
        Task DiactivateUser(long userId);
        Task<User> EnsureUser(long userId);
    }
}

