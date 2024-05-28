namespace Application.Configurations
{
    public class PasswordConfiguration : IPasswordConfiguration
    {
        public required string RegexPattern { get; set; }
        public required string ErrorMessage { get; set; }
    }
}
