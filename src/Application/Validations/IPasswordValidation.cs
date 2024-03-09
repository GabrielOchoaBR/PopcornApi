namespace Application.Validations
{
    public interface IPasswordValidation
    {
        string RegexPattern { get; set; }
        string ErrorMessage { get; set; }
    }
}