using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using GreenPipes;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Unidash.Core.Infrastructure;

namespace Unidash.Core.Extensions
{
    public static class HostingStartupExtensions
    {
        /// <summary>
        /// Adds a suitable entity repository implementation based on the connection string.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configurationSection"></param>
        /// <returns></returns>
        public static IServiceCollection AddEntityRepository(this IServiceCollection services, [NotNull] IConfigurationSection configurationSection)
        {
            services.Configure<MongoDbConnectionOptions>(configurationSection);

            var options = configurationSection.Get<MongoDbConnectionOptions>();
            var uri = new Uri(options.ConnectionString);

            switch (uri.Scheme)
            {
                case "mongodb":
                    services.AddSingleton(typeof(IEntityRepository<>), typeof(MongoEntityRepository<>));
                    break;
                case "local":
                    services.AddSingleton(typeof(IEntityRepository<>), typeof(InMemoryEntityRepository<>));
                    break;
            }

            return services;
        }

        public static IServiceCollection AddMessageBroker(this IServiceCollection services, params Type[] consumerAssemblyTypes)
        {
            services.AddMassTransit(x =>
            {
                x.AddConsumers(consumerAssemblyTypes.Select(x => x.Assembly).ToArray());

                x.AddBus(context => Bus.Factory.CreateUsingRabbitMq(config =>
                {
                    config.UseHealthCheck(context);

                    config.Host("rabbitmq://rabbitmq");

                    config.ReceiveEndpoint(consumerAssemblyTypes.First().FullName, configurator =>
                    {
                        configurator.PrefetchCount = 16;
                        configurator.UseMessageRetry(r => r.Interval(2, 100));

                        configurator.ConfigureConsumers(context);
                    });
                }));
            });

            services.AddMassTransitHostedService();

            return services;
        }
    }
}