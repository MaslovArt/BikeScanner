namespace BikeScanner.Infrastructure.VK.Api.Abstraction
{
    internal interface IVkApiMethod
    {
        /// <summary>
        /// Return api method url with parameters.
        /// </summary>
        /// <returns>Uri</returns>
        string GetRequestString();
    }
}
