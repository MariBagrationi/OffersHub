using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OffersHub.Domain.Models;

namespace OffersHub.Persistance.Configurations
{
    public class CompanyConfiguration : IEntityTypeConfiguration<Company>
    {
        public void Configure(EntityTypeBuilder<Company> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.IsDeleted).HasDefaultValue(false);

            builder.Property(x => x.Image)
               .HasMaxLength(255);

            builder.Property(x => x.IsActive)
                   .IsRequired();

            builder.HasOne(x => x.User)
               .WithOne() 
               .HasForeignKey<Company>(x => x.UserId)
               .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
