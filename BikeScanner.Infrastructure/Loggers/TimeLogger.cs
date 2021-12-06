﻿using System;
using Microsoft.Extensions.Logging;

namespace BikeScanner.Infrastructure.Loggers
{
    public class TimeLogger<T> : ILogger<T>
    {
        private readonly ILogger _logger;

        public TimeLogger(ILogger logger) => _logger = logger;

        public TimeLogger(ILoggerFactory loggerFactory) : this(new Logger<T>(loggerFactory)) { }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter) =>
            _logger.Log(logLevel, eventId, state, exception, (s, ex) => $"[{DateTime.UtcNow:HH:mm:ss.fff}]: {formatter(s, ex)}");

        public bool IsEnabled(LogLevel logLevel) => _logger.IsEnabled(logLevel);

        public IDisposable BeginScope<TState>(TState state) => _logger.BeginScope(state);
    }
}