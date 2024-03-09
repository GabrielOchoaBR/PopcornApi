using AutoFixture;
using MongoDB.Bson;

namespace UnitTests
{
    public class BaseTests
    {
        protected readonly Fixture fixture = new();
        public BaseTests()
        {
            fixture.Register(ObjectId.GenerateNewId);
        }
    }
}
