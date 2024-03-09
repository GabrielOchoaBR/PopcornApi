using Application.Engines.Cryptography;
using Application.Mappers;
using Application.V1.Dtos.Admin.Users;
using Domain.V1.Entities.Users;
using FluentValidation;
using Infrastructure.UnitOfWork;
using MediatR;

namespace Application.V1.Features.Users
{
    public static class GetByNameAndPassword
    {
        public sealed class Query : IRequest<UserGetDto?>
        {
            public required UserGetByNameAndPasswordDto UserGetByNameAndPasswordDto { get; set; }
        }

        public sealed class Handler(IUnitOfWork unitOfWork, ITextCryptography textCryptography) : IRequestHandler<Query, UserGetDto?>
        {
            private readonly IUnitOfWork unitOfWork = unitOfWork;
            private readonly ITextCryptography textCryptography = textCryptography;

            public async Task<UserGetDto?> Handle(Query request, CancellationToken cancellationToken)
            {
                User? user = await unitOfWork.GetUserRepository().FindOneAsync(x => x.Name == request.UserGetByNameAndPasswordDto.Name);

                if (user == null || await textCryptography.VerifyAsync(request.UserGetByNameAndPasswordDto.Password, user.Password) == false)
                    return null;

                return user.ToDto();
            }
        }

        public sealed class Validator : AbstractValidator<Query>
        {
            public Validator()
            {
                RuleFor(x => x.UserGetByNameAndPasswordDto.Name)
                    .NotEmpty();

                RuleFor(x => x.UserGetByNameAndPasswordDto.Password)
                    .NotEmpty();
            }
        }
    }
}
