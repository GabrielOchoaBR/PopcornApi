namespace Application.Validations
{
    public class PasswordValidation : IPasswordValidation
    {
        public required string RegexPattern { get; set; }
        public required string ErrorMessage { get; set; }
    }
}
