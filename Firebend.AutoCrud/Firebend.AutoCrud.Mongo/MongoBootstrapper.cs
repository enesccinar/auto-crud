using System;
using Firebend.AutoCrud.Mongo.HostedServices;
using Firebend.AutoCrud.Mongo.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Core.Events;
using Scrutor;

namespace Firebend.AutoCrud.Mongo
{
    public static class MongoBootstrapper
    {
        public static IServiceCollection ConfigureMongoDb(
            this IServiceCollection services,
            string connectionString,
            bool enableCommandLogging,
            IMongoDbConfigurator configurator)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString));
            }
            
            services.Scan(action => action.FromAssemblies()
                .AddClasses(classes => classes.AssignableTo<IMongoMigration>())
                .UsingRegistrationStrategy(RegistrationStrategy.Append)
                .As<IMongoMigration>()
                .WithTransientLifetime()
            );

            var mongoUrl = new MongoUrl(connectionString);

            configurator.Configure();

            services.AddScoped<IMongoClient>(x =>
            {
                var logger = x.GetService<ILogger<MongoClient>>();

                var mongoClientSettings = MongoClientSettings.FromUrl(mongoUrl);

                if (enableCommandLogging)
                {
                    mongoClientSettings.ClusterConfigurator = cb =>
                    {
                        cb.Subscribe<CommandStartedEvent>(e =>
                            logger.LogDebug("MONGO: {CommandName} - {Command}", e.CommandName, e.Command.ToJson()));

                        cb.Subscribe<CommandSucceededEvent>(e =>
                            logger.LogDebug("SUCCESS: {CommandName}({Duration}) - {Reply}", e.CommandName, e.Duration,
                                e.Reply.ToJson()));

                        cb.Subscribe<CommandFailedEvent>(e =>
                            logger.LogError("ERROR: {CommandName}({Duration}) - {Error}", e.CommandName, e.Duration,
                                e.Failure));
                    };
                }

                return new MongoClient(mongoClientSettings);
            });

            services.AddHostedService<ConfigureCollectionsHostedService>();

            services.AddHostedService<MongoMigrationHostedService>();

            return services;
        }
    }
}