using Microsoft.EntityFrameworkCore;
using OffersHub.Application.Repositories;
using OffersHub.Domain.Contracts;
using OffersHub.Infrastructure;
using OffersHub.Persistance.Context;

namespace OffersHub.WorkerService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = Host.CreateApplicationBuilder(args);

            builder.Services.AddDbContext<OffersHubContext>(options =>
                options.UseSqlServer("Server=MARI\\SQLEXPRESS;Database=OffersHub;Trusted_Connection=True;Encrypt=False;"));

            builder.Services.AddScoped<IOfferRepository, OfferRepository>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Use IServiceScopeFactory to resolve scoped services inside the worker
            builder.Services.AddSingleton<IServiceScopeFactory>(sp => sp.GetRequiredService<IServiceScopeFactory>());
            builder.Services.AddHostedService<ArchiveExpiredOffersWorker>();

            var host = builder.Build();
            host.Run();
        }
    }
}