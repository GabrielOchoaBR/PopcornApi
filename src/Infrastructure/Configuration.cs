using System.Diagnostics.CodeAnalysis;
using Infrastructure.Context;
using Infrastructure.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace Infrastructure
{
    [ExcludeFromCodeCoverage]
    public static class Configuration
    {
        public static void AddInfrastructureConfiguration(this IServiceCollection services, AppDbContextConfiguration appDbContextConfig, CustomLoggerProviderConfig customLoggerProviderConfig)
        {
            services.AddScoped<UnitOfWork.IUnitOfWork, UnitOfWork.UnitOfWork>();

            services.AddScoped<AppDbContext.Connection>(x => () =>
            {
                if (appDbContextConfig.DatabaseName == null)
                    throw new Exception("DatabaseName is invalid.");

                return new MongoClient(appDbContextConfig.ConnectionString).GetDatabase(appDbContextConfig.DatabaseName); 
            });

            services.AddLogging(x =>
            {
                x.AddProvider(new CustomLoggerProvider(new CustomLoggerProviderConfig()
                {
                    LogLevel = customLoggerProviderConfig.LogLevel,
                    EventId = customLoggerProviderConfig.EventId,
                    FileName = customLoggerProviderConfig.FileName
                }));
            });
        }
    }
}
