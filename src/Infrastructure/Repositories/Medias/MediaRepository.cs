using Domain.V1.Entities.Medias;
using Infrastructure.Context;
using MongoDB.Driver;

namespace Infrastructure.Repositories.Medias
{
    public class MediaRepository(AppDbContext.Connection dbContext) : RepositoryBase<Media>(dbContext), IMediaRepository
    {
        protected override FilterDefinition<Media> GetAllFilterDefinition(string searchTerm)
        {
            return Builders<Media>.Filter.Where(x => x.Title.Contains(searchTerm, StringComparison.CurrentCultureIgnoreCase));
        }

        public async Task<Rating> RatingFindByNameAsync(string name)
        {
            FilterDefinition<Media> filterDefinition = Builders<Media>.Filter
                        .Where(x => x.Rating != null
                        && x.Rating.Name != null
                        && x.Rating.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase));

            return await collection.Find(filterDefinition)
                        .Project(x => new Rating()
                        {
                            Id = x.Rating!.Id,
                            Name = x.Rating.Name,
                            AllowedAge = x.Rating.AllowedAge
                        })
                        .FirstOrDefaultAsync();
        }

        public async Task<Director> DirectorFindByNameAsync(string name)
        {
            FilterDefinition<Media> filterDefinition = Builders<Media>.Filter
                        .Where(x => x.Director != null
                        && x.Director.Name != null
                        && x.Director.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase));

            return await collection.Find(filterDefinition)
                        .Project(x => new Director()
                        {
                            Id = x.Director!.Id,
                            Name = x.Director.Name,
                            BirthDate = x.Director.BirthDate
                        })
                        .FirstOrDefaultAsync();
        }

    }
}
