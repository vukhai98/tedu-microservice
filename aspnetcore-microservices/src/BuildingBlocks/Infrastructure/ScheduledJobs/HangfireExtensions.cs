﻿using Hangfire;
using Hangfire.Console;
using Hangfire.Console.Extensions;
using Hangfire.Mongo;
using Hangfire.Mongo.Migration.Strategies;
using Hangfire.Mongo.Migration.Strategies.Backup;
using Hangfire.PostgreSql;
using Infrastructure.Extensions;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Newtonsoft.Json;
using Shared.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.ScheduledJobs
{
    public static class HangfireExtensions
    {
        public static IServiceCollection AddHangFireFServiceCustom(this IServiceCollection services)
        {
            var settings = services.GetOptions<HangFireSettings>(nameof(HangFireSettings));
            
            if (settings == null || string.IsNullOrEmpty(settings.Storage.ConnectionString))
            {
                throw new Exception(" HangFireSettings is not configured properly!");
            }

            services.AddHangfireServer(options =>
            {
                options.ServerName = settings.ServerName;
            });

            services.ConfigureHangfireServices(settings);

            return services;
        }

        private static IServiceCollection ConfigureHangfireServices(this IServiceCollection services, HangFireSettings settings)
        {
            if (string.IsNullOrEmpty(settings.Storage.DBProvider))
                throw new Exception("HangFire DBProvider is not configured.");

            switch (settings.Storage.DBProvider.ToLower())
            {
                case "mongodb":

                    var mongoUrlBuilder = new MongoUrlBuilder(settings.Storage.ConnectionString);

                    var mongoClientSettings = MongoClientSettings.FromUrl(new MongoUrl(settings.Storage.ConnectionString));

                    mongoClientSettings.SslSettings = new SslSettings
                    {
                        EnabledSslProtocols = System.Security.Authentication.SslProtocols.Tls12
                    };

                    var mongoClient = new MongoClient(mongoClientSettings);

                    var mongoStorageOptions = new MongoStorageOptions
                    {
                        MigrationOptions = new MongoMigrationOptions
                        {
                            MigrationStrategy = new MigrateMongoMigrationStrategy(),
                            BackupStrategy = new CollectionMongoBackupStrategy(),
                        },
                        CheckConnection = true,
                        Prefix = "SchedulerQueue",
                        CheckQueuedJobsStrategy = CheckQueuedJobsStrategy.TailNotificationsCollection
                    };

                    services.AddHangfire((provider, config) =>
                    {
                        config.UseSimpleAssemblyNameTypeSerializer()
                              .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                              .UseRecommendedSerializerSettings()
                              .UseConsole()
                              .UseMongoStorage(mongoClient, mongoUrlBuilder.DatabaseName, mongoStorageOptions);

                        var jsonSetting = new JsonSerializerSettings
                        {
                            TypeNameHandling = TypeNameHandling.All,
                        };

                        config.UseSerializerSettings(jsonSetting);

                    });
                    services.AddHangfireConsoleExtensions();
                    break;

                case "postgresql":
                    services.AddHangfire(x =>{
                        x.UsePostgreSqlStorage(settings.Storage.ConnectionString);
                    });
                    break;

                case "mssql":
                    break;

                case "mysql":
                    break;

                default:
                    throw new Exception("DBProvider is not esxit");
            }

            return services;
        }
    }
}
