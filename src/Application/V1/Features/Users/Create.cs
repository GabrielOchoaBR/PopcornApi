using Application.Configurations;
using Application.Engines.Cryptography;
using Application.Engines.DataControl;
using Application.Mappers;
using Application.V1.Dtos.Admin.Users;
using Domain.V1.Entities.Users;
using FluentValidation;
using Infrastructure.UnitOfWork;
using MediatR;

namespace Application.V1.Features.Users
{
    public static class Create
    {
        public sealed class Command : IRequest<UserGetDto?>
        {
            public required UserPostDto UserPostDto { get; set; }
        }

        public sealed class Handler(IUnitOfWork unitOfWork, ITextCryptography textCryptography, IUserDataControl userDataControl) : IRequestHandler<Command, UserGetDto?>
        {
            private readonly IUnitOfWork unitOfWork = unitOfWork;
            private readonly ITextCryptography textCryptography = textCryptography;
            private readonly IUserDataControl userDataControl = userDataControl;

            public async Task<UserGetDto?> Handle(Command request, CancellationToken cancellationToken)
            {
                User user = request.UserPostDto.ToDomainModel();
                user.Password = await textCryptography.HashAsync(user.Password);

                userDataControl.SetCreatedInfo(user);

                await unitOfWork.GetUserRepository().InsertOneAsync(user);

                if (user.Id == MongoDB.Bson.ObjectId.Empty)
                    return null;

                return user.ToDto();
            }
        }

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator(IUnitOfWork unitOfWork, IPasswordConfiguration passwordValidation)
            {
                RuleFor(x => x.UserPostDto.Name)
                    .NotEmpty()
                    .MustAsync(async (name, cancellationToken) => await unitOfWork.GetUserRepository().FindOneAsync(x => x.Name == name) == null)
                    .WithMessage("User already registered");

                RuleFor(x => x.UserPostDto.Password)
                    .Matches(passwordValidation.RegexPattern)
                    .WithMessage(passwordValidation.ErrorMessage);

                RuleFor(x => x.UserPostDto.Email)
                    .NotEmpty()
                    .EmailAddress();
            }
        }
    }
}
