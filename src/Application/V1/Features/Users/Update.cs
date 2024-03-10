using Application.Engines.DataControl;
using Application.Mappers;
using Application.V1.Dtos.Admin.Users;
using Domain.V1.Entities.Users;
using FluentValidation;
using Infrastructure.UnitOfWork;
using MediatR;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Application.V1.Features.Users
{
    public static class Update
    {
        public sealed class Command : IRequest<UserGetDto?>
        {
            public required UserPutDto UserPutDto { get; set; }
        }

        public sealed class Handler(IUnitOfWork unitOfWork, IUserDataControl userDataControl) : IRequestHandler<Command, UserGetDto?>
        {
            private readonly IUnitOfWork unitOfWork = unitOfWork;
            private readonly IUserDataControl userDataControl = userDataControl;

            public async Task<UserGetDto?> Handle(Command request, CancellationToken cancellationToken)
            {
                User userPersist = await unitOfWork.GetUserRepository().FindByIdAsync(request.UserPutDto.Id);

                if (userPersist == null)
                    return null;

                userPersist.Parse(request.UserPutDto);

                userDataControl.SetModifiedInfo(userPersist);

                User? changed = await unitOfWork.GetUserRepository().ReplaceOneAsync(userPersist);

                if (changed == null)
                    return null;

                return userPersist.ToDto();
            }
        }

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator(IUnitOfWork unitOfWork)
            {
                RuleFor(x => x.UserPutDto.Id)
                    .NotEmpty()
                    .MustAsync(async (id, cancellationToken) => await unitOfWork.GetUserRepository().FindByIdAsync(id) != null)
                    .WithMessage("User not found.");

                RuleFor(x => x.UserPutDto.Name)
                    .NotEmpty();

                RuleFor(x => x.UserPutDto)
                    .MustAsync(async (user, cancellationToken) =>
                    {
                        var objectId = new ObjectId(user.Id);
                        var filter = Builders<User>.Filter.And([
                            Builders<User>.Filter.Ne(doc => doc.Id, objectId),
                            Builders<User>.Filter.Eq(doc => doc.Name, user.Name)
                        ]);

                        return !(await unitOfWork.GetUserRepository().FilterByAsync(filter)).Any();
                    })
                    .WithMessage("Username already registered");

                RuleFor(x => x.UserPutDto.Email)
                    .NotEmpty()
                    .EmailAddress();
            }
        }
    }
}
