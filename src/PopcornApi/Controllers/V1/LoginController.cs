using Application.V1.Dtos.Admin.Users;
using Application.V1.Features.Users;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using PopcornApi.Model.Login;
using PopcornApi.Model.Settings;
using PopcornApi.Security.TokenServices;

namespace PopcornApi.Controllers.V1
{
    [ApiVersion("1.0")]
    public class LoginController(IMediator mediator, IAppSettings appSettings, ILogger<LoginController> logger, ITokenService tokenService) : ControllerBase(mediator)
    {
        private readonly IAppSettings appSettings = appSettings;
        private readonly ILogger<LoginController> logger = logger;
        private readonly ITokenService tokenService = tokenService;

        /// <summary>
        /// Validates user credential by user and password.
        /// </summary>
        /// <param name="userGetByNameAndPasswordDto">Credentials</param>
        /// <returns>Valid authentication session</returns>
        [AllowAnonymous]
        [EnableRateLimiting("LimitedAttempted")]
        [HttpPost("Login")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<AuthenticationDto>> GetByNameAndPassword(UserGetByNameAndPasswordDto userGetByNameAndPasswordDto)
        {
            UserGetDto? userGetDto = await mediator.Send(new GetByNameAndPassword.Query() { UserGetByNameAndPasswordDto = userGetByNameAndPasswordDto });

            if (userGetDto == null)
            {
                logger.LogWarning($"[{nameof(LoginController)}] Invalid login credentials - {userGetByNameAndPasswordDto.Name} ");
                return NotFound();
            }

            DateTime tokenCreatedAt = DateTime.UtcNow;
            DateTime tokenExpiredAt = tokenCreatedAt.AddMinutes(appSettings.Authentication.ExpireIn);
            DateTime refreshTokenExpiredAt = tokenCreatedAt.AddMinutes(appSettings.Authentication.RefreshIn);

            var token = tokenService.GenerateToken(userGetDto, tokenExpiredAt);
            var refreshToken = tokenService.GenerateRefreshToken();

            UserPostRefreshTokenDto userPutDto = new(
                userGetDto.Id,
                refreshToken,
                tokenCreatedAt,
                refreshTokenExpiredAt);

            await mediator.Send(new UpdateRefreshToken.Command() { UserPostRefreshTokenDto = userPutDto });

            return Ok(new AuthenticationDto(token, refreshToken, tokenExpiredAt));
        }

        /// <summary>
        /// Updates Token by expired authentication session.
        /// </summary>
        /// <param name="authenticationDto">Expired session</param>
        /// <returns>Valid authentication session</returns>
        [AllowAnonymous]
        [EnableRateLimiting("LimitedAttempted")]
        [HttpPost("RefreshToken")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<AuthenticationDto>> RefreshToken(AuthenticationDto authenticationDto)
        {
            if (authenticationDto == null || string.IsNullOrWhiteSpace(authenticationDto.Token) || string.IsNullOrWhiteSpace(authenticationDto.RefreshToken))
                return BadRequest();

            var principal = tokenService.GetTokenInformation(authenticationDto.Token);

            if (string.IsNullOrEmpty(principal?.Identity?.Name))
                return BadRequest();

            UserGetDto? userGetDto = await mediator.Send(new GetById.Query() { Id = principal.Identity.Name });

            if (userGetDto == null || string.IsNullOrWhiteSpace(userGetDto.RefreshToken) || userGetDto.RefreshTokenExpiredAt <= DateTime.UtcNow)
                return BadRequest();


            DateTime tokenCreatedAt = DateTime.UtcNow;
            DateTime tokenExpiredAt = tokenCreatedAt.AddMinutes(appSettings.Authentication.ExpireIn);
            DateTime refreshTokenExpiredAt = tokenCreatedAt.AddMinutes(appSettings.Authentication.RefreshIn);

            var token = tokenService.GenerateToken(userGetDto, tokenExpiredAt);
            var refreshToken = tokenService.GenerateRefreshToken();

            UserPostRefreshTokenDto userPutDto = new(
                userGetDto.Id,
                refreshToken,
                tokenCreatedAt,
                refreshTokenExpiredAt);

            await mediator.Send(new UpdateRefreshToken.Command() { UserPostRefreshTokenDto = userPutDto });

            return Ok(new AuthenticationDto(token, refreshToken, tokenExpiredAt));
        }

        /// <summary>
        /// Revokes user refresh token.
        /// </summary>
        /// <param name="id">User identification</param>
        /// <returns>User information.</returns>
        [HttpPost("Revoke/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UserGetDto>> Revoke(string id)
        {
            UserGetDto? userGetDto = await mediator.Send(new UpdateRefreshToken.Command() { UserPostRefreshTokenDto = new(id, null, null, null) });

            if (userGetDto == null)
                return BadRequest();

            return Ok(userGetDto);
        }

        /// <summary>
        /// Updates user password
        /// </summary>
        /// <param name="userPostUpdatePasswordDto">User Id with new password</param>
        /// <returns>Confirmation</returns>
        [HttpPost("UpdatePassword")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserGetDto>> UpdatePassword(UserPostUpdatePasswordDto userPostUpdatePasswordDto)
        {
            UserGetDto? user = await mediator.Send(new UpdatePassword.Command() { UserPostUpdatePasswordDto = userPostUpdatePasswordDto });

            if (user == null)
                return NotFound();

            return Ok(user);
        }
    }
}
