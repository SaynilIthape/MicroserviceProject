using Discount.Grpc.Models;
using Microsoft.EntityFrameworkCore;

namespace Discount.Grpc.Data
{
    public class DiscountDBContext:DbContext
    {
        public DiscountDBContext(DbContextOptions<DiscountDBContext> options):base(options)
        {
            
        }
        public DbSet<Coupon> Coupons { get; set; }  =default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Coupon>().HasData(
                new Coupon { Id = 1, ProductName = "MotoG", Description = "Moto Discount", Amount = 230 },
                new Coupon { Id = 2, ProductName = "Samsung S30", Description = "Samsung Discount", Amount = 100 }
            );  
        }
    }
}
