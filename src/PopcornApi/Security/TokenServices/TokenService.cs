using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Application.Engines.DataControl;
using Application.V1.Dtos.Admin.Users;
using Microsoft.IdentityModel.Tokens;
using PopcornApi.Model.Settings;

namespace PopcornApi.Security.TokenServices
{
    public class TokenService(IHttpContextAccessor httpContextAccessor, IAppSettings settings) : ITokenService, IAuthorizationService
    {
        private readonly IHttpContextAccessor httpContextAccessor = httpContextAccessor;

        private readonly IAppSettings appSettings = settings;
        public string GenerateToken(UserGetDto user, DateTime tokenExpiredAt)
        {
            var claims = new List<Claim>() {
                new(ClaimTypes.Name, user.Id),
                new("id", user.Id),
                new("name", user.Name),
                new("email", user.Email),
            };

            foreach (var role in user.Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.ToString()));
            }

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appSettings.Authentication.Key));

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(expires: tokenExpiredAt,
                                             claims: claims,
                                             signingCredentials: credentials);

            var tokenHandler = new JwtSecurityTokenHandler();

            return tokenHandler.WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            var randomBytes = new byte[128];

            using var randomNumberGenerator = RandomNumberGenerator.Create();

            randomNumberGenerator.GetBytes(randomBytes);

            var refreshToken = Convert.ToBase64String(randomBytes);

            return refreshToken;
        }

        public ClaimsPrincipal GetTokenInformation(string token)
        {
            if (string.IsNullOrEmpty(appSettings.Authentication.Key))
                throw new Exception(@"Key field in AppSettings\Authentication is required");

            var tokenValidationParameters = new TokenValidationParameters()
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateLifetime = false,

                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appSettings.Authentication.Key))
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken validatedToken);

            if (validatedToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCulture))
            {
                throw new Exception("Token invalid.");
            }

            return principal;
        }

        public string? GetUserId()
        {
            var httpContext = this.httpContextAccessor.HttpContext!;

            var userId = httpContext.User.Identity?.Name;

            if (userId != null)
            {
                return userId;
            }

            return null;
        }
    }
}
