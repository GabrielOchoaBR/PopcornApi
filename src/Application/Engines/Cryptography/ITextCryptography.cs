
namespace Application.Engines.Cryptography
{
    public interface ITextCryptography
    {
        Task<string> HashAsync(string text);
        Task<bool> VerifyAsync(string text, string hash);
    }
}