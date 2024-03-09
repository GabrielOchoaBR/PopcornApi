using Application.V1.Dtos.Admin.Users;
using Application.V1.Features.Users;
using AutoFixture;
using AutoFixture.Xunit2;
using Domain.V1.Entities.Users;
using FluentAssertions;
using Moq;

namespace UnitTests.Application.Features.V1.Users
{
    public class GetByIdTests : BaseRepositoryTests
    {
        private readonly GetById.Handler handler;
        public GetByIdTests() 
            => handler = new GetById.Handler(unitOfWorkMock.Object);

        [Theory, AutoData]
        public async void Handler_WithData_ReturnsOk(GetById.Query query)
        {
            //Arrange
            var user = fixture.Create<User>();

            userRepositoryMock.Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                              .ReturnsAsync(user);

            //Act
            var result = await handler.Handle(query, CancellationToken.None);

            //Assert
            result.Should().NotBeNull().And.BeOfType<UserGetDto>();
            result!.Id.Should().Be(user.Id.ToString());
            result.Name.Should().Be(user.Name);
            result.Email.Should().Be(user.Email);
            result.Roles.Count().Should().Be(result.Roles.Count());
        }

        [Theory, AutoData]
        public async void Handler_WithNoData_ReturnsNull(GetById.Query query)
        {
            //Arrange
            userRepositoryMock.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync((User)null!);

            //Act
            var result = await handler.Handle(query, CancellationToken.None);

            //Assert
            result.Should().BeNull();
        }
    }
}
