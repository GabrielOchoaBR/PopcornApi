using Microsoft.AspNetCore.Mvc.Testing;

namespace IntegrationTests.Factories
{
    public class HttpClientFactory<TStartup>(WebApplicationFactory<TStartup> appFactory) : IHttpClientFactory
        where TStartup : class
    {
        private readonly WebApplicationFactory<TStartup> _appFactory = appFactory;
        public HttpClient CreateClient(string name) => _appFactory.CreateClient();
    }
}
