using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OffersHub.Domain.Models;

namespace OffersHub.Persistance.Configurations
{
    public class ClientOfferConfiguration : IEntityTypeConfiguration<ClientOffer>
    {
        public void Configure(EntityTypeBuilder<ClientOffer> builder)
        {
            builder.HasKey(x => x.Id);
            
            builder.Property(x => x.Quantity).IsRequired();

            builder.Property(x => x.DateAdded)
                   .HasDefaultValueSql("GETUTCDATE()");

            builder.HasOne(x => x.Client)
               .WithMany(x => x.Cart)
               .HasForeignKey(x => x.ClientId)
               .OnDelete(DeleteBehavior.Cascade);  

            builder.HasOne(x => x.Offer)
                   .WithMany(x => x.Clients)
                   .HasForeignKey(x => x.OfferId)
                   .OnDelete(DeleteBehavior.Restrict);  

        }
    }
}
