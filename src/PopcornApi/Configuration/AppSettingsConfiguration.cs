using PopcornApi.Model.Settings;

namespace PopcornApi.Configuration
{
    public static class AppSettingsConfiguration
    {
        public static AppSettings GetSettings()
        {
            IConfigurationRoot configurationRoot = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            return new()
            {
                DbContext = new DbContextSettings()
                {
                    ConnectionString = configurationRoot["ConnectionStrings:DefaultConnection"]
                                        ?? throw new Exception(@"AppSettings\ConnectionString\DefaultConnection is required"),
                    DatabaseName = configurationRoot["ConnectionStrings:DatabaseName"]
                                        ?? throw new Exception(@"AppSettings\ConnectionString\DatabaseName is required"),
                },
                Authentication = new AuthenticationSettings()
                {
                    Key = configurationRoot["Authentication:Key"]!,
                    ExpireIn = Convert.ToInt32(configurationRoot["Authentication:ExpireIn"]),
                    RefreshIn = Convert.ToInt32(configurationRoot["Authentication:RefreshIn"]),
                },
                Logger = new LoggerSettings()
                {
                    LogLevel = (LogLevel)int.Parse(configurationRoot["Logger:LogLevel"]!),
                    EventId = int.Parse(configurationRoot["Logger:EventId"]!),
                    FileName = configurationRoot["Logger:FileName"]
                },
                UserConfiguration = new UserConfigurationSettings()
                {
                    PasswordValidation = configurationRoot["UserConfiguration:PasswordValidation"] ?? string.Empty,
                    PasswordValidationMessage = configurationRoot["UserConfiguration:PasswordValidationMessage"] ?? string.Empty
                }
            };
        }
    }
}
