
using MassTransit;
using MassTransit.Definition;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

namespace David.Common.MassTransit
{
    public static class Extensions
    {
        public static IServiceCollection AddMassTransitWithRabbitMq(this IServiceCollection services, string serviceName)
        {
            services.AddMassTransit(x =>
            {
                x.AddConsumers(Assembly.GetEntryAssembly());

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host("localhost");
                    cfg.ConfigureEndpoints(context, new KebabCaseEndpointNameFormatter(serviceName, false));
                                    });
            });
            services.AddMassTransitHostedService();

            return services;
        }
    }
}
