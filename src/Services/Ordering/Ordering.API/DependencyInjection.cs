using BuildingBlocks.Exceptions.Handler;
using Carter;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using System.Reflection;

namespace Ordering.API
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration config)
        {
            // Register your API services here, e.g. controllers, authentication, etc.
            //services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly())); 
            services.AddCarter();
            services.AddExceptionHandler<CustomExceptionHandler>();
            services.AddHealthChecks()
                .AddSqlServer(config.GetConnectionString("Database"));
            return services;
        }

        public static WebApplication UseWebApplication(this WebApplication builder)
        {
            builder.MapCarter();
            builder.UseExceptionHandler(options => { });
            builder.UseHealthChecks("/health" ,
                new HealthCheckOptions
                    { 
                ResponseWriter =  UIResponseWriter.WriteHealthCheckUIResponse   
                });
            return builder; 
        }
    }
}
