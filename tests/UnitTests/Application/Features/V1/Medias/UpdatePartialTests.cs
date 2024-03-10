using Application.V1.Dtos.Medias;
using Application.V1.Features.Medias;
using AutoFixture;
using Domain.V1.Entities.Medias;
using FluentAssertions;
using Microsoft.AspNetCore.JsonPatch;
using Moq;

namespace UnitTests.Application.Features.V1.Medias
{
    public class UpdatePartialTests : BaseRepositoryTests
    {
        private readonly UpdatePartial.Handler handler;

        public UpdatePartialTests() 
            => handler = new UpdatePartial.Handler(unitOfWorkMock.Object, userDataControlMock.Object);

        [Fact]
        public async void Handler_NotFoundData_ReturnsNull()
        {
            //Arrange
            var command = new UpdatePartial.Command() { Id = string.Empty, JsonPatchDocument = new JsonPatchDocument<MediaGetDto>() };

            mediaRepositoryMock.Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                               .ReturnsAsync((Media)null!);

            //Act
            var result = await handler.Handle(command, CancellationToken.None);

            //Assert
            result.Should().BeNull();
        }

        [Fact]
        public async void Handler_CannotUpdateData_ReturnsNull()
        {
            //Arrange
            var command = new UpdatePartial.Command() { Id = string.Empty, JsonPatchDocument = new JsonPatchDocument<MediaGetDto>() };

            var mediaDb = fixture.Build<Media>().Create();

            mediaRepositoryMock.Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                               .ReturnsAsync(mediaDb);

            mediaRepositoryMock.Setup(x => x.ReplaceOneAsync(It.IsAny<Media>()))
                               .ReturnsAsync((Media)null!);

            //Act
            var result = await handler.Handle(command, CancellationToken.None);

            //Assert
            result.Should().BeNull();

        }

        [Fact]
        public async void Handler_Data_ReturnsOk()
        {
            //Arrange
            var command = new UpdatePartial.Command() { Id = string.Empty, JsonPatchDocument = new JsonPatchDocument<MediaGetDto>() };

            var mediaDb = fixture.Create<Media>();
            var mediaUpdated = new Media();

            mediaRepositoryMock.Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                               .ReturnsAsync(mediaDb);

            mediaRepositoryMock.Setup(x => x.ReplaceOneAsync(It.IsAny<Media>()))
                               .Callback<Media>(param =>
                               {
                                   mediaUpdated = param;
                                   mediaUpdated.UpdatedAt = DateTime.UtcNow;
                               })
                               .ReturnsAsync(mediaUpdated);

            //Act
            var result = await handler.Handle(command, CancellationToken.None);

            //Assert
            result!.Id.Should().Be(mediaUpdated.Id.ToString());
            result!.UpdatedAt.Should().Be(mediaUpdated.UpdatedAt);
        }
    }
}
