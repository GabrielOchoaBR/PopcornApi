using Application.V1.Dtos.Admin.Users;
using Application.V1.Features.Users;
using AutoFixture;
using AutoFixture.Xunit2;
using Domain.V1.Entities.Users;
using FluentAssertions;
using Moq;

namespace UnitTests.Application.Features.V1.Users
{
    public class UpdateRefreshTokenTests : BaseRepositoryTests
    {
        private readonly UpdateRefreshToken.Handler handler;

        public UpdateRefreshTokenTests() 
            => handler = new(unitOfWorkMock.Object);

        [Theory, AutoData]
        public async void Handler_WrongData_ReturnsOk(UpdateRefreshToken.Command command)
        {
            //Arrange
            userRepositoryMock.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync((User)null!);

            //Act
            var result = await handler.Handle(command, CancellationToken.None);

            //Assert
            result.Should().BeNull();
        }

        [Theory, AutoData]
        public async void Handler_DidNotChange_ReturnsOk(UpdateRefreshToken.Command command)
        {
            //Arrange
            var user = fixture.Create<User>();
            userRepositoryMock.Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                              .ReturnsAsync(user);
            userRepositoryMock.Setup(x => x.ReplaceOneAsync(It.IsAny<User>()))
                              .ReturnsAsync((User)null!);

            //Act
            var result = await handler.Handle(command, CancellationToken.None);

            //Assert
            result.Should().BeNull();
        }

        [Theory, AutoData]
        public async void Handler_WithData_ReturnsOk(UpdateRefreshToken.Command command)
        {
            //Arrange
            var token = fixture.Create<string>();
            var user = fixture.Create<User>();

            user.RefreshToken = string.Empty;
            user.RefreshTokenCreatedAt = null;
            user.RefreshTokenExpiredAt = null;

            userRepositoryMock.Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                              .ReturnsAsync(user);
            userRepositoryMock.Setup(x => x.ReplaceOneAsync(It.IsAny<User>()))
                              .Callback<User>(param =>
                              {
                                  param.RefreshToken = token;
                                  param.RefreshTokenCreatedAt = DateTime.UtcNow;
                                  param.RefreshTokenExpiredAt = DateTime.UtcNow.AddMinutes(1);
                                  user = param;
                              })
                              .ReturnsAsync(user);

            //Act
            var result = await handler.Handle(command, CancellationToken.None);

            //Assert
            result.Should().NotBeNull().And.BeOfType<UserGetDto>();
            result!.RefreshToken.Should().NotBeNullOrEmpty().And.Be(user.RefreshToken);
            result.RefreshTokenCreatedAt.Should().NotBeNull().And.Be(user.RefreshTokenCreatedAt);
            result.RefreshTokenExpiredAt.Should().NotBeNull().And.Be(user.RefreshTokenExpiredAt);
        }
    }
}
