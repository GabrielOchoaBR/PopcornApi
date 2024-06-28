using Application.V1.Dtos.Admin.Users;
using AutoFixture;
using Domain.V1.Entities.Users;
using FluentAssertions;
using MongoDB.Bson;
using PopcornClient.Model;
using PopcornClient.V1;

namespace IntegrationTests.PopcornClient.V1
{
    public class UserClientTests : BaseTests
    {
        private const string Password = "PAsSwOrdStrOng@123%";
        private const string Email = "email@email.com";

        [Fact]
        public async Task GetAll_WithData_ReturnsOk()
        {
            //Arrange
            await webApp.UnitOfWork.GetUserRepository().InsertOneAsync(webApp.UserFixtures.CreateUser().First());
            string token = webApp.UserFixtures.CreateToken([RoleType.Read]);

            //Act
            var userClient = new UserClient(string.Empty, httpClientFactory, token);
            var result = await userClient.GetAllAsync(CancellationToken.None);

            //Assert
            result.Should().NotBeNull().And.BeOfType<ResponseDto<IEnumerable<UserGetDto>>>();
            result.Response.Should().HaveCount(1);
        }

        [Fact]
        public async Task GetById_WrongData_ReturnsOk()
        {
            //Arrange
            User user = webApp.UserFixtures.CreateUser().First();
            await webApp.UnitOfWork.GetUserRepository().InsertOneAsync(user);
            string token = webApp.UserFixtures.CreateToken([RoleType.Read]);

            //Act
            var userClient = new UserClient(string.Empty, httpClientFactory, token);
            var result = await userClient.GetByIdAsync(ObjectId.GenerateNewId().ToString(), CancellationToken.None);

            //Assert
            result.Should().NotBeNull().And.BeOfType<ResponseDto<UserGetDto>>();
            result.IsSuccess.Should().BeFalse();
            result.FailedResult.Should().NotBeNull();
            result.FailedResult!.StatusCode.Should().Be(404);
        }

        [Fact]
        public async Task GetById_WithData_ReturnsOk()
        {
            //Arrange
            User user = webApp.UserFixtures.CreateUser().First();
            await webApp.UnitOfWork.GetUserRepository().InsertOneAsync(user);
            string token = webApp.UserFixtures.CreateToken([RoleType.Read]);

            //Act
            var userClient = new UserClient(string.Empty, httpClientFactory, token);
            var result = await userClient.GetByIdAsync(user.Id.ToString(), CancellationToken.None);

            //Assert
            result.Should().NotBeNull().And.BeOfType<ResponseDto<UserGetDto>>();
            result.IsSuccess.Should().BeTrue();
            result.Response!.Id.Should().Be(user.Id.ToString());
            result.Response.Name.Should().Be(user.Name);
            result.Response.Email.Should().Be(user.Email);
        }

        [Fact]
        public async Task Create_WithData_ReturnsOk()
        {
            //Arrange
            var userPostDto = fixture.Build<UserPostDto>()
                .With(x => x.Password, Password)
                .With(x => x.Email, Email)
                .Create();
            string token = webApp.UserFixtures.CreateToken([]);

            //Act
            var userClient = new UserClient(string.Empty, httpClientFactory, token);
            var result = await userClient.CreateAsync(userPostDto, CancellationToken.None);

            //Assert
            result.Should().NotBeNull().And.BeOfType<ResponseDto<UserGetDto>>();
            result.Response!.Name.Should().Be(userPostDto.Name);
            result.Response.Email.Should().Be(userPostDto.Email);
        }

        [Fact]
        public async Task Update_WithData_ReturnsOk()
        {
            //Arrange
            const string newEmail = "newEmail@email.com";

            User user = webApp.UserFixtures.CreateUser().First();
            await webApp.UnitOfWork.GetUserRepository().InsertOneAsync(user);

            var userPutDto = fixture.Build<UserPutDto>()
                .With(x => x.Id, user.Id.ToString())
                .With(x => x.Email, newEmail)
                .Create();

            string token = webApp.UserFixtures.CreateToken([RoleType.Write]);

            //Act
            var userClient = new UserClient(string.Empty, httpClientFactory, token);
            var result = await userClient.UpdateAsync(userPutDto, CancellationToken.None);

            //Assert
            user.Name.Should().NotBe(userPutDto.Name);
            user.Email.Should().NotBe(userPutDto.Email);

            result.Should().NotBeNull().And.BeOfType<ResponseDto<UserGetDto>>();
            result.Response!.Name.Should().Be(userPutDto.Name);
            result.Response.Email.Should().Be(userPutDto.Email);
        }

        [Fact]
        public async Task Delete_WithData_ReturnsOk()
        {
            //Arrange
            User user = webApp.UserFixtures.CreateUser().First();
            await webApp.UnitOfWork.GetUserRepository().InsertOneAsync(user);
            string token = webApp.UserFixtures.CreateToken([RoleType.Write]);

            //Act
            var userClient = new UserClient(string.Empty, httpClientFactory, token);
            var result = await userClient.DeleteAsync(user.Id.ToString(), CancellationToken.None);

            //Assert
            result.Should().NotBeNull().And.BeOfType<ResponseDto<UserGetDto>>();
            result.Response!.Id.Should().Be(user.Id.ToString());
            result.Response.Name.Should().Be(user.Name);
            result.Response.Email.Should().Be(user.Email);
        }

    }
}
