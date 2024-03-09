using Microsoft.Extensions.Logging;

namespace Infrastructure.Logging
{
    public class CustomLoggerProviderConfig
    {
        public LogLevel LogLevel { get; set; } = LogLevel.Information;
        public int EventId { get; set; } = 0;

        public string? FileName = string.Empty;
    }
}
