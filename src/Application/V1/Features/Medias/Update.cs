using Application.Mappers;
using Application.V1.Dtos.Medias;
using Domain.V1.Entities.Medias;
using Infrastructure.UnitOfWork;
using MediatR;

namespace Application.V1.Features.Medias
{
    public static class Update
    {
        public sealed class Command : IRequest<MediaGetDto?>
        {
            public required MediaPutDto MediaPutDto { get; set; }
        }

        public sealed class Handler(IUnitOfWork unitOfWork) : IRequestHandler<Command, MediaGetDto?>
        {
            private readonly IUnitOfWork unitOfWork = unitOfWork;

            public async Task<MediaGetDto?> Handle(Command request, CancellationToken cancellationToken)
            {
                Media? mediaPersist = await unitOfWork.GetMediaRepository().FindByIdAsync(request.MediaPutDto.Id);

                if (mediaPersist == null)
                    return null;

                mediaPersist.Parse(request.MediaPutDto);

                if (mediaPersist.Rating != null)
                {
                    Rating? rating = await unitOfWork.GetMediaRepository().RatingFindByNameAsync(mediaPersist.Rating!.Name);
                    if (rating != null)
                        mediaPersist.Rating = rating;
                }

                if (mediaPersist.Director != null)
                {
                    Director? director = await unitOfWork.GetMediaRepository().DirectorFindByNameAsync(mediaPersist.Director!.Name);
                    if (director != null)
                        mediaPersist.Director = director;
                }

                Media changed = await unitOfWork.GetMediaRepository().ReplaceOneAsync(mediaPersist);

                if (changed == null)
                {
                    return null;
                }

                return mediaPersist.ToDto();
            }
        }
    }
}
