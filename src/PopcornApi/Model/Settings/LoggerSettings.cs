﻿namespace PopcornApi.Model.Settings
{
    public class LoggerSettings
    {
        public LogLevel LogLevel { get; set; } = LogLevel.Information;
        public int EventId { get; set; } = 0;

        public string? FileName = string.Empty;
    }
}
