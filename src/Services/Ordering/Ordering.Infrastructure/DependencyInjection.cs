using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Ordering.Application.Data;
using Ordering.Infrastructure.Data;
using Ordering.Infrastructure.Data.Interceptors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfraStructureServices(this IServiceCollection services, IConfiguration config)
        {
            var connectionString = config.GetConnectionString("Database");
            // Register your infrastructure services here, e.g. database context, repositories, etc.
            
            services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptors>();
            services.AddScoped<ISaveChangesInterceptor,  DispatchDomainEventInterceptor>();   
            services.AddDbContext<ApplicationDbContext>((sp,option) =>
            {
                //option.AddInterceptors(sp.GetService<ISaveChangesInterceptor>());
                //option.UseSqlServer(connectionString);

                // Resolve ALL registered ISaveChangesInterceptor
                var interceptors = sp.GetServices<ISaveChangesInterceptor>();

                option.AddInterceptors(interceptors);
                option.UseSqlServer(connectionString);
            });

            services.AddScoped<IApplicationDbContext, ApplicationDbContext>();  

            return services;
        }
    }
}
