using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Logging
{
    [ExcludeFromCodeCoverage]
    public class CustomLoggerProvider(CustomLoggerProviderConfig loggerConfig) : ILoggerProvider
    {
        private readonly CustomLoggerProviderConfig loggerConfig = loggerConfig;

        private readonly ConcurrentDictionary<string, CustomLogger> customLogger = new();

        public ILogger CreateLogger(string categoryName)
        {
            return customLogger.GetOrAdd(categoryName, name => new CustomLogger(name, loggerConfig));
        }

        public void Dispose()
        {
            customLogger.Clear();
        }
    }
}
