using System.Collections.Specialized;
using Application.V1.Dtos.Medias;
using Application.V1.Dtos.Shared;
using PopcornClient.Model;

namespace PopcornClient.V1
{
    public class MediaClient : BaseClient
    {
        public MediaClient(string endpoint, IHttpClientFactory httpClientFactory) : base(endpoint, httpClientFactory)
        { }
        public MediaClient(string endpoint, IHttpClientFactory httpClientFactory, string authenticationToken) : base(endpoint, httpClientFactory, authenticationToken)
        { }

        private const string mediaPath = "/api/v1/Media";

        public async Task<ResponseDto<ResponseGetAll<MediaGetDto>>> GetAllAsync(QueryGetAll queryGetAll, CancellationToken cancellationToken)
        {
            NameValueCollection queryString = System.Web.HttpUtility.ParseQueryString(string.Empty);

            if (!string.IsNullOrEmpty(queryGetAll.SearchTerm))
                queryString.Add(nameof(queryGetAll.SearchTerm), queryGetAll.SearchTerm);
            if (!string.IsNullOrEmpty(queryGetAll.SortColumn))
                queryString.Add(nameof(queryGetAll.SortColumn), queryGetAll.SortColumn);
            if (!string.IsNullOrEmpty(queryGetAll.SortDirection))
                queryString.Add(nameof(queryGetAll.SortDirection), queryGetAll.SortDirection);
            if (queryGetAll.PageIndex != 0)
                queryString.Add(nameof(queryGetAll.PageIndex), queryGetAll.PageIndex.ToString());
            if (queryGetAll.PageSize != 0)
                queryString.Add(nameof(queryGetAll.PageSize), queryGetAll.PageSize.ToString());

            var mediaPathResult = string.Empty;

            if (queryString.Count > 0)
                mediaPathResult = BuildUri($"{mediaPath}?{queryString}");
            else
                mediaPathResult = BuildUri(mediaPath);

            return await GetAsync<ResponseGetAll<MediaGetDto>>(mediaPathResult, cancellationToken);
        }

        public async Task<ResponseDto<MediaGetDto>> GetByIdAsync(string id, CancellationToken cancellationToken)
            => await GetAsync<MediaGetDto>(BuildUri($"{mediaPath}/{id}"), cancellationToken);

        public async Task<ResponseDto<MediaGetDto>> CreateAsync(MediaPostDto MediaPostDto, CancellationToken cancellationToken)
            => await PostAsync<MediaGetDto, MediaPostDto>(BuildUri(mediaPath), MediaPostDto, cancellationToken);

        public async Task<ResponseDto<MediaGetDto>> UpdateAsync(MediaPutDto MediaPutDto, CancellationToken cancellationToken)
            => await PutAsync<MediaGetDto, MediaPutDto>(BuildUri(mediaPath), MediaPutDto, cancellationToken);

        public async Task<ResponseDto<MediaGetDto>> DeleteAsync(string id, CancellationToken cancellationToken)
            => await DeleteAsync<MediaGetDto>(BuildUri($"{mediaPath}/{id}"), cancellationToken);
    }
}
