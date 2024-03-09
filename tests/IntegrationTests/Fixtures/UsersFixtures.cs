using Application.V1.Dtos.Admin.Users;
using AutoFixture;
using Domain.V1.Entities.Users;
using MongoDB.Bson;
using PopcornApi.Security.TokenServices;

namespace IntegrationTests.Fixtures
{
    public class UsersFixtures
    {
        private readonly Fixture fixture = new();
        private readonly ITokenService tokenService;

        public UsersFixtures(ITokenService tokenService)
        {
            fixture.Register(ObjectId.GenerateNewId);
            this.tokenService = tokenService;
        }

        public IEnumerable<User> CreateUser(int count = 1)
            => fixture.Build<User>()
                .Without(x => x.Id)
                .With(x => x.Name, "user")
                .Without(x => x.RefreshToken)
                .Without(x => x.RefreshTokenCreatedAt)
                .Without(x => x.RefreshTokenExpiredAt)
                .CreateMany(count);

        public string CreateToken(RoleType[] roleTypes)
            => tokenService.GenerateToken(
                fixture.Build<UserGetDto>()
                    .With(x => x.Id, ObjectId.GenerateNewId().ToString())
                    .With(x => x.Roles, roleTypes)
                    .Create(),
                DateTime.UtcNow.AddMinutes(5));
    }
}
