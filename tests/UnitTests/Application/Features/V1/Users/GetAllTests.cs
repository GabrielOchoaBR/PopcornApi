using System.Linq.Expressions;
using Application.V1.Features.Users;
using AutoFixture;
using AutoFixture.Xunit2;
using Domain.V1.Entities.Users;
using FluentAssertions;
using Moq;

namespace UnitTests.Application.Features.V1.Users
{
    public class GetAllTests : BaseRepositoryTests
    {
        private readonly GetAll.Handler handler;

        public GetAllTests()
            => handler = new(unitOfWorkMock.Object);

        [Theory, AutoData]
        public async void Handler_WithData_ReturnsOk(GetAll.Query query)
        {
            //Arrange
            var users = fixture.CreateMany<User>().ToList();

            userRepositoryMock.Setup(x => x.FilterByAsync(It.IsAny<Expression<Func<User, bool>>>())).ReturnsAsync(users);

            //Act
            var result = await handler.Handle(query, CancellationToken.None);

            //Assert
            result.Should().NotBeNull();
            result.Count().Should().Be(users.Count());
        }
    }
}
