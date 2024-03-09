using Application.Mappers;
using Application.V1.Dtos.Admin.Users;
using Infrastructure.UnitOfWork;
using MediatR;

namespace Application.V1.Features.Users
{
    public static class GetAll
    {
        public sealed class Query : IRequest<IEnumerable<UserGetDto>>
        {
        }

        public sealed class Handler(IUnitOfWork unitOfWork) : IRequestHandler<Query, IEnumerable<UserGetDto>>
        {
            private readonly IUnitOfWork unitOfWork = unitOfWork;
            public async Task<IEnumerable<UserGetDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                return (await unitOfWork.GetUserRepository().FilterByAsync(x => true)).Select(x => x.ToDto());
            }
        }
    }
}
