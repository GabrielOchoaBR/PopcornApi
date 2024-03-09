using Microsoft.Extensions.Logging;

namespace Infrastructure.Logging
{
    public class CustomLogger(string loggerName, CustomLoggerProviderConfig customLoggerProviderConfig) : ILogger
    {
        private readonly string loggerName = loggerName;
        private readonly CustomLoggerProviderConfig customLoggerProviderConfig = customLoggerProviderConfig;

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            if (string.IsNullOrWhiteSpace(customLoggerProviderConfig.FileName))
                return false;

            return logLevel == customLoggerProviderConfig.LogLevel;
        }

        public async void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            string message = $"{logLevel} - {eventId} - {formatter(state, exception)}";

            await File.AppendAllLinesAsync(Util.General.ApiBasePath + customLoggerProviderConfig.FileName, [message]);
        }
    }
}
