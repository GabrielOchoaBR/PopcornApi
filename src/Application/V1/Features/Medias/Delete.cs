using Application.Mappers;
using Application.V1.Dtos.Medias;
using Domain.V1.Entities.Medias;
using FluentValidation;
using Infrastructure.UnitOfWork;
using MediatR;

namespace Application.V1.Features.Medias
{
    public static class Delete
    {
        public sealed class Command : IRequest<MediaGetDto?>
        {
            public required string Id { get; set; }
        }

        public sealed class Handler(IUnitOfWork unitOfWork) : IRequestHandler<Command, MediaGetDto?>
        {
            private readonly IUnitOfWork unitOfWork = unitOfWork;

            public async Task<MediaGetDto?> Handle(Command request, CancellationToken cancellationToken)
            {
                Media? media = await unitOfWork.GetMediaRepository().DeleteByIdAsync(request.Id);

                if (media == null)
                    return null;

                return media.ToDto();
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
