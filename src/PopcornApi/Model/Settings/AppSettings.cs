namespace PopcornApi.Model.Settings
{
    public class AppSettings : IAppSettings
    {
        public required DbContextSettings DbContext { get; set; }
        public required AuthenticationSettings Authentication { get; set; }
        public required LoggerSettings Logger { get; set; }
        public required UserConfigurationSettings UserConfiguration { get; set; }
    }
}
