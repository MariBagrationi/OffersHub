using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OffersHub.Domain.Models;

namespace OffersHub.Persistance.Configurations
{
    public class OfferConfiguration : IEntityTypeConfiguration<Offer>
    {
        public void Configure(EntityTypeBuilder<Offer> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Price)
                   .IsRequired()
                   .HasColumnType("decimal(18,2)"); 

            builder.Property(x => x.OfferDueDate).IsRequired();
            builder.Property(x => x.Quantity).IsRequired();

            builder.Property(x => x.IsDeleted).HasDefaultValue(false);

            builder.HasOne(x => x.Category)
                   .WithMany(x => x.Offers)
                   .HasForeignKey(x => x.CategoryId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Company)
                   .WithMany(x => x.Offers)
                   .HasForeignKey(x => x.CompanyId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
