using Application.Engines.DataControl;
using Application.Mappers;
using Application.V1.Dtos.Medias;
using Domain.V1.Entities.Medias;
using FluentValidation;
using Infrastructure.UnitOfWork;
using MediatR;

namespace Application.V1.Features.Medias
{
    public static class Create
    {
        public sealed class Command : IRequest<MediaGetDto>
        {
            public required MediaPostDto MediaPostDto { get; set; }
        }

        public sealed class Handler(IUnitOfWork unitOfWork, IUserDataControl userDataControl) : IRequestHandler<Command, MediaGetDto>
        {
            private readonly IUnitOfWork unitOfWork = unitOfWork;
            private readonly IUserDataControl userDataControl = userDataControl;

            public async Task<MediaGetDto> Handle(Command request, CancellationToken cancellationToken)
            {
                Media media = request.MediaPostDto.ToDomainModel();

                Rating? rating = await unitOfWork.GetMediaRepository().RatingFindByNameAsync(media.Rating!.Name);
                if (rating != null)
                    media.Rating = rating;

                Director? diretor = await unitOfWork.GetMediaRepository().DirectorFindByNameAsync(media.Director!.Name);
                if (diretor != null)
                    media.Director = diretor;

                userDataControl.SetCreatedInfo(media);

                await unitOfWork.GetMediaRepository().InsertOneAsync(media);

                return media.ToDto();
            }
        }

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(x => x.MediaPostDto.Title)
                    .NotEmpty();

                RuleFor(x => x.MediaPostDto.Rating.Name)
                    .NotNull();

                RuleFor(x => x.MediaPostDto.Director.Name)
                                    .NotNull();
            }
        }
    }
}
