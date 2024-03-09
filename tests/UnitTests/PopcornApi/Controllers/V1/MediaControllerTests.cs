using Application.V1.Dtos.Medias;
using Application.V1.Dtos.Shared;
using Application.V1.Features.Medias;
using AutoFixture;
using AutoFixture.Xunit2;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PopcornApi.Controllers.V1;

namespace UnitTests.PopcornApi.Controllers.V1
{
    public class MediaControllerTests : BaseControllerTests
    {
        private readonly MediaController mediaController;

        public MediaControllerTests()
        {
            mediaController = new(mediatorMock.Object);
        }

        [Theory, AutoData]
        public async void GetAll_WithData_Returns200(QueryGetAll queryGetAll)
        {
            //Arrange
            var mediaGetDtos = fixture.Create<ResponseGetAll<MediaGetDto>>();
            mediatorMock.Setup(x => x.Send(It.IsAny<GetAll.Query>(), It.IsAny<CancellationToken>())).ReturnsAsync(mediaGetDtos);

            //Act
            var getAll = await mediaController.GetAll(queryGetAll);

            //Assert
            getAll.Should().NotBeNull();
            getAll.Result.Should().BeOfType<OkObjectResult>().Which.StatusCode.Should().Be(StatusCodes.Status200OK);

            OkObjectResult result = (OkObjectResult)getAll.Result!;
            ((ResponseGetAll<MediaGetDto>)result.Value!).Result.Count().Should().Be(mediaGetDtos.Result.Count());
            ((ResponseGetAll<MediaGetDto>)result.Value!).TotalCount.Should().Be(mediaGetDtos.TotalCount);
        }

        [Theory, AutoData]
        public async void GetById_WrongData_Returns404(string id)
        {
            //Arrange
            mediatorMock.Setup(x => x.Send(It.IsAny<GetById.Query>(), It.IsAny<CancellationToken>())).ReturnsAsync((MediaGetDto)null!);

            //Act
            var getById = await mediaController.GetById(id);

            //Assert
            getById.Should().NotBeNull();
            getById.Result.Should().BeOfType<NotFoundResult>().Which.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }

        [Theory, AutoData]
        public async void GetById_WithData_Returns200(string id)
        {
            //Arrange
            fixture.Customize<MediaGetDto>(c => c.With(x => x.Id, id));
            var mediaGetDto = fixture.Create<MediaGetDto>();
            mediatorMock.Setup(x => x.Send(It.IsAny<GetById.Query>(), It.IsAny<CancellationToken>())).ReturnsAsync(mediaGetDto);

            //Act
            var getById = await mediaController.GetById(id);

            //Assert
            getById.Should().NotBeNull();
            getById.Result.Should().BeOfType<OkObjectResult>().Which.StatusCode.Should().Be(StatusCodes.Status200OK);

            OkObjectResult result = (OkObjectResult)getById.Result!;
            ((MediaGetDto)result.Value!).Id.Should().Be(mediaGetDto.Id);
        }
        [Theory, AutoData]
        public async void Post_WrongData_Returns400(MediaPostDto mediaPostDto)
        {
            //Arrange
            mediatorMock.Setup(x => x.Send(It.IsAny<Create.Command>(), It.IsAny<CancellationToken>())).ReturnsAsync((MediaGetDto)null!);

            //Act
            var getById = await mediaController.Post(mediaPostDto);

            //Assert
            getById.Should().NotBeNull();
            getById.Result.Should().BeOfType<BadRequestResult>().Which.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }

        [Theory, AutoData]
        public async void Post_WithData_Returns200(MediaPostDto mediaPostDto)
        {
            //Arrange
            fixture.Customize<MediaGetDto>(c => c.With(x => x.Title, mediaPostDto.Title));
            var mediaGetDto = fixture.Create<MediaGetDto>();
            mediatorMock.Setup(x => x.Send(It.IsAny<Create.Command>(), It.IsAny<CancellationToken>())).ReturnsAsync(mediaGetDto);

            //Act
            var getById = await mediaController.Post(mediaPostDto);

            //Assert
            getById.Should().NotBeNull();
            getById.Result.Should().BeOfType<OkObjectResult>().Which.StatusCode.Should().Be(StatusCodes.Status200OK);

            OkObjectResult result = (OkObjectResult)getById.Result!;
            ((MediaGetDto)result.Value!).Title.Should().Be(mediaGetDto.Title);
        }

