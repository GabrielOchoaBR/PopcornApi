using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using PopcornApi.Controllers.V1;
using PopcornApi.Model.Settings;
using PopcornApi.Security.TokenServices;

namespace UnitTests.PopcornApi.Controllers
{
    public class BaseControllerTests: BaseTests
    {
        protected readonly Mock<IMediator> mediatorMock = new();
        protected readonly Mock<IAppSettings> appSettingsMock = new();
        protected readonly Mock<ILogger<LoginController>> loggerMock = new();
        protected readonly Mock<ITokenService> tokenServiceMock = new();
    }
}
