using Application.V1.Features.Medias;
using AutoFixture.Xunit2;
using Domain.V1.Entities.Medias;
using FluentAssertions;
using MongoDB.Bson;
using Moq;

namespace UnitTests.Application.Features.V1.Medias
{
    public class CreateTests : BaseRepositoryTests
    {
        private readonly Create.Handler handler;
        public CreateTests()
            => handler = new Create.Handler(unitOfWorkMock.Object, userDataControlMock.Object);

        [Theory, AutoData]
        public async void Handler_Data_ReturnsOk(Create.Command command)
        {
            //Arrange
            var mediaGetDto = new Media();

            mediaRepositoryMock.Setup(x => x.InsertOneAsync(It.IsAny<Media>()))
                        .Callback<Media>(param =>
                        {
                            mediaGetDto = param;
                            mediaGetDto.Id = ObjectId.GenerateNewId(DateTime.UtcNow);
                            mediaGetDto.CreatedBy = ObjectId.GenerateNewId();
                        });

            //Act
            var result = await handler.Handle(command, CancellationToken.None);

            //Assert
            result.Id.Should().Be(mediaGetDto!.Id.ToString());
            result.Title.Should().Be(mediaGetDto!.Title);
            result.CreatedAt.Should().Be(mediaGetDto!.CreatedAt);
            result.CreatedBy.Should().Be(mediaGetDto!.CreatedBy.ToString());
        }
    }
}
