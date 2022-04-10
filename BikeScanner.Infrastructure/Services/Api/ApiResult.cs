namespace BikeScanner.Infrastructure.Services.Api
{
	internal class ApiResult<TResult, TError>
	{
		public TResult Result { get; set; }
		public TError Error { get; set; }
		public bool IsSuccess => Error == null && Result != null;
	}
}

