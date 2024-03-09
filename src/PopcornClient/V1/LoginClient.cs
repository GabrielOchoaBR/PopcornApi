using Amazon.Runtime;
using Application.V1.Dtos.Admin.Users;
using PopcornApi.Model.Login;
using PopcornClient.Model;

namespace PopcornClient.V1
{
    public class LoginClient : BaseClient
    {
        public LoginClient(string endpoint, IHttpClientFactory httpClientFactory) : base(endpoint, httpClientFactory)
        { }
        public LoginClient(string endpoint, IHttpClientFactory httpClientFactory, string authenticationToken) : base(endpoint, httpClientFactory, authenticationToken)
        { }

        private const string loginPath = "/api/v1/login";

        public async Task<ResponseDto<AuthenticationDto>> GetByNameAndPassword(UserGetByNameAndPasswordDto userGetByNameAndPasswordDto, CancellationToken cancellationToken)
            => await PostAsync<AuthenticationDto, UserGetByNameAndPasswordDto>(BuildUri($"{loginPath}/Login"), userGetByNameAndPasswordDto, cancellationToken);

        public async Task<ResponseDto<AuthenticationDto>> RefreshToken(AuthenticationDto authenticationDto, CancellationToken cancellationToken)
            => await PostAsync<AuthenticationDto, AuthenticationDto>(BuildUri($"{loginPath}/RefreshToken"), authenticationDto, cancellationToken);

        public async Task<ResponseDto<UserGetDto>> Revoke(string id, CancellationToken cancellationToken)
            => await PostAsync<UserGetDto, object>(BuildUri($"{loginPath}/Revoke/{id}"), null!, cancellationToken);

        public async Task<ResponseDto<UserGetDto>> UpdatePassword(UserPostUpdatePasswordDto userPostUpdatePasswordDto, CancellationToken cancellationToken)
            => await PostAsync<UserGetDto, UserPostUpdatePasswordDto>(BuildUri($"{loginPath}/UpdatePassword"), userPostUpdatePasswordDto, cancellationToken);
    }
}
