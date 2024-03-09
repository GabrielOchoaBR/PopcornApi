using Application.Mappers;
using Application.V1.Dtos.Medias;
using Domain.V1.Entities.Medias;
using FluentValidation;
using Infrastructure.UnitOfWork;
using MediatR;

namespace Application.V1.Features.Medias
{
    public static class GetById
    {
        public sealed class Query : IRequest<MediaGetDto?>
        {
            public required string Id { get; set; }
        }

        public sealed class Handler(IUnitOfWork unitOfWork) : IRequestHandler<Query, MediaGetDto?>
        {
            private readonly IUnitOfWork unitOfWork = unitOfWork;
            public async Task<MediaGetDto?> Handle(Query request, CancellationToken cancellationToken)
            {
                Media? media = await unitOfWork.GetMediaRepository().FindByIdAsync(request.Id);

                if (media == null)
                    return null;

                return media.ToDto();
            }
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
