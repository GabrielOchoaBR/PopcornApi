namespace PopcornApi.Model.Settings
{
    public interface IAppSettings
    {
        AuthenticationSettings Authentication { get; set; }
        DbContextSettings DbContext { get; set; }
        LoggerSettings Logger { get; set; }
        UserConfigurationSettings UserConfiguration { get; set; }
    }
}