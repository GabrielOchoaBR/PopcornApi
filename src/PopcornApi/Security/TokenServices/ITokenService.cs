using System.Security.Claims;
using Application.V1.Dtos.Admin.Users;

namespace PopcornApi.Security.TokenServices
{
    public interface ITokenService
    {
        string GenerateRefreshToken();
        string GenerateToken(UserGetDto user, DateTime tokenExpiresAt);
        ClaimsPrincipal GetTokenInformation(string token);
    }
}