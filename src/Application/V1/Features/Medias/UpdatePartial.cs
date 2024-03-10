using Application.Engines.DataControl;
using Application.Mappers;
using Application.V1.Dtos.Medias;
using Domain.V1.Entities.Medias;
using Infrastructure.UnitOfWork;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;

namespace Application.V1.Features.Medias
{
    public static class UpdatePartial
    {
        public sealed class Command : IRequest<MediaGetDto?>
        {
            public required string Id { get; set; }

            public required JsonPatchDocument<MediaGetDto> JsonPatchDocument { get; set; }
        }

        public sealed class Handler(IUnitOfWork unitOfWork, IUserDataControl userDataControl) : IRequestHandler<Command, MediaGetDto?>
        {
            private readonly IUnitOfWork unitOfWork = unitOfWork;
            private readonly IUserDataControl userDataControl = userDataControl;

            public async Task<MediaGetDto?> Handle(Command request, CancellationToken cancellationToken)
            {
                Media media = await unitOfWork.GetMediaRepository().FindByIdAsync(request.Id);

                if (media == null)
                    return null;

                MediaGetDto changes = media.ToDto();

                request.JsonPatchDocument.ApplyTo(changes);

                media = changes.ToDomainModel();

                userDataControl.SetModifiedInfo(media);

                var changed = await unitOfWork.GetMediaRepository().ReplaceOneAsync(media);

                if (changed == null)
                    return null;

                return media.ToDto();
            }
        }


    }
}
