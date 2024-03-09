using Application.Mappers;
using Application.V1.Dtos.Admin.Users;
using Domain.V1.Entities.Users;
using Infrastructure.UnitOfWork;
using MediatR;

namespace Application.V1.Features.Users
{
    public static class UpdateRefreshToken
    {
        public sealed class Command : IRequest<UserGetDto?>
        {
            public required UserPostRefreshTokenDto UserPostRefreshTokenDto { get; set; }
        }

        public sealed class Handler(IUnitOfWork unitOfWork) : IRequestHandler<Command, UserGetDto?>
        {
            private readonly IUnitOfWork unitOfWork = unitOfWork;

            public async Task<UserGetDto?> Handle(Command request, CancellationToken cancellationToken)
            {
                User userBeforeChange = await unitOfWork.GetUserRepository().FindByIdAsync(request.UserPostRefreshTokenDto.Id);

                if (userBeforeChange == null)
                    return null;

                userBeforeChange.RefreshToken = request.UserPostRefreshTokenDto.RefreshToken;
                userBeforeChange.RefreshTokenCreatedAt = request.UserPostRefreshTokenDto.RefreshTokenCreatedAt;
                userBeforeChange.RefreshTokenExpiredAt = request.UserPostRefreshTokenDto.RefreshTokenExpiredAt;

                User user = await unitOfWork.GetUserRepository().ReplaceOneAsync(userBeforeChange);

                if (user == null)
                    return null;

                return userBeforeChange.ToDto();
            }
        }
    }
}
