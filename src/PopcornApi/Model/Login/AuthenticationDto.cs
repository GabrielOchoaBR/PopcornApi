namespace PopcornApi.Model.Login
{
    public record AuthenticationDto(string Token, string RefreshToken, DateTime ExpiredAt);
}
