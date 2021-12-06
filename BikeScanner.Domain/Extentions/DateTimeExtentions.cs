using System;

namespace BikeScanner.Domain.Extentions
{
    public static class DateTimeExtentions
    {
        public static long UnixStamp(this DateTime date) => ((DateTimeOffset)date).ToUnixTimeSeconds();
    }
}
