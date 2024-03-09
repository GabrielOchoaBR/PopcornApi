namespace Application.Exceptions
{
    public class ApplicationException(string title, string message) : Exception(message)
    {
        public string Title { get; set; } = title;
    }
}
