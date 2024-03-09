namespace Application.V1.Dtos.Admin.Users
{
    public record UserPostRefreshTokenDto(string Id,
                                         string? RefreshToken,
                                         DateTime? RefreshTokenCreatedAt,
                                         DateTime? RefreshTokenExpiredAt);
}
