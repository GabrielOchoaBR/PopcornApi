using Domain.V1.Entities.Medias;

namespace Infrastructure.Repositories.Medias
{
    public interface IMediaRepository : IRepositoryBase<Media>
    {
        Task<Director> DirectorFindByNameAsync(string name);
        Task<Rating> RatingFindByNameAsync(string name);
    }
}
