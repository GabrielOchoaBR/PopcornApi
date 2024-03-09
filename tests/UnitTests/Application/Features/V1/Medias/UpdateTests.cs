using Application.V1.Features.Medias;
using AutoFixture;
using AutoFixture.Xunit2;
using Domain.V1.Entities.Medias;
using FluentAssertions;
using Moq;

namespace UnitTests.Application.Features.V1.Medias
{
    public class UpdateTests : BaseRepositoryTests
    {
        private readonly Update.Handler handler;

        public UpdateTests() 
            => handler = new Update.Handler(unitOfWorkMock.Object);

        [Theory, AutoData]
        public async void Handler_NotFoundData_ReturnsNull(Update.Command command)
        {
            //Arrange
            mediaRepositoryMock.Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                               .ReturnsAsync((Media)null!);

            //Act
            var result = await handler.Handle(command, CancellationToken.None);

            //Assert
            result.Should().BeNull();
        }

        [Theory, AutoData]
        public async void Handler_CannotUpdateData_ReturnsNull(Update.Command command)
        {
            //Arrange
            var mediaDb = fixture.Create<Media>();

            mediaRepositoryMock.Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                               .ReturnsAsync(mediaDb);

            mediaRepositoryMock.Setup(x => x.ReplaceOneAsync(It.IsAny<Media>()))
                               .ReturnsAsync((Media)null!);

            //Act
            var result = await handler.Handle(command, CancellationToken.None);

            //Assert
            result.Should().BeNull();

        }

        [Theory, AutoData]
        public async void Handler_Data_ReturnsOk(Update.Command command)
        {
            //Arrange
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
