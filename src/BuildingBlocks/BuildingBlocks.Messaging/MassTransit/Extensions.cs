using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Reflection;

namespace BuildingBlocks.Messaging.MassTransit
{
    public static class Extensions
    {
        public static IServiceCollection AddMessageBroker(this IServiceCollection services, 
            IConfiguration configuration,Assembly? assembly=null)
        {
            //var options = new MassTransitOptions();
            //configure(options);
            services.AddMassTransit(x =>
            {
                x.SetKebabCaseEndpointNameFormatter();

                if (assembly != null)
                {
                    x.AddConsumers(assembly);
                }   
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(new Uri(configuration["MessageBroker:Host"]!), h =>
                    {
                        h.Username(configuration["MessageBroker:UserName"]);
                        h.Password(configuration["MessageBroker:Password"]);
                    });
                    // Configure endpoints for consumers
                    cfg.ConfigureEndpoints(context);
                });
            });
            return services;
        }   
    }
}
