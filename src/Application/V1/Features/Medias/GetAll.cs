using Application.Mappers;
using Application.V1.Dtos.Medias;
using Application.V1.Dtos.Shared;
using FluentValidation;
using Infrastructure.UnitOfWork;
using MediatR;

namespace Application.V1.Features.Medias
{
    public static class GetAll
    {
        public sealed class Query : IRequest<ResponseGetAll<MediaGetDto>>
        {
            public required QueryGetAll QueryGetAll { get; set; }
        }

        public sealed class Handler(IUnitOfWork unitOfWork) : IRequestHandler<Query, ResponseGetAll<MediaGetDto>>
        {
            private readonly IUnitOfWork unitOfWork = unitOfWork;

            public async Task<ResponseGetAll<MediaGetDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var (result, quantity) = await unitOfWork.GetMediaRepository().GetAllAsync(request.QueryGetAll.SearchTerm,
                                                                                 request.QueryGetAll.SortColumn,
                                                                                 request.QueryGetAll.SortDirection.ToString(),
                                                                                 request.QueryGetAll.PageIndex,
                                                                                 request.QueryGetAll.PageSize);

                return new(result.Select(x => x.ToDto()), quantity);
            }
        }

        public sealed class Validator : AbstractValidator<Query>
        {
            public Validator()
            {
                RuleFor(x => x.QueryGetAll.PageIndex)
                    .NotEmpty()
                    .GreaterThan(0);

                RuleFor(x => x.QueryGetAll.PageSize)
                    .NotEmpty()
                    .GreaterThan(0);
            }
        }

    }
}
