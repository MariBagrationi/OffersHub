using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OffersHub.Domain.Models;
using OffersHub.Domain.Security;
using OffersHub.Persistance.Context;
using Serilog;

namespace OffersHub.Persistance.Seed
{
    public static class OffersHubSeed
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<OffersHubContext>();

            try
            {
                await MigrateDatabaseAsync(context);
                await SeedDataAsync(context);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while seeding the database.");
            }
        }
        private static async Task MigrateDatabaseAsync(OffersHubContext context)
        {
            Log.Information("Applying database migrations...");
            await context.Database.MigrateAsync();
            Log.Information("Database migrations applied.");
        }

        private static async Task SeedDataAsync(OffersHubContext context)
        {
            bool adminsSeeded = await SeedAdminsAsync(context);

            if (adminsSeeded)
            {
                await context.SaveChangesAsync();
                Log.Information("Database seeding completed.");
            }
            else
            {
                Log.Information("No new data to seed.");
            }
        }

        private static async Task<bool> SeedAdminsAsync(OffersHubContext context)
        {
            if (!await context.Users.AnyAsync(u => u.Role == Role.Admin))
            {
                var admins = new List<User>
                {
                    new User { UserName = "Admin1", PasswordHash = PasswordHasher.HashPassword("Admin*1"), Role = Role.Admin },
                    new User { UserName = "Admin2", PasswordHash = PasswordHasher.HashPassword("Admin*2"), Role = Role.Admin },
                    new User { UserName = "Admin3", PasswordHash = PasswordHasher.HashPassword("Admin*3"), Role = Role.Admin }
                };

                await context.Users.AddRangeAsync(admins);
                return true;
            }
            return false;
        }
    }
}
