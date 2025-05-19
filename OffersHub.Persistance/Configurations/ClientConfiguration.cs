using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OffersHub.Domain.Models;

namespace OffersHub.Persistance.Configurations
{
    public class ClientConfiguration : IEntityTypeConfiguration<Client>
    {
        public void Configure(EntityTypeBuilder<Client> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.IsDeleted).HasDefaultValue(false);

            builder.Property(x => x.Balance)
              .HasColumnType("decimal(18,2)")
              .IsRequired();

            builder.HasOne(x => x.User)
                   .WithOne()
                   .HasForeignKey<Client>(x => x.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(c => c.Orders)
               .WithOne()
               .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
