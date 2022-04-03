using System;
namespace BikeScanner.Domain.Models
{
	public class DevMessageEntity : BaseEntity
	{
		public long UserId { get; set; }
		public string Message { get; set; }
		public DateTime Created { get; set; }
		public bool Viewed { get; set; }

        public DevMessageEntity()
        { }

        public DevMessageEntity(long userId, string message)
        {
			UserId = userId;
			Message = message;
			Created = DateTime.UtcNow;
			Viewed = false;
        }
	}
}

