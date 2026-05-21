using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.Domain.Models;

namespace Ordering.Infrastructure.Data.Configurations
{
    public class CustomerConfigurations :IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.HasKey(c=>c.Id);    
            builder.Property(c=>c.Id).HasConversion(
                id => id.ToString(), // Convert Guid to string for storage
                str => Guid.Parse(str) // Convert string back to Guid when reading
            );
             builder.Property(c=>c.Name).IsRequired().HasMaxLength(100);    
             builder.Property(c=>c.Email).HasMaxLength(255);
            builder.HasIndex(c=>c.Email).IsUnique(); // Ensure email is unique
        }
    }
}
