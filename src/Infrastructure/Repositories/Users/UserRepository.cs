using Domain.V1.Entities.Users;
using Infrastructure.Context;
using MongoDB.Driver;

namespace Infrastructure.Repositories.Users
{
    public class UserRepository(AppDbContext.Connection dbContext) : RepositoryBase<User>(dbContext), IUserRepository
    {
        protected override FilterDefinition<User> GetAllFilterDefinition(string searchTerm) => null!;
    }
}
