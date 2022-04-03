using System;

namespace BikeScanner.Domain.Exceptions
{
    public class AppError : Exception
    {
        public ErrorCode Code { get; set; }

        public AppError(ErrorCode code, string message)
            : base(message)
        {
            Code = code;
        }

        public static AppError Validation(string message) =>
            new AppError(ErrorCode.ValidationFail, message);

        public static AppError TooMuchSubs => 
            new AppError(ErrorCode.TooMuchSubs, "Достигнуто максимальное число подписок.");

        public static AppError SubAlreadyExists(string query) => 
            new AppError(ErrorCode.SubDuplicate, $"Подписка '{query}' уже существует.");

        public static AppError SearchLimit =>
            new AppError(ErrorCode.SearchLimit, $"Достигнут лимит поисков за сутки.");

        public static AppError NotExists(string description) =>
            new AppError(ErrorCode.ElementNotExists, $"{description} не существует.");

        public static AppError Forbidden(string message) =>
            new AppError(ErrorCode.Forbidden, message);
    }
}
