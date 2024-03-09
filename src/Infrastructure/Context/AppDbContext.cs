using MongoDB.Driver;

namespace Infrastructure.Context
{
    public class AppDbContext
    {
        public delegate IMongoDatabase Connection();
    }
}
