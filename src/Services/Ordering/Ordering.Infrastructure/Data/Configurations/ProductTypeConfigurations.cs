using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Data.Configurations
{
    public class ProductTypeConfigurations : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).HasConversion(
                id => id.ToString(), // Convert Guid to string for storage
                str => Guid.Parse(str) // Convert string back to Guid when reading
            );  
            builder.Property(p => p.Name).IsRequired().HasMaxLength(100);
                builder.Property(p => p.Price).HasColumnType("decimal(18,2)"); // Specify precision for decimal 
        }
    }
}
