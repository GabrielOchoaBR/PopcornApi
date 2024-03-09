using System.Linq.Expressions;
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
    public class GetByNameAndPasswordTests : BaseRepositoryTests
    {
        private readonly GetByNameAndPassword.Handler handler;
        private readonly Mock<ITextCryptography> textCryptographyMock = new();

        public GetByNameAndPasswordTests()
        {
            handler = new(unitOfWorkMock.Object, textCryptographyMock.Object);
        }

        [Theory, AutoData]
        public async void Handler_WithData_ReturnsOk(GetByNameAndPassword.Query query)
        {
            //Arrange
            var user = fixture.Create<User>();

            userRepositoryMock.Setup(x => x.FindOneAsync(It.IsAny<Expression<Func<User, bool>>>()))
                              .ReturnsAsync(user);

            textCryptographyMock.Setup(x => x.VerifyAsync(It.IsAny<string>(), It.IsAny<string>()))
                                .ReturnsAsync(true);

            //Act
            var result = await handler.Handle(query, CancellationToken.None);

            //Assert
            result.Should().NotBeNull().And.BeOfType<UserGetDto>();
            result!.Id.Should().Be(user.Id.ToString());
        }

        [Theory, AutoData]
        public async void Handler_WrongPassword_ReturnsOk(GetByNameAndPassword.Query query)
        {
            //Arrange
            var user = fixture.Create<User>();

            userRepositoryMock.Setup(x => x.FindOneAsync(It.IsAny<Expression<Func<User, bool>>>()))
                              .ReturnsAsync(user);

            textCryptographyMock.Setup(x => x.VerifyAsync(It.IsAny<string>(), It.IsAny<string>()))
                                .ReturnsAsync(false);

            //Act
            var result = await handler.Handle(query, CancellationToken.None);

            //Assert
            result.Should().BeNull();
        }
    }
}
