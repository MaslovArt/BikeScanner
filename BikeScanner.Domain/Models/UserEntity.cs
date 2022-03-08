namespace BikeScanner.Domain.Models
{
    public class UserEntity : BaseEntity
	{
		public long UserId { get; set; }
		public string SocialDisplayName { get; set; }
		//public SocialType SocialType { get; set; }
		public AccountStatus AccountStatus { get; set; }
	}
}

