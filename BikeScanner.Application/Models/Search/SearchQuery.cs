namespace BikeScanner.Application.Models.Search
{
	public record SearchQuery(long UserId, string Query, int Take, int Skip);
}

