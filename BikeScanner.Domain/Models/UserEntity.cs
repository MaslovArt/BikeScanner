namespace BikeScanner.Domain.Models
{
    public class UserEntity : BaseEntity
	{
		public long UserId { get; set; }
		public string SocialDisplayName { get; set; }
		public AccountState State { get; set; }
	}
}

