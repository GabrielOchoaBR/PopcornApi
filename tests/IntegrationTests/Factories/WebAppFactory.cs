﻿using Application.Engines.Cryptography;
using Infrastructure.Context;
using Infrastructure.UnitOfWork;
using IntegrationTests.Fixtures;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Mongo2Go;
using MongoDB.Driver;
using PopcornApi.Security.TokenServices;

namespace IntegrationTests.Factories
{
    internal class WebAppFactory : WebApplicationFactory<Program>
    {
        private readonly string connectionString = string.Empty;
        private MongoDbRunner mongoDbRunner;

        public WebAppFactory()
        {
            mongoDbRunner = MongoDbRunner.Start(singleNodeReplSet: false, additionalMongodArguments: $"--quiet");
            connectionString = mongoDbRunner.ConnectionString;


            UnitOfWork = new UnitOfWork(Services.GetRequiredService<IMongoDatabase>);
            TextCryptography = Services.GetRequiredService<ITextCryptography>();
            UsersFixtures = new UsersFixtures(Services.GetRequiredService<ITokenService>());
            MediasFixtures = new MediasFixtures();
        }

        public IUnitOfWork UnitOfWork { get; }
        public ITextCryptography TextCryptography { get; }
        public UsersFixtures UsersFixtures { get; }
        public MediasFixtures MediasFixtures { get; }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureLogging(x =>
            {
                x.ClearProviders();
            });

            builder.ConfigureTestServices(services =>
            {
                services.RemoveAll(typeof(AppDbContext.Connection));

                services.AddSingleton(x =>
                {
                    MongoClient client = new(connectionString);
                    return client.GetDatabase("IntegrationTest");
                });
                services.AddScoped<AppDbContext.Connection>(x =>
                    () => (IMongoDatabase)x.GetRequiredService(typeof(IMongoDatabase)));
            });
        }
    }
}
