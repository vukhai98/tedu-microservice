using Infrastructure.Extensions;
using Inventory.Grpc.Repositories;
using Inventory.Grpc.Repositories.Interfaces;
using MongoDB.Driver;
using Shared.Configurations;
using System.Runtime.CompilerServices;

namespace Inventory.Grpc
{
    public static class ServiceExtensions
    {
        public static void AddInfratructureServices(this IServiceCollection services)
        {
            services.AddScoped<IInventoryRepository, InventoryRepository>();
        }

        public static IServiceCollection AddConfigurationSettings(this IServiceCollection services, IConfiguration configuration)
        {
            var mongoDBSettings = configuration.GetSection(nameof(MongoDBSettings)).Get<MongoDBSettings>();

            services.AddSingleton(mongoDBSettings);

            return services;
        }

        private static string getMongoConnectionString(this IServiceCollection services)
        {
            var mongoDBSettings = services.GetOptions<MongoDBSettings>(nameof(MongoDBSettings));

            if (string.IsNullOrEmpty(mongoDBSettings.ConnectionString))
                throw new ArgumentNullException("MongoDB connection string is not configured.");

            var databaseName = mongoDBSettings.DatabaseName;
            var mongoDBConnectionString = mongoDBSettings.ConnectionString + "/" + databaseName + "?authSource = admin";

            return mongoDBConnectionString;
        }

        public static void ConfigurationMongoDBClient(this IServiceCollection services)
        {
            services.AddSingleton<IMongoClient>(
                new MongoClient(getMongoConnectionString(services)))
                    .AddScoped(x => x.GetService<IMongoClient>()?.StartSession());
        }
    }
}
