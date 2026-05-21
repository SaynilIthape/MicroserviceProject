using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.Domain.Enums;
using Ordering.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Data.Configurations
{
    public class OrderConfigurations : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
           builder.HasKey(x => x.Id);   

          builder.Property(o => o.Id).HasConversion(
                 id => id.ToString(), // Convert Guid to string for storage
                 str => Guid.Parse(str) // Convert string back to Guid when reading
             ); 

            builder.HasOne<Customer>()
                .WithMany()
                .HasForeignKey(o => o.CustomerId)
                .IsRequired();

            builder.HasMany(o => o.OrderItems)
                .WithOne()
                .HasForeignKey(oi => oi.OrderId);

            //builder.ComplexProperty(
            //    o => o.OrderName, newBuilder =>
            //    {
            //        newBuilder.Property(b => b.ToString())
            //        .HasColumnName(nameof(Order.OrderName))
            //        .HasMaxLength(100)
            //            .IsRequired();
            //    });

            builder.Property(o => o.OrderName)
                .HasMaxLength(100)
                .IsRequired();

            builder.ComplexProperty(o => o.ShippingAddress, newBuilder =>
            {
                newBuilder.Property(a=>a.FirstName)
                .HasMaxLength(100)
                .IsRequired();

                newBuilder.Property(a => a.LastName)
                .HasMaxLength(100).IsRequired();

                newBuilder.Property(b => b.EmailAddress)
                .HasMaxLength(50);

                newBuilder.Property(b=>b.AddressLine)
                .HasMaxLength(180) .IsRequired();

                newBuilder.Property(a => a.Country)
                .HasMaxLength(50);

                newBuilder.Property(b => b.State)
                .HasMaxLength(50);

                newBuilder.Property(a=>a.ZipCode)
                .HasMaxLength(5).IsRequired();

            });


            builder.ComplexProperty(o => o.BiilingAddress, newBuilder =>
            {
                newBuilder.Property(a => a.FirstName)
                .HasMaxLength(100)
                .IsRequired();

                newBuilder.Property(a => a.LastName)
                .HasMaxLength(100).IsRequired();

                newBuilder.Property(b => b.EmailAddress)
                .HasMaxLength(50);

                newBuilder.Property(b => b.AddressLine)
                .HasMaxLength(180).IsRequired();

                newBuilder.Property(a => a.Country)
                .HasMaxLength(50);

                newBuilder.Property(b => b.State)
                .HasMaxLength(50);

                newBuilder.Property(a => a.ZipCode)
                .HasMaxLength(5).IsRequired();

            });

            builder.ComplexProperty(o => o.Payment,
                newBuilder => {

                    newBuilder.Property(a => a.CardName)
                    .HasMaxLength(50);

                    newBuilder.Property(p=>p.CardNumber)
                    .HasMaxLength(24)
                    .IsRequired();

                    newBuilder.Property(p=>p.Expiration)
                    .HasMaxLength (10);

                    newBuilder.Property(p=>p.CVV)
                    .HasMaxLength(3);

                    newBuilder.Property(p => p.PaymentMethod);

                
                });

            builder.Property(o => o.Status)
                .HasDefaultValue(OrderStatus.Draft)
                .HasConversion(x => x.ToString(),
                 dbStatus => (OrderStatus)Enum.Parse(typeof(OrderStatus), dbStatus));

            builder.Property(o => o.TotalPrice);

        }
    }
}
