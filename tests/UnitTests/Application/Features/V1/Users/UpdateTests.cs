using Application.V1.Dtos.Admin.Users;
using Application.V1.Features.Users;
using AutoFixture;
using AutoFixture.Xunit2;
using Domain.V1.Entities.Users;
using FluentAssertions;
using Moq;

namespace UnitTests.Application.Features.V1.Users
{
    public class UpdateTests : BaseRepositoryTests
    {
        private readonly Update.Handler handler;
        public UpdateTests() 
            => handler = new(unitOfWorkMock.Object);

        [Theory, AutoData]
        public async void Handler_WrongIdentity_ReturnsNull(Update.Command command)
        {
            //Arrange
            userRepositoryMock.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync((User)null!);

            //Act
            var result = await handler.Handle(command, CancellationToken.None);

            //Assert
            result.Should().BeNull();
        }

        [Theory, AutoData]
        public async void Handler_NotUpdate_ReturnsOk(Update.Command command)
        {
            //Arrange
            var userDb = fixture.Create<User>();

            userRepositoryMock.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(userDb);

            userRepositoryMock.Setup(x => x.ReplaceOneAsync(It.IsAny<User>()))
                              .ReturnsAsync((User)null!);

            //Act
            var result = await handler.Handle(command, CancellationToken.None);

            //Assert
            result.Should().BeNull();
        }

        [Theory, AutoData]
        public async void Handler_WithData_ReturnsOk(Update.Command command)
        {
            //Arrange
            var userDb = fixture.Create<User>();
            var userChanged = new User();

            userRepositoryMock.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(userDb);

            userRepositoryMock.Setup(x => x.ReplaceOneAsync(It.IsAny<User>()))
                              .Callback<User>(param =>
                              {
                                  userChanged = param;
                                  userChanged.UpdatedAt = DateTime.UtcNow;
                              })
                              .ReturnsAsync(userChanged);

            //Act
            var result = await handler.Handle(command, CancellationToken.None);

            //Assert
            result.Should().NotBeNull().And.BeOfType<UserGetDto>();
            result!.Id.Should().Be(userChanged!.Id.ToString());
            result!.Name.Should().Be(userChanged.Name);
            result!.UpdatedAt.Should().Be(userChanged.UpdatedAt);
            result!.CreatedAt.Should().Be(userChanged.CreatedAt);
            result!.CreatedBy.Should().Be(userChanged.CreatedBy.ToString());
            result!.UpdatedBy.Should().Be(userChanged.UpdatedBy.ToString());
        }
    }
}
