using Microsoft.EntityFrameworkCore;

namespace Discount.Grpc.Data
{
    public static class Extensions
    {
        public static IApplicationBuilder UseMigrationDB(this IApplicationBuilder app)
        {
            var scope = app.ApplicationServices.CreateScope();  
            var dbContext = scope.ServiceProvider.GetRequiredService<DiscountDBContext>();
            dbContext.Database.EnsureCreated();
            dbContext.Database.MigrateAsync();
            return app;
        }

    }
}
