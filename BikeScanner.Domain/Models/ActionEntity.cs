using System;
namespace BikeScanner.Domain.Models
{
	public class ActionEntity : BaseEntity
	{
		public long UserId { get; set; }
		public string Data { get; set; }
		public ActionType Action { get; set; }
		public DateTime Date { get; set; }

		public ActionEntity() { }

		public ActionEntity(long userId, string data, ActionType action)
		{
			UserId = userId;
			Data = data;
			Action = action;
			Date = DateTime.UtcNow;
		}
	}
}

