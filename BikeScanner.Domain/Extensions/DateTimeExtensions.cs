using System;

namespace BikeScanner.Domain.Extensions
{
    public static class DateTimeExtensions
    {
        public static long UnixStamp(this DateTime date) => ((DateTimeOffset)date).ToUnixTimeSeconds();
    }
}
