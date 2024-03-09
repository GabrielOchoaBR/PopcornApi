using Application.V1.Dtos.Medias;
using Application.V1.Features.Medias;
using AutoFixture;
using AutoFixture.Xunit2;
using Domain.V1.Entities.Medias;
using FluentAssertions;
using Moq;

namespace UnitTests.Application.Features.V1.Medias
{
    public class GetByIdTests : BaseRepositoryTests
    {
        private readonly GetById.Handler handler;

        public GetByIdTests()
            => handler = new GetById.Handler(unitOfWorkMock.Object);


        [Theory, AutoData]
        public async void Handler_WithData_ReturnsOk(GetById.Query query)
        {
            //Arrange
            var Media = fixture.Create<Media>();
            mediaRepositoryMock.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(Media);

            //Act
            var result = await handler.Handle(query, CancellationToken.None);

            //Assert
            result.Should().NotBeNull().And.BeOfType<MediaGetDto>();
            result!.Id.Should().Be(Media.Id.ToString());
            result!.Title.Should().Be(Media.Title);
            result!.CreatedAt.Should().Be(Media.CreatedAt);
            result!.CreatedBy.Should().Be(Media.CreatedBy.ToString());
            result!.UpdatedAt.Should().Be(Media.UpdatedAt);
            result!.UpdatedBy.Should().Be(Media.UpdatedBy.ToString());
        }

        [Theory, AutoData]
        public async void Handler_WitNoData_ReturnFails(GetById.Query query)
        {
            //Arrange
            mediaRepositoryMock.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync((Domain.V1.Entities.Medias.Media)null!);

            //Act
            var result = await handler.Handle(query, CancellationToken.None);

            //Assert
            result.Should().BeNull();
        }
    }
}