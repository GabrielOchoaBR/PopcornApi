using Application.Mappers;
using Application.V1.Dtos.Medias;
using Application.V1.Dtos.Medias.Director;
using Application.V1.Dtos.Medias.Rating;
using Domain.V1.Entities.Medias;
using Infrastructure.UnitOfWork;
using MediatR;

namespace Application.V1.Features.Medias
{
    public static class GetByGraphQL
    {
        public sealed class Query : IRequest<IQueryable<MediaGetDto>> { }

        public sealed class Handler(IUnitOfWork unitOfWork) : IRequestHandler<Query, IQueryable<MediaGetDto>>
        {
            private readonly IUnitOfWork unitOfWork = unitOfWork;

            public async Task<IQueryable<MediaGetDto>> Handle(Query request, CancellationToken cancellationToken) =>
                (await unitOfWork.GetMediaRepository().GetQueryableAsync()).SelectToDto();
        }
    }
}
