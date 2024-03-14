using Application.V1.Dtos.Medias;
using Application.V1.Features.Medias;
using HotChocolate.Authorization;
using HotChocolate.Data;
using MediatR;

namespace PopcornApi.Controllers.V1
{
    public class QueryController
    {
        [Authorize(Policy = "Read")]
        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public async Task<IQueryable<MediaGetDto>> GetMediaAsync([Service] IMediator mediator) =>
            await mediator.Send(new GetByGraphQL.Query());
    }
}
