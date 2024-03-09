using Application.Engines.Cryptography;
using Application.Mappers;
using Application.V1.Dtos.Admin.Users;
using Application.Validations;
using Domain.V1.Entities.Users;
using FluentValidation;
using Infrastructure.UnitOfWork;
using MediatR;

namespace Application.V1.Features.Users
{
    public static class UpdatePassword
    {
        public sealed class Command : IRequest<UserGetDto?>
        {
            public required UserPostUpdatePasswordDto UserPostUpdatePasswordDto { get; set; }
        }

        public sealed class Handler(IUnitOfWork unitOfWork, ITextCryptography textCryptography) : IRequestHandler<Command, UserGetDto?>
        {
            private readonly IUnitOfWork unitOfWork = unitOfWork;
            private readonly ITextCryptography textCryptography = textCryptography;

            public async Task<UserGetDto?> Handle(Command request, CancellationToken cancellationToken)
            {
                User? user = await unitOfWork.GetUserRepository().FindByIdAsync(request.UserPostUpdatePasswordDto.Id);

                if (user == null ||
                    await textCryptography.VerifyAsync(request.UserPostUpdatePasswordDto.OldPassword, user.Password) == false)
                    return null;

                user.Password = await textCryptography.HashAsync(request.UserPostUpdatePasswordDto.NewPassword);

                User changed = await unitOfWork.GetUserRepository().ReplaceOneAsync(user);

                if (changed == null)
                    return null;

                return user.ToDto();
            }
        }

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator(IPasswordValidation passwordValidation)
            {
                RuleFor(x => x.UserPostUpdatePasswordDto.Id)
                    .Must((Id) => MongoDB.Bson.ObjectId.TryParse(Id, out _))
                    .WithMessage("Id invalid");

                RuleFor(x => x.UserPostUpdatePasswordDto.NewPassword)
                    .Matches(passwordValidation.RegexPattern)
                    .WithMessage(passwordValidation.ErrorMessage);
            }
        }
    }
}
