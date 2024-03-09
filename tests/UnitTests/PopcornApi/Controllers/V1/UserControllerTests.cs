using Application.V1.Dtos.Admin.Users;
using Application.V1.Features.Users;
using AutoFixture;
using AutoFixture.Xunit2;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PopcornApi.Controllers.V1;

namespace UnitTests.PopcornApi.Controllers.V1
{
    public class UserControllerTests : BaseControllerTests
    {
        private readonly UserController userController;

        public UserControllerTests()
        {
            userController = new(mediatorMock.Object);
        }

        [Fact]
        public async void GetAll_WithData_Returns200()
        {
            //Arrange
            var userGetDtos = fixture.CreateMany<UserGetDto>();
            mediatorMock.Setup(x => x.Send(It.IsAny<GetAll.Query>(), It.IsAny<CancellationToken>())).ReturnsAsync(userGetDtos);

            //Act
            var getAll = await userController.GetAll();

            //Assert
            getAll.Should().NotBeNull();
            getAll.Result.Should().BeOfType<OkObjectResult>().Which.StatusCode.Should().Be(StatusCodes.Status200OK);

            OkObjectResult result = (OkObjectResult)getAll.Result!;
            ((IEnumerable<UserGetDto>)result.Value!).Count().Should().Be(userGetDtos.Count());
        }

        [Theory, AutoData]
        public async void GetById_WrongData_Returns404(string id)
        {
            //Arrange
            mediatorMock.Setup(x => x.Send(It.IsAny<GetById.Query>(), It.IsAny<CancellationToken>())).ReturnsAsync((UserGetDto)null!);

            //Act
            var getById = await userController.GetById(id);

            //Assert
            getById.Should().NotBeNull();
            getById.Result.Should().BeOfType<NotFoundResult>().Which.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }

        [Theory, AutoData]
        public async void GetById_WithData_Returns200(string id)
        {
            //Arrange
            fixture.Customize<UserGetDto>(c => c.With(x => x.Id, id));
            var userGetDto = fixture.Create<UserGetDto>();
            mediatorMock.Setup(x => x.Send(It.IsAny<GetById.Query>(), It.IsAny<CancellationToken>())).ReturnsAsync(userGetDto);

            //Act
            var getById = await userController.GetById(id);

            //Assert
            getById.Should().NotBeNull();
            getById.Result.Should().BeOfType<OkObjectResult>().Which.StatusCode.Should().Be(StatusCodes.Status200OK);

            OkObjectResult result = (OkObjectResult)getById.Result!;
            ((UserGetDto)result.Value!).Id.Should().Be(userGetDto.Id);
        }

        [Theory, AutoData]
        public async void Post_WrongData_Returns400(UserPostDto userPostDto)
        {
            //Arrange
            mediatorMock.Setup(x => x.Send(It.IsAny<Create.Command>(), It.IsAny<CancellationToken>())).ReturnsAsync((UserGetDto)null!);

            //Act
            var getById = await userController.Post(userPostDto);

            //Assert
            getById.Should().NotBeNull();
            getById.Result.Should().BeOfType<BadRequestResult>().Which.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }

        [Theory, AutoData]
        public async void Post_WithData_Returns200(UserPostDto userPostDto)
        {
            //Arrange
            fixture.Customize<UserGetDto>(c => c.With(x => x.Name, userPostDto.Name));
            var userGetDto = fixture.Create<UserGetDto>();
            mediatorMock.Setup(x => x.Send(It.IsAny<Create.Command>(), It.IsAny<CancellationToken>())).ReturnsAsync(userGetDto);

            //Act
            var getById = await userController.Post(userPostDto);

            //Assert
            getById.Should().NotBeNull();
            getById.Result.Should().BeOfType<OkObjectResult>().Which.StatusCode.Should().Be(StatusCodes.Status200OK);

            OkObjectResult result = (OkObjectResult)getById.Result!;
            ((UserGetDto)result.Value!).Name.Should().Be(userGetDto.Name);
        }

        [Theory, AutoData]
        public async void Put_WrongData_Returns404(UserPutDto userPutDto)
        {
            //Arrange
            mediatorMock.Setup(x => x.Send(It.IsAny<Update.Command>(), It.IsAny<CancellationToken>())).ReturnsAsync((UserGetDto)null!);

            //Act
            var getById = await userController.Put(userPutDto);

            //Assert
            getById.Should().NotBeNull();
            getById.Result.Should().BeOfType<NotFoundResult>().Which.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }

        [Theory, AutoData]
        public async void Put_WithData_Returns200(UserPutDto userPutDto)
        {
            //Arrange
            fixture.Customize<UserGetDto>(c => c.With(x => x.Id, userPutDto.Id));
            var userGetDto = fixture.Create<UserGetDto>();
            mediatorMock.Setup(x => x.Send(It.IsAny<Update.Command>(), It.IsAny<CancellationToken>())).ReturnsAsync(userGetDto);

            //Act
            var getById = await userController.Put(userPutDto);

            //Assert
            getById.Should().NotBeNull();
            getById.Result.Should().BeOfType<OkObjectResult>().Which.StatusCode.Should().Be(StatusCodes.Status200OK);

            OkObjectResult result = (OkObjectResult)getById.Result!;
            ((UserGetDto)result.Value!).Id.Should().Be(userGetDto.Id);
        }

        [Theory, AutoData]
        public async void Delete_WrongData_Returns404(string id)
        {
            //Arrange
            mediatorMock.Setup(x => x.Send(It.IsAny<Delete.Command>(), It.IsAny<CancellationToken>())).ReturnsAsync((UserGetDto)null!);

            //Act
            var delete = await userController.Delete(id);

            //Assert
            delete.Should().NotBeNull();
            delete.Result.Should().BeOfType<NotFoundResult>().Which.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }

        [Theory, AutoData]
        public async void Delete_WithData_Returns200(string id)
        {
            //Arrange
            fixture.Customize<UserGetDto>(c => c.With(x => x.Id, id));
            var userGetDto = fixture.Create<UserGetDto>();
            mediatorMock.Setup(x => x.Send(It.IsAny<Delete.Command>(), It.IsAny<CancellationToken>())).ReturnsAsync(userGetDto);

            //Act
            var delete = await userController.Delete(id);

            //Assert
            delete.Should().NotBeNull();
            delete.Result.Should().BeOfType<OkObjectResult>().Which.StatusCode.Should().Be(StatusCodes.Status200OK);

            OkObjectResult result = (OkObjectResult)delete.Result!;
            ((UserGetDto)result.Value!).Id.Should().Be(userGetDto.Id);
        }
    }
}
