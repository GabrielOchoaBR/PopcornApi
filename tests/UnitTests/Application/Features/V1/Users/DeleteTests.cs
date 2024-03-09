using Application.V1.Dtos.Admin.Users;
using Application.V1.Features.Users;
using AutoFixture;
using AutoFixture.Xunit2;
using Domain.V1.Entities.Users;
using FluentAssertions;
using Moq;

namespace UnitTests.Application.Features.V1.Users
{
    public class DeleteTests : BaseRepositoryTests
    {
        private readonly Delete.Handler handler;

        public DeleteTests() => handler = new(unitOfWorkMock.Object);

        [Theory, AutoData]
        public async void Handler_WrongIdentity_ReturnsNull(Delete.Command command)
        {
            //Arrange
            userRepositoryMock.Setup(x => x.DeleteByIdAsync(It.IsAny<string>()))
                              .ReturnsAsync((User)null!);

            //Act
            var result = await handler.Handle(command, CancellationToken.None);

            //Assert
            result.Should().BeNull();
        }


        [Theory, AutoData]
        public async void Handler_WithData_ReturnsOk(Delete.Command command)
        {
            //Arrange
            User user = fixture.Create<User>();

            userRepositoryMock.Setup(x => x.DeleteByIdAsync(It.IsAny<string>()))
                              .ReturnsAsync(user);

            //Act
            var result = await handler.Handle(command, CancellationToken.None);

            //Assert
            result.Should().NotBeNull().And.BeOfType<UserGetDto>();
            result.Id.Should().Be(user.Id.ToString());
        }
    }
}
