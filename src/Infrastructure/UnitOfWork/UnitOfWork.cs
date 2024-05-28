using Infrastructure.Context;
using Infrastructure.Repositories.Medias;
using Infrastructure.Repositories.Users;

namespace Infrastructure.UnitOfWork
{
    public class UnitOfWork(AppDbContext.Connection dbContext) : IUnitOfWork
    {
        private readonly AppDbContext.Connection appDbContext = dbContext;

        private IUserRepository? userRepository;
        private IMediaRepository? mediaRepository;

        public IUserRepository GetUserRepository() => userRepository ??= new UserRepository(appDbContext);

        public IMediaRepository GetMediaRepository() => mediaRepository ??= new MediaRepository(appDbContext);
    }
}
