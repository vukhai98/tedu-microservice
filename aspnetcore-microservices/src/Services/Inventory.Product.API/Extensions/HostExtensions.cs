using Common.Logging;
using Inventory.Product.API.Persistence;
using MongoDB.Driver;
using Serilog;
using Shared.Configurations;

namespace Inventory.Product.API.Extensions
{
    public static class HostExtensions
    {
        public static IHost MigrateDatabase(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;
            var settings = services.GetService<MongoDBSettings>();

            if (string.IsNullOrEmpty(settings.ConnectionString))
                throw new ArgumentNullException("MongoDB connection string is not configured.");

            var mongoClient = services.GetRequiredService<IMongoClient>();

            new InventoryDbSeed().SeedDataAsync(mongoClient, settings).Wait();

            return host;

        }
        public static void AddAppConfigurations(this ConfigureHostBuilder host)
        {
            host.ConfigureAppConfiguration((context, config) =>
            {
                var env = context.HostingEnvironment;
                config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                        .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                        .AddEnvironmentVariables();
            }).UseSerilog(Serilogger.Configure);
        }
    }
}
