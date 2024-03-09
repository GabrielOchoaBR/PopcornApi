using Application.Mappers;
using Application.V1.Dtos.Admin.Users;
using Domain.V1.Entities.Users;
using FluentValidation;
using Infrastructure.UnitOfWork;
using MediatR;

namespace Application.V1.Features.Users
{
    public static class Delete
    {
        public sealed class Command : IRequest<UserGetDto?>
        {
            public required string Id { get; set; }
        }

        public sealed class Handler(IUnitOfWork unitOfWork) : IRequestHandler<Command, UserGetDto?>
        {
            private readonly IUnitOfWork unitOfWork = unitOfWork;

            public async Task<UserGetDto?> Handle(Command request, CancellationToken cancellationToken)
            {
                User? user = await unitOfWork.GetUserRepository().DeleteByIdAsync(request.Id);

                if (user == null)
                    return null;

                return user.ToDto();
            }
        }

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(x => x.Id)
                    .Must((Id) => MongoDB.Bson.ObjectId.TryParse(Id, out _))
                    .WithMessage("Id invalid");
            }
        }
    }
}
