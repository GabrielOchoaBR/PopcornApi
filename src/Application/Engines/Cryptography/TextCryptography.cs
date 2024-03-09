using System.Diagnostics.CodeAnalysis;

namespace Application.Engines.Cryptography
{
    [ExcludeFromCodeCoverage]
    public class TextCryptography : ITextCryptography
    {
        public async Task<bool> VerifyAsync(string text, string hash)
            => await Task.Run(() => BCrypt.Net.BCrypt.Verify(text, hash));

        public async Task<string> HashAsync(string text)
            => await Task.Run(() => BCrypt.Net.BCrypt.HashPassword(text));
    }
}