        [Theory, AutoData]
        public async void Put_WrongData_Returns404(MediaPutDto mediaPutDto)
        {
            //Arrange
            mediatorMock.Setup(x => x.Send(It.IsAny<Update.Command>(), It.IsAny<CancellationToken>())).ReturnsAsync((MediaGetDto)null!);

            //Act
            var getById = await mediaController.Put(mediaPutDto);

            //Assert
            getById.Should().NotBeNull();
            getById.Result.Should().BeOfType<NotFoundResult>().Which.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }

        [Theory, AutoData]
        public async void Put_WithData_Returns200(MediaPutDto mediaPutDto)
        {
            //Arrange
            fixture.Customize<MediaGetDto>(c => c.With(x => x.Id, mediaPutDto.Id));
            var mediaGetDto = fixture.Create<MediaGetDto>();
            mediatorMock.Setup(x => x.Send(It.IsAny<Update.Command>(), It.IsAny<CancellationToken>())).ReturnsAsync(mediaGetDto);

            //Act
            var getById = await mediaController.Put(mediaPutDto);

            //Assert
            getById.Should().NotBeNull();
            getById.Result.Should().BeOfType<OkObjectResult>().Which.StatusCode.Should().Be(StatusCodes.Status200OK);

            OkObjectResult result = (OkObjectResult)getById.Result!;
            ((MediaGetDto)result.Value!).Id.Should().Be(mediaGetDto.Id);
        }

        [Theory, AutoData]
        public async void Patch_WithData_Returns200(string id)
        {
            //Arrange
            var jsonPatchDocument = new JsonPatchDocument<MediaGetDto>();

            fixture.Customize<MediaGetDto>(c => c.With(x => x.Id, id));
            var mediaGetDto = fixture.Create<MediaGetDto>();
            mediatorMock.Setup(x => x.Send(It.IsAny<UpdatePartial.Command>(), It.IsAny<CancellationToken>())).ReturnsAsync(mediaGetDto);

            ////Act
            var patch = await mediaController.Patch(id, jsonPatchDocument);

            ////Assert
            patch.Should().NotBeNull();
            patch.Result.Should().BeOfType<OkObjectResult>().Which.StatusCode.Should().Be(StatusCodes.Status200OK);

            OkObjectResult result = (OkObjectResult)patch.Result!;
            ((MediaGetDto)result.Value!).Id.Should().Be(id);
        }

        [Theory, AutoData]
        public async void Delete_WrongData_Returns404(string id)
        {
            //Arrange
            mediatorMock.Setup(x => x.Send(It.IsAny<Delete.Command>(), It.IsAny<CancellationToken>())).ReturnsAsync((MediaGetDto)null!);

            //Act
            var delete = await mediaController.Delete(id);

            //Assert
            delete.Should().NotBeNull();
            delete.Result.Should().BeOfType<NotFoundResult>().Which.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }

        [Theory, AutoData]
        public async void Delete_WithData_Returns200(string id)
        {
            //Arrange
            fixture.Customize<MediaGetDto>(c => c.With(x => x.Id, id));
            var mediaGetDto = fixture.Create<MediaGetDto>();
            mediatorMock.Setup(x => x.Send(It.IsAny<Delete.Command>(), It.IsAny<CancellationToken>())).ReturnsAsync(mediaGetDto);

            //Act
            var delete = await mediaController.Delete(id);

            //Assert
            delete.Should().NotBeNull();
            delete.Result.Should().BeOfType<OkObjectResult>().Which.StatusCode.Should().Be(StatusCodes.Status200OK);

            OkObjectResult result = (OkObjectResult)delete.Result!;
            ((MediaGetDto)result.Value!).Id.Should().Be(mediaGetDto.Id);
        }
    }
}
