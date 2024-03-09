using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.JsonPatch;
using PopcornApi.Model.WebApi;
using PopcornClient.Model;

namespace PopcornClient
{
    public abstract class BaseClient
    {
        protected readonly string endpoint;
        protected readonly IHttpClientFactory httpClientFactory;
        protected readonly JsonSerializerOptions jsonSerializerOptions;
        private readonly string authenticationToken;
        protected BaseClient(string endpoint, IHttpClientFactory httpClient) : this(endpoint, httpClient, string.Empty)
        { }
        protected BaseClient(string endpoint, IHttpClientFactory httpClientFactory, string authenticationToken)
        {
            this.jsonSerializerOptions = new() { PropertyNameCaseInsensitive = true };
            this.endpoint = endpoint;
            this.httpClientFactory = httpClientFactory;
            this.authenticationToken = authenticationToken;
        }

        protected async Task<ResponseDto<TDocument>> GetAsync<TDocument>(string uri, CancellationToken cancellationToken)
            where TDocument : class
        {
            var httpClient = CreateHttpClient();

            using var response = await httpClient.GetAsync(uri, cancellationToken);

            return await ParseResponseAsync<TDocument>(response, cancellationToken);
        }

        protected async Task<ResponseDto<TDocument>> PostAsync<TDocument, TParam>(string url, TParam param, CancellationToken cancellationToken)
            where TDocument : class
            where TParam : class
        {
            StringContent? content = SerializeParam(param);

            if (param is null)
                content = null;

            var httpClient = CreateHttpClient();

            using var response = await httpClient.PostAsync(url, content, cancellationToken);

            return await ParseResponseAsync<TDocument>(response, cancellationToken);
        }

        protected async Task<ResponseDto<TDocument>> PutAsync<TDocument, TParam>(string url, TParam param, CancellationToken cancellationToken)
            where TDocument : class
            where TParam : class
        {
            StringContent? content = SerializeParam(param);

            var httpClient = CreateHttpClient();

            using var response = await httpClient.PutAsync(url, content, cancellationToken);

            return await ParseResponseAsync<TDocument>(response, cancellationToken);
        }

        protected async Task<ResponseDto<TDocument>> PatchAsync<TDocument, TParam>(string url, JsonPatchDocument<TParam> param, CancellationToken cancellationToken)
            where TDocument : class
            where TParam : class
        {
            StringContent? content = SerializeParam(param);

            var httpClient = CreateHttpClient();

            using var response = await httpClient.PatchAsync(url, content, cancellationToken);

            return await ParseResponseAsync<TDocument>(response, cancellationToken);
        }


        protected async Task<ResponseDto<TDocument>> DeleteAsync<TDocument>(string url, CancellationToken cancellationToken)
            where TDocument : class
        {
            var httpClient = CreateHttpClient();

            using var response = await httpClient.DeleteAsync(url, cancellationToken);

            return await ParseResponseAsync<TDocument>(response, cancellationToken);
        }

        protected string BuildUri(string basePath) => new($"{this.endpoint}{basePath}");

        private HttpClient CreateHttpClient()
        {
            var httpClient = httpClientFactory.CreateClient();
            if (authenticationToken is not null)
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authenticationToken);
            return httpClient;
        }

        private async Task<ResponseDto<TDocument>> ParseResponseAsync<TDocument>(HttpResponseMessage response, CancellationToken cancellationToken)
            where TDocument : class
        {
            var apiContent = await response.Content.ReadAsStreamAsync(cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                var exceptionResponse = await JsonSerializer.DeserializeAsync<ExceptionResponse>(apiContent, jsonSerializerOptions, cancellationToken);
                return new ResponseDto<TDocument>(response.IsSuccessStatusCode, null, new FailedResult(response.ReasonPhrase!, (int)response.StatusCode, exceptionResponse!));
            }

            var contentResponse = await JsonSerializer.DeserializeAsync<TDocument>(apiContent, jsonSerializerOptions, cancellationToken);
            return new ResponseDto<TDocument>(response.IsSuccessStatusCode, contentResponse, null);
        }
        private static StringContent SerializeParam<TParam>(TParam param)
            where TParam : class
            => new StringContent(JsonSerializer.Serialize(param), Encoding.UTF8, System.Net.Mime.MediaTypeNames.Application.Json);

    }
}
