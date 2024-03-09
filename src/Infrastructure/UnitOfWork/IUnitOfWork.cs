using Infrastructure.Repositories.Medias;
using Infrastructure.Repositories.Users;

namespace Infrastructure.UnitOfWork
{
    public interface IUnitOfWork
    {
        IUserRepository GetUserRepository();

        IMediaRepository GetMediaRepository();
    }
}