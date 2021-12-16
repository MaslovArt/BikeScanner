using System;

namespace BikeScanner.Domain.Exceptions
{
    public class NotificatorTypeException : Exception
    {
        public NotificatorTypeException(string notificationType)
            : base("No notificator implementation for type: " + notificationType)
        { }
    }
}
