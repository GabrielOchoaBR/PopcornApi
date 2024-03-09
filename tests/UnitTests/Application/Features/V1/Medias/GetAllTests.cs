using Application.V1.Dtos.Medias;
using Application.V1.Dtos.Shared;
using Application.V1.Features.Medias;
using AutoFixture;
using AutoFixture.Xunit2;
using Domain.V1.Entities.Medias;
using FluentAssertions;
using Moq;

namespace UnitTests.Application.Features.V1.Medias
{
    public class GetAllTests : BaseRepositoryTests
    {
        private readonly GetAll.Handler handler;

        public GetAllTests() 
            => handler = new GetAll.Handler(unitOfWorkMock.Object);

        [Theory, AutoData]
        public async void Handler_WithRandomData_ReturnsOk(GetAll.Query query)
        {
            //Arrange
            var medias = fixture.CreateMany<Media>();
            long totalCount = medias.Count();

            mediaRepositoryMock.Setup(x => x.GetAllAsync(It.IsAny<string>(),
                                                    It.IsAny<string>(),
                                                    It.IsAny<string>(),
                                                    It.IsAny<int>(),
                                                    It.IsAny<int>()))
                                            .ReturnsAsync((medias, totalCount));

            //Act
            var result = await handler.Handle(query, CancellationToken.None);

            //Assert
            result.Should().NotBeNull().And.BeOfType<ResponseGetAll<MediaGetDto>>();
            result.TotalCount.Should().Be(totalCount);
            result.Result.Count().Should().Be(medias.Count());
        }
    }
}
