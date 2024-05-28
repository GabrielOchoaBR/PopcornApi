using Application.V1.Dtos.Admin.Users;
using Domain.V1.Entities.Users;

namespace Application.Mappers
{
    public static class UserMapper
    {
        public static User ToDomainModel(this UserPostDto userPostDto) => new()
        {
            Name = userPostDto.Name,
            Password = userPostDto.Password,
            Email = userPostDto.Email,

            Roles = userPostDto.Roles
        };

        public static void Parse(this User user, UserPutDto userPutDto)
        {
            user.Name = userPutDto.Name;
            user.Email = userPutDto.Email;
            user.Roles = userPutDto.Roles;
        }

        public static UserGetDto ToDto(this User user)
            => new(user.Id.ToString(),
                   user.Name,
                   user.Email,
                   user.RefreshToken,
                   user.RefreshTokenCreatedAt,
                   user.RefreshTokenExpiredAt,
                   user.Roles,
                   user.CreatedAt,
                   user.CreatedBy?.ToString()!,
                   user.UpdatedAt,
                   user.UpdatedBy?.ToString());
    }
}
