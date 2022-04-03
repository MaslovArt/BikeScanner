namespace BikeScanner.Application.Models.Subs
{
	public record Subscription
    {
        public int Id { get; set; }
        public long UserId { get; set; }
        public string SearchQuery { get; set; }
    }
}

