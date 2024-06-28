using Application.V1.Dtos.Admin.Users;
using AutoFixture;
using Domain.V1.Entities.Users;
using FluentAssertions;
using PopcornApi.Model.Login;
using PopcornClient.Model;
using PopcornClient.V1;
namespace IntegrationTests.PopcornClient.V1
{
    public class LoginClientTests : BaseTests
    {
        private const string Password = "PasswordStrong@123";

        [Fact]
        public async Task GetByNameAndPassword_WrongAttempt_ReturnsNull()
        {
            //Arrange
            var loginClient = new LoginClient(string.Empty, httpClientFactory);

            //Act
            var result = await loginClient.GetByNameAndPassword(new UserGetByNameAndPasswordDto(string.Empty, string.Empty), CancellationToken.None);

            //Assert
            result.Should().NotBeNull().And.BeOfType<ResponseDto<AuthenticationDto>>();
            result.IsSuccess.Should().BeFalse();
            result.FailedResult!.StatusCode.Should().Be(422);
            result.FailedResult.ExceptionResponse.Should().NotBeNull();
        }

        [Fact]
        public async Task GetByNameAndPassword_NoUserRegistered_ReturnsNull()
        {
            //Arrange
            var loginClient = new LoginClient(string.Empty, httpClientFactory);

            //Act
            var result = await loginClient.GetByNameAndPassword(new UserGetByNameAndPasswordDto("User Name", "PasswordStrong!@#!@#"), CancellationToken.None);

            //Assert
            result.Should().NotBeNull().And.BeOfType<ResponseDto<AuthenticationDto>>();
            result.IsSuccess.Should().BeFalse();
            result.FailedResult!.StatusCode.Should().Be(404);
        }


        [Fact]
        public async Task GetByNameAndPassword_WithData_ReturnsOk()
        {
            //Arrange
            User user = webApp.UserFixtures.CreateUser().First();
            user.Password = await webApp.TextCryptography.HashAsync(Password);
            await webApp.UnitOfWork.GetUserRepository().InsertOneAsync(user);

            //Act
            var loginClient = new LoginClient(string.Empty, httpClientFactory);
            var result = await loginClient.GetByNameAndPassword(new UserGetByNameAndPasswordDto(user.Name, Password), CancellationToken.None);

            //Assert
            result.Should().NotBeNull();
            result.Response!.Token.Should().NotBeNull();
            result.Response.RefreshToken.Should().NotBeNull();
            result.Response.ExpiredAt.Should().BeAfter(DateTime.UtcNow);
        }

        [Fact]
        public async Task RefreshToken_WithData_ReturnsOk()
        {
            //Arrange
            var user = fixture.Build<User>()
                .With(x => x.Name, "user")
                .With(x => x.Password, await webApp.TextCryptography.HashAsync(Password))
                .With(x => x.RefreshTokenCreatedAt, DateTime.UtcNow.AddMinutes(1))
                .With(x => x.RefreshTokenExpiredAt, DateTime.UtcNow.AddMinutes(1))
                .Create();
            await webApp.UnitOfWork.GetUserRepository().InsertOneAsync(user);

            //Act
            var loginClient = new LoginClient(string.Empty, httpClientFactory);
            var authenticationDto = await loginClient.GetByNameAndPassword(new UserGetByNameAndPasswordDto(user.Name, Password), CancellationToken.None);
            var result = await loginClient.RefreshToken(authenticationDto.Response!, CancellationToken.None);

            //Assert
            result.Should().NotBeNull();
            result.Response!.Token.Should().NotBeNull();
            result.Response.RefreshToken.Should().NotBeNull();
            result.Response.ExpiredAt.Should().BeAfter(DateTime.UtcNow);
        }

        [Fact]
        public async Task Revoke_WithData_ReturnsOk()
        {
            //Arrange
            string token = webApp.UserFixtures.CreateToken([]);
            User user = webApp.UserFixtures.CreateUser().First();
            await webApp.UnitOfWork.GetUserRepository().InsertOneAsync(user);

            //Act
            var loginClient = new LoginClient(string.Empty, httpClientFactory, token);
            var result = await loginClient.Revoke(user.Id.ToString(), CancellationToken.None);

            //Assert
            result.Should().NotBeNull();
            result.Response!.Id.Should().Be(user.Id.ToString());
            result.Response.RefreshToken.Should().BeNull();
            result.Response.RefreshTokenCreatedAt.Should().BeNull();
            result.Response.RefreshTokenExpiredAt.Should().BeNull();
        }

        [Fact]
        public async Task UpdatePassword_WithData_ReturnsOk()
        {
            //Arrange
            string newPassword = "NewStrongPassword@123";
            string oldPassword = Password;

            string token = webApp.UserFixtures.CreateToken([]);
            User user = webApp.UserFixtures.CreateUser().First();
            user.Password = await webApp.TextCryptography.HashAsync(oldPassword);
            await webApp.UnitOfWork.GetUserRepository().InsertOneAsync(user);

            //Act
            var loginClient = new LoginClient(string.Empty, httpClientFactory, token);
            var result = await loginClient.UpdatePassword(new UserPostUpdatePasswordDto(user.Id.ToString(), oldPassword, newPassword), CancellationToken.None);
            var userDb = await webApp.UnitOfWork.GetUserRepository().FindOneAsync(x => true);

            //Assert
            result.Should().NotBeNull();
            result.Response!.Id.Should().Be(user.Id.ToString());

            (await webApp.TextCryptography.VerifyAsync(newPassword, userDb!.Password)).Should().Be(true);
        }
    }
}