﻿using AutoFixture;
using Domain.V1.Entities.Medias;
using MongoDB.Bson;

namespace IntegrationTests.Fixtures
{
    internal class MediaFixtures
    {
        private readonly Fixture fixture = new();

        public MediaFixtures()
        {
            fixture.Register(ObjectId.GenerateNewId);
        }

        public IEnumerable<Media> CreateMedia(int count = 1)
            => fixture.Build<Media>()
                .Without(x => x.Id)
                .CreateMany(count);
    }
}
