namespace Application.Configurations
{
    public interface IPasswordConfiguration
    {
        string RegexPattern { get; set; }
        string ErrorMessage { get; set; }
    }
}