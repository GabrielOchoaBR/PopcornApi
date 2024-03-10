using Application.V1.Dtos.Admin.Users;
using Application.V1.Features.Users;
using AutoFixture;
using AutoFixture.Xunit2;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PopcornApi.Controllers.V1;
using PopcornApi.Model.Login;
using PopcornApi.Model.Settings;
using PopcornApi.Security.TokenServices;

namespace UnitTests.PopcornApi.Controllers.V1
{
    public class LoginControllerTests : BaseControllerTests
    {
        private readonly LoginController loginController;

        public LoginControllerTests()
        {
            loginController = new(mediatorMock.Object, appSettingsMock.Object, loggerMock.Object, tokenServiceMock.Object);
        }


        [Theory, AutoData]
        public async void GetByNameAndPassword_WrongData_Returns404(UserGetByNameAndPasswordDto userGetByNameAndPasswordDto)
        {
            //Arrange
            mediatorMock.Setup(x => x.Send(It.IsAny<GetByNameAndPassword.Query>(),
                                            It.IsAny<CancellationToken>()))
                         .ReturnsAsync((UserGetDto)null!);

            //Act
            var getByNameAndPassword = await loginController.GetByNameAndPassword(userGetByNameAndPasswordDto);

            //Assert
            getByNameAndPassword.Should().NotBeNull();
            getByNameAndPassword.Result.Should().BeOfType<NotFoundResult>().Which.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }

        [Theory, AutoData]
        public async void GetByNameAndPassword_WithData_Returns200(UserGetByNameAndPasswordDto userGetByNameAndPasswordDto)
        {
            //Arrange
            appSettingsMock.SetupGet(m => m.Authentication).Returns(() => new AuthenticationSettings() { Key = fixture.Create<string>(), ExpireIn = 1, RefreshIn = 2 });

            var userGetDto = fixture.Create<UserGetDto>();
            mediatorMock.Setup(x => x.Send(It.IsAny<GetByNameAndPassword.Query>(),
                                            It.IsAny<CancellationToken>()))
                         .ReturnsAsync(userGetDto);

            var token = fixture.Create<string>();
            tokenServiceMock.Setup(x => x.GenerateToken(It.IsAny<UserGetDto>(), It.IsAny<DateTime>())).Returns(token);
            tokenServiceMock.Setup(x => x.GenerateRefreshToken()).Returns(fixture.Create<string>());

            //Act
            var getByNameAndPassword = await loginController.GetByNameAndPassword(userGetByNameAndPasswordDto);

            //Assert
            getByNameAndPassword.Should().NotBeNull();
            getByNameAndPassword.Result.Should().BeOfType<OkObjectResult>().Which.StatusCode.Should().Be(StatusCodes.Status200OK);

            OkObjectResult result = (OkObjectResult)getByNameAndPassword.Result!;
            ((AuthenticationDto)result.Value!).Token.Should().Be(token);

        }


        [Fact]
        public async void RefreshToken_WithData_Returns200()
        {
            //Arrange
            Mock<IHttpContextAccessor> httpContextAccessorMock = new();
            fixture.Customize<AppSettings>(c => c.With(x => x.Authentication, new AuthenticationSettings() { Key = fixture.Create<string>(), ExpireIn = 1, RefreshIn = 2 }));
            var appSettings = fixture.Create<AppSettings>();
            var tokenService = new TokenService(httpContextAccessorMock.Object, appSettings);

            appSettingsMock.SetupGet(m => m.Authentication).Returns(() => appSettings.Authentication);

            fixture.Customize<UserGetDto>(c => c.With(x => x.RefreshTokenExpiredAt, DateTime.UtcNow.AddMinutes(1)));
            var userGetDto = fixture.Create<UserGetDto>();
            string token = tokenService.GenerateToken(userGetDto, DateTime.UtcNow.AddMinutes(1));
            string refreshToken = tokenService.GenerateRefreshToken();

            fixture.Customize<AuthenticationDto>(c => c.With(x => x.Token, token));
            var authenticationDto = fixture.Create<AuthenticationDto>();

            var claimPrincipal = tokenService.GetTokenInformation(token);
            tokenServiceMock.Setup(x => x.GetTokenInformation(It.IsAny<string>())).Returns(claimPrincipal);

            mediatorMock.Setup(x => x.Send(It.Is<GetById.Query>(x => x.Id == claimPrincipal.Identity!.Name),
                                                        It.IsAny<CancellationToken>()))
                                     .ReturnsAsync(userGetDto);

            tokenServiceMock.Setup(x => x.GenerateToken(It.IsAny<UserGetDto>(), It.IsAny<DateTime>())).Returns(token);
            tokenServiceMock.Setup(x => x.GenerateRefreshToken()).Returns(refreshToken);

            //Act
            var refreshTokenResult = await loginController.RefreshToken(authenticationDto);

            //Assert
            refreshTokenResult.Should().NotBeNull();
            refreshTokenResult.Result.Should().BeOfType<OkObjectResult>().Which.StatusCode.Should().Be(StatusCodes.Status200OK);

            OkObjectResult result = (OkObjectResult)refreshTokenResult.Result!;
            ((AuthenticationDto)result.Value!).Token.Should().Be(token);
            ((AuthenticationDto)result.Value!).RefreshToken.Should().Be(refreshToken);
        }

