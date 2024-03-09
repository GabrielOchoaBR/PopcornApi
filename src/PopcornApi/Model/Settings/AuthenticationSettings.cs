namespace PopcornApi.Model.Settings
{
    public class AuthenticationSettings
    {
        public required string Key { get; set; }
        public int ExpireIn { get; set; } = 10;
        public int RefreshIn { get; set; } = 20;
    }
}
