using Application.Engines.Cryptography;
using Application.V1.Dtos.Admin.Users;
using Application.V1.Features.Users;
using AutoFixture.Xunit2;
using Domain.V1.Entities.Users;
using FluentAssertions;
using Moq;

namespace UnitTests.Application.Features.V1.Users
{
    public class CreateTests : BaseRepositoryTests
    {
        private readonly Create.Handler handler;
        private readonly Mock<ITextCryptography> textCryptographyMock = new();

        public CreateTests()
        {
            handler = new(unitOfWorkMock.Object, textCryptographyMock.Object, userDataControlMock.Object);
        }

        [Theory, AutoData]
        public async void Handler_WrongData_ReturnsNull(Create.Command command)
        {
            //Arrange
            userRepositoryMock.Setup(x => x.InsertOneAsync(It.IsAny<User>()));
            textCryptographyMock.Setup(x => x.HashAsync(It.IsAny<string>())).ReturnsAsync(string.Empty);

            //Act
            var result = await handler.Handle(command, CancellationToken.None);

            //Assert
            result.Should().BeNull();
        }

        [Theory, AutoData]
        public async void Handler_CreateNewUser_ReturnsOk(Create.Command command)
        {
            //Arrange
            User? user = null;
            userRepositoryMock.Setup(x => x.InsertOneAsync(It.IsAny<User>()))
                              .Callback<User>(param =>
                              {
                                  user = param;
                                  user.Id = MongoDB.Bson.ObjectId.GenerateNewId();
                              });

            textCryptographyMock.Setup(x => x.HashAsync(It.IsAny<string>()))
                                .ReturnsAsync(string.Empty);


            //Act
            var result = await handler.Handle(command, CancellationToken.None);

            //Assert
            result.Should().NotBeNull().And.BeOfType<UserGetDto>();
            result!.Id.Should().Be(user!.Id.ToString());
            result.Name.Should().Be(user!.Name);
            result.Email.Should().Be(user!.Email);
        }
    }
}
