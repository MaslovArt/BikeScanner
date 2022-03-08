using System.Threading.Tasks;
using BikeScanner.Domain.Models;
using BikeScanner.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BikeScanner.Data.Postgre.Repositories
{
    public class UsersRepository : BaseRepository<UserEntity>, IUsersRepository
	{
        public UsersRepository(BikeScannerContext context)
            : base(context)
        { }

        public Task<UserEntity> FindUser(long socialId)
        {
            return Set.FirstOrDefaultAsync(e => e.UserId == socialId);
        }
    }
}

