using AutoFixture;
using IntegrationTests.Factories;
using MongoDB.Bson;

namespace IntegrationTests.PopcornClient
{
    public class BaseTests
    {
        internal readonly WebAppFactory webApp;
        internal readonly HttpClientFactory<Program> httpClientFactory;
        internal readonly Fixture fixture = new();

        public BaseTests()
        {
            webApp = new();
            httpClientFactory = new(webApp);

            fixture.Register(ObjectId.GenerateNewId);
        }

    }
}
