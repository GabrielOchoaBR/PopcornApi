using Application.Mappers;
using Application.V1.Dtos.Admin.Users;
using FluentValidation;
using Infrastructure.UnitOfWork;
using MediatR;

namespace Application.V1.Features.Users
{
    public static class GetById
    {
        public sealed class Query : IRequest<UserGetDto?>
        {
            public required string Id { get; set; }
        }

        public sealed class Handler(IUnitOfWork unitOfWork) : IRequestHandler<Query, UserGetDto?>
        {
            private readonly IUnitOfWork unitOfWork = unitOfWork;

            public async Task<UserGetDto?> Handle(Query request, CancellationToken cancellationToken) =>
                (await unitOfWork.GetUserRepository().FindByIdAsync(request.Id))?.ToDto();
        }

        public sealed class Validator : AbstractValidator<Query>
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