        [Theory, AutoData]
        public async void Revoke_WrongData_Returns400(string id)
        {
            //Arrange
            mediatorMock.Setup(x => x.Send(It.IsAny<UpdateRefreshToken.Command>(), It.IsAny<CancellationToken>())).ReturnsAsync((UserGetDto)null!);

            //Act
            var revoke = await loginController.Revoke(id);

            //Assert
            revoke.Should().NotBeNull();
            revoke.Result.Should().BeOfType<BadRequestResult>().Which.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }

        [Theory, AutoData]
        public async void Revoke_WithData_Returns200(string id)
        {
            //Arrange
            fixture.Customize<UserGetDto>(c => c.With(x => x.Id, id));
            var userGetDto = fixture.Create<UserGetDto>();

            mediatorMock.Setup(x => x.Send(It.IsAny<UpdateRefreshToken.Command>(), It.IsAny<CancellationToken>())).ReturnsAsync(userGetDto);

            //Act
            var revoke = await loginController.Revoke(id);

            //Assert
            revoke.Should().NotBeNull();
            revoke.Result.Should().BeOfType<OkObjectResult>().Which.StatusCode.Should().Be(StatusCodes.Status200OK);

            OkObjectResult result = (OkObjectResult)revoke.Result!;
            ((UserGetDto)result.Value!).Id.Should().Be(id);
        }


        [Theory, AutoData]
        public async void UpdatePassword_WrongIdentity_Returns404(UserPostUpdatePasswordDto userPostUpdatePasswordDto)
        {
            //Arrange
            mediatorMock.Setup(x => x.Send(It.IsAny<UpdatePassword.Command>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync((UserGetDto)null!);

            //Act
            var updatePassword = await loginController.UpdatePassword(userPostUpdatePasswordDto);

            //Assert
            updatePassword.Result.Should().NotBeNull();
            updatePassword.Result.Should().BeOfType<NotFoundResult>().Which.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }

        [Theory, AutoData]
        public async void UpdatePassword_WithData_Returns200(UserPostUpdatePasswordDto userPostUpdatePasswordDto)
        {
            //Arrange
            fixture.Customize<UserGetDto>(c => c.With(x => x.Id, userPostUpdatePasswordDto.Id));
            var userGetDto = fixture.Create<UserGetDto>();

            mediatorMock.Setup(x => x.Send(It.IsAny<UpdatePassword.Command>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(userGetDto);

            //Act
            var updatePassword = await loginController.UpdatePassword(userPostUpdatePasswordDto);

            //Assert
            updatePassword.Result.Should().NotBeNull();
            updatePassword.Result.Should().BeOfType<OkObjectResult>().Which.StatusCode.Should().Be(StatusCodes.Status200OK);

            OkObjectResult result = (OkObjectResult)updatePassword.Result!;
            ((UserGetDto)result.Value!).Id.Should().Be(userPostUpdatePasswordDto.Id);
        }
    }
}
