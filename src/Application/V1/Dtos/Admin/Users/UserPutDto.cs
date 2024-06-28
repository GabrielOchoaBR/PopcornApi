using Domain.V1.Entities.Users;

namespace Application.V1.Dtos.Admin.Users
{
    public record UserPutDto(string Id,
                             string Name,
                             string Email,
                             IEnumerable<RoleType> Roles);
}
