using Application.V1.Dtos.Admin.Users;
using Microsoft.Extensions.Hosting;
using PopcornClient.Model;

namespace PopcornClient.V1
{
    public class UserClient : BaseClient
    {
        public UserClient(string endpoint, IHttpClientFactory httpClientFactory) : base(endpoint, httpClientFactory)
        { }
        public UserClient(string endpoint, IHttpClientFactory httpClientFactory, string authenticationToken) : base(endpoint, httpClientFactory, authenticationToken)
        { }

        private const string userPath = "/api/v1/user";

        public async Task<ResponseDto<IEnumerable<UserGetDto>>> GetAllAsync(CancellationToken cancellationToken)
            => await GetAsync<IEnumerable<UserGetDto>>(BuildUri(userPath), cancellationToken);

        public async Task<ResponseDto<UserGetDto>> GetByIdAsync(string id, CancellationToken cancellationToken)
            => await GetAsync<UserGetDto>(BuildUri($"{userPath}/{id}"), cancellationToken);

        public async Task<ResponseDto<UserGetDto>> CreateAsync(UserPostDto userPostDto, CancellationToken cancellationToken)
            => await PostAsync<UserGetDto, UserPostDto>(BuildUri(userPath), userPostDto, cancellationToken);

        public async Task<ResponseDto<UserGetDto>> UpdateAsync(UserPutDto userPutDto, CancellationToken cancellationToken)
            => await PutAsync<UserGetDto, UserPutDto>(BuildUri(userPath), userPutDto, cancellationToken);

        public async Task<ResponseDto<UserGetDto>> DeleteAsync(string id, CancellationToken cancellationToken)
            => await DeleteAsync<UserGetDto>(BuildUri($"{userPath}/{id}"), cancellationToken);
    }
}
