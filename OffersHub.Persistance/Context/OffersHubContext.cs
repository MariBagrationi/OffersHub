using Microsoft.EntityFrameworkCore;
using OffersHub.Domain.Models;

namespace OffersHub.Persistance.Context
{
    public class OffersHubContext : DbContext
    {
        public OffersHubContext(DbContextOptions<OffersHubContext> options) : base(options) { }

        public DbSet<Offer> Offers { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<ClientOffer> ClientOffers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Automatically apply all IEntityTypeConfiguration<> classes
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(OffersHubContext).Assembly);

            // Apply a global query filter for soft deletes
            modelBuilder.Entity<Offer>().HasQueryFilter(p => !p.IsDeleted);
            modelBuilder.Entity<Client>().HasQueryFilter(c => !c.IsDeleted);
            modelBuilder.Entity<Company>().HasQueryFilter(c => !c.IsDeleted);
            modelBuilder.Entity<Category>().HasQueryFilter(c => !c.IsDeleted);
            modelBuilder.Entity<User>().HasQueryFilter(c => !c.IsDeleted);
            modelBuilder.Entity<ClientOffer>().HasQueryFilter(c => !c.IsDeleted);
            modelBuilder.Entity<Order>().HasQueryFilter(c => !c.IsDeleted);
            modelBuilder.Entity<OrderItem>().HasQueryFilter(c => !c.IsDeleted);
        }

    }

    //public class AppDbContext : IdentityDbContext<ApplicationUser>
    //{
    //    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    //    {

    //    }
    //    public DbSet<Offer> Offers { get; set; }
    //    //public DbSet<ApplicationUser> AspNetUsers { get; set; }
    //    public DbSet<Company> Companies { get; set; }
    //    public DbSet<Client> Clients { get; set; }
    //    public DbSet<ClientOffer> ClientProducts { get; set; }
    //    public DbSet<Order> Orders { get; set; }
    //    public DbSet<OrderItem> OrderItems { get; set; }

    //    protected override void OnModelCreating(ModelBuilder modelBuilder)
    //    {
    //        base.OnModelCreating(modelBuilder); // <--- important for Identity!

    //        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

    //        modelBuilder.Entity<ApplicationUser>(entity =>
    //        {
    //            entity.ToTable("AspNetUsers"); // Explicitly set the table name to "AspNetUsers"
    //        });

    //        modelBuilder.Entity<Offer>().HasQueryFilter(p => !p.IsDeleted);
    //        modelBuilder.Entity<Client>().HasQueryFilter(c => !c.IsDeleted);
    //        modelBuilder.Entity<Company>().HasQueryFilter(c => !c.IsDeleted);
    //        modelBuilder.Entity<Category>().HasQueryFilter(c => !c.IsDeleted);
    //        //modelBuilder.Entity<ApplicationUser>().HasQueryFilter(c => !c.IsDeleted);
    //        modelBuilder.Entity<ClientOffer>().HasQueryFilter(c => !c.IsDeleted);
    //        modelBuilder.Entity<Order>().HasQueryFilter(c => !c.IsDeleted);
    //        modelBuilder.Entity<OrderItem>().HasQueryFilter(c => !c.IsDeleted);
    //    }
    //}

    //public class ApplicationUser : IdentityUser
    //{
    //}
}
