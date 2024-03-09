using Domain.V1.Entities.Users;

namespace Application.V1.Dtos.Admin.Users
{
    public class UserPostDto
    {
        public required string Name { get; set; }
        public required string Password { get; set; }
        public required string Email { get; set; }

        public IEnumerable<RoleType> Roles { get; set; } = [];
    }
}
