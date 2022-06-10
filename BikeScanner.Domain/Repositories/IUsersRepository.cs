using System.Threading.Tasks;
using BikeScanner.Domain.Models;

namespace BikeScanner.Domain.Repositories
{
	public interface IUsersRepository : IRepository<UserEntity>
	{
		Task<UserEntity> FindUser(long socialId);
		Task<UserEntity[]> GetBlackList();
	}
}
