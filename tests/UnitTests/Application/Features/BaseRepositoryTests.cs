using Infrastructure.Repositories.Medias;
using Infrastructure.Repositories.Users;
using Infrastructure.UnitOfWork;
using Moq;

namespace UnitTests.Application.Features
{
    public class BaseRepositoryTests : BaseTests
    {
        protected readonly Mock<IUnitOfWork> unitOfWorkMock = new();
        protected readonly Mock<IUserRepository> userRepositoryMock = new();
        protected readonly Mock<IMediaRepository> mediaRepositoryMock = new();

        public BaseRepositoryTests()
        {
            unitOfWorkMock.Setup(x => x.GetUserRepository()).Returns(userRepositoryMock.Object);
            unitOfWorkMock.Setup(x => x.GetMediaRepository()).Returns(mediaRepositoryMock.Object);
        }
    }
}
