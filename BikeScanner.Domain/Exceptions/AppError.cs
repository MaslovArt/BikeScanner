﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public static AppError TooMuchSubs => 
            new AppError(ErrorCode.TooMuchSubs, "Достигнуто максимальное число подписок");

        public static AppError SubAlreadyExists => 
            new AppError(ErrorCode.SubDuplicate, "Такая подписка уже существует");

        public static AppError SearchLimit =>
            new AppError(ErrorCode.SearchLimit, $"Достигнут лимит поисков за сутки");

        public static AppError NotExists(string description) =>
            new AppError(ErrorCode.ElementNotExists, $"{description} не существует");
    }
}