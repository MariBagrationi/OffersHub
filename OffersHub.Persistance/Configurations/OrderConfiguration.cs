using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OffersHub.Domain.Models;
using System.Reflection.Emit;

namespace OffersHub.Persistance.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(o => o.UserName)
                 .HasMaxLength(100)
                 .IsRequired();

            builder
               .Property(o => o.TotalPrice)
               .HasPrecision(18, 2); 

            builder.Property(o => o.CreatedAt)
                   .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(o => o.Status)
                   //.HasConversion<string>() // Stores enum as a string
                   .IsRequired();

            builder.HasMany(o => o.Items)
                   .WithOne(oi => oi.Order)
                   .HasForeignKey(oi => oi.OrderId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
