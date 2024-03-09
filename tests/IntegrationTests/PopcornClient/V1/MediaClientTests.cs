using Application.V1.Dtos.Medias;
using Application.V1.Dtos.Shared;
using AutoFixture;
using Domain.V1.Entities.Medias;
using Domain.V1.Entities.Users;
using FluentAssertions;
using MongoDB.Bson;
using PopcornClient.Model;
using PopcornClient.V1;

namespace IntegrationTests.PopcornClient.V1
{
    public class MediaClientTests : BaseTests
    {

        [Fact]
        public async Task GetAll_WithData_ReturnsOk()
        {
            //Arrange
            IEnumerable<Media> medias = webApp.MediasFixtures.CreateMedia(10);
            foreach (var media in medias)
                await webApp.UnitOfWork.GetMediaRepository().InsertOneAsync(media);

            string token = webApp.UsersFixtures.CreateToken([RoleType.Read]);
            var queryGetAll = new QueryGetAll()
            {
                SearchTerm = "title",
                PageIndex = 1,
                PageSize = 10,
                SortColumn = "Title",
                SortDirection = "ASC"
            };

            //Act
            var mediaClient = new MediaClient(string.Empty, httpClientFactory, token);
            var result = await mediaClient.GetAllAsync(queryGetAll, CancellationToken.None);

            //Assert
            result.Should().NotBeNull().And.BeOfType<ResponseDto<ResponseGetAll<MediaGetDto>>>();
            result.IsSuccess.Should().BeTrue();
            result.Response!.Result.Should().HaveCount(10);
        }

        [Fact]
        public async Task GetById_WrongData_ReturnsOk()
        {
            //Arrange
            Media media = webApp.MediasFixtures.CreateMedia().First();
            await webApp.UnitOfWork.GetMediaRepository().InsertOneAsync(media);
            string token = webApp.UsersFixtures.CreateToken([RoleType.Read]);

            //Act
            var mediaClient = new MediaClient(string.Empty, httpClientFactory, token);
            var result = await mediaClient.GetByIdAsync(ObjectId.GenerateNewId().ToString(), CancellationToken.None);

            //Assert
            result.Should().NotBeNull().And.BeOfType<ResponseDto<MediaGetDto>>();
            result.IsSuccess.Should().BeFalse();
            result.FailedResult.Should().NotBeNull();
            result.FailedResult!.StatusCode.Should().Be(404);
        }

        [Fact]
        public async Task GetById_WithData_ReturnsOk()
        {
            //Arrange
            Media media = webApp.MediasFixtures.CreateMedia().First();
            await webApp.UnitOfWork.GetMediaRepository().InsertOneAsync(media);
            string token = webApp.UsersFixtures.CreateToken([RoleType.Read]);

            //Act
            var mediaClient = new MediaClient(string.Empty, httpClientFactory, token);
            var result = await mediaClient.GetByIdAsync(media.Id.ToString(), CancellationToken.None);

            //Assert
            result.Should().NotBeNull().And.BeOfType<ResponseDto<MediaGetDto>>();
            result.IsSuccess.Should().BeTrue();
            result.Response!.Id.Should().Be(media.Id.ToString());
            result.Response.Title.Should().Be(media.Title);
        }

        [Fact]
        public async Task Create_WithData_ReturnsOk()
        {
            //Arrange
            var mediaPostDto = fixture.Create<MediaPostDto>();
            string token = webApp.UsersFixtures.CreateToken([RoleType.Write]);

            //Act
            var mediaClient = new MediaClient(string.Empty, httpClientFactory, token);
            var result = await mediaClient.CreateAsync(mediaPostDto, CancellationToken.None);

            //Assert
            result.Should().NotBeNull().And.BeOfType<ResponseDto<MediaGetDto>>();
            result.Response!.Title.Should().Be(mediaPostDto.Title);
        }

        [Fact]
        public async Task Update_WithData_ReturnsOk()
        {
            //Arrange
            Media media = webApp.MediasFixtures.CreateMedia().First();
            await webApp.UnitOfWork.GetMediaRepository().InsertOneAsync(media);

            var mediaPutDto = fixture.Build<MediaPutDto>()
                .With(x => x.Id, media.Id.ToString())
                .Create();

            string token = webApp.UsersFixtures.CreateToken([RoleType.Write]);

            //Act
            var mediaClient = new MediaClient(string.Empty, httpClientFactory, token);
            var result = await mediaClient.UpdateAsync(mediaPutDto, CancellationToken.None);

            //Assert
            media.Title.Should().NotBe(mediaPutDto.Title);

            result.Should().NotBeNull().And.BeOfType<ResponseDto<MediaGetDto>>();
            result.Response!.Title.Should().Be(mediaPutDto.Title);
        }

        [Fact]
        public async Task Delete_WithData_ReturnsOk()
        {
            //Arrange
            Media media = webApp.MediasFixtures.CreateMedia().First();
            await webApp.UnitOfWork.GetMediaRepository().InsertOneAsync(media);
            string token = webApp.UsersFixtures.CreateToken([RoleType.Write]);

            //Act
            var mediaClient = new MediaClient(string.Empty, httpClientFactory, token);
            var result = await mediaClient.DeleteAsync(media.Id.ToString(), CancellationToken.None);

            //Assert
            result.Should().NotBeNull().And.BeOfType<ResponseDto<MediaGetDto>>();
            result.Response!.Id.Should().Be(media.Id.ToString());
            result.Response.Title.Should().Be(media.Title);
        }

    }
}
