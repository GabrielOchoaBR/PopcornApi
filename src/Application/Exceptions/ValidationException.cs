namespace Application.Exceptions
{
    public class ValidationException(IDictionary<string, string[]> errors) : ApplicationException(title, message)
    {
        private const string title = "Validation Failure";
        private const string message = "One or more validation errors occurred";

        public IDictionary<string, string[]> ErrorsDictionary { get; } = errors;
    }
}
