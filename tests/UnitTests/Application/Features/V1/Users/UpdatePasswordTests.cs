using Application.Engines.Cryptography;
using Application.V1.Dtos.Admin.Users;
using Application.V1.Features.Users;
using AutoFixture;
using AutoFixture.Xunit2;
using Domain.V1.Entities.Users;
using FluentAssertions;
using Moq;

namespace UnitTests.Application.Features.V1.Users
{
    public class UpdatePasswordTests : BaseRepositoryTests
    {
        private readonly UpdatePassword.Handler handler;
        private readonly Mock<ITextCryptography> textCryptographyMock = new();

        public UpdatePasswordTests()
        {
            handler = new(unitOfWorkMock.Object, textCryptographyMock.Object);
        }

        [Theory, AutoData]
        public async void Handler_WrongData_ReturnsOk(UpdatePassword.Command command)
        {
            //Arrange
            userRepositoryMock.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync((User)null!);
            textCryptographyMock.Setup(x => x.VerifyAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(false);

            //Act
            var result = await handler.Handle(command, CancellationToken.None);

            //Assert
            result.Should().BeNull();
        }

        [Theory, AutoData]
        public async void Handler_DidNotChange_ReturnsOk(UpdatePassword.Command command)
        {
            //Arrange
            var newPassword = fixture.Create<string>();
            var user = fixture.Create<User>();
            userRepositoryMock.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(user);
            userRepositoryMock.Setup(x => x.ReplaceOneAsync(It.IsAny<User>())).ReturnsAsync((User)null!);
            textCryptographyMock.Setup(x => x.VerifyAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(true);
            textCryptographyMock.Setup(x => x.HashAsync(It.IsAny<string>())).ReturnsAsync(newPassword);

            //Act
            var result = await handler.Handle(command, CancellationToken.None);

            //Assert
            result.Should().BeNull();
        }

        [Theory, AutoData]
        public async void Handler_WithRightNameAndPassword_ReturnsOk(UpdatePassword.Command command)
        {
            //Arrange
            var newPassword = fixture.Create<string>();
            var user = fixture.Create<User>();

            userRepositoryMock.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(user);
            userRepositoryMock.Setup(x => x.ReplaceOneAsync(It.IsAny<User>())).ReturnsAsync(user);

            textCryptographyMock.Setup(x => x.VerifyAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(true);
            textCryptographyMock.Setup(x => x.HashAsync(It.IsAny<string>())).ReturnsAsync(newPassword);

            //Act
            var result = await handler.Handle(command, CancellationToken.None);

            //Assert
            result.Should().NotBeNull().And.BeOfType<UserGetDto>();

        }
    }
}
