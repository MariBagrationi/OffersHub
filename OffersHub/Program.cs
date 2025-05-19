using FluentValidation;
using Microsoft.EntityFrameworkCore;
using OffersHub.API.Infrastructure.Authentication.JWT;
using OffersHub.API.Infrastructure.Extensions;
using OffersHub.API.Infrastructure.Mappings;
using OffersHub.API.Infrastructure.Middlewares;
using OffersHub.Persistance.Connections;
using OffersHub.Persistance.Context;
using OffersHub.Persistance.Seed;
using Serilog;
using System.Reflection;

namespace OffersHub.API
{
    public class Program
    {
        public async static Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
                .CreateBootstrapLogger();


            builder.Host.UseSerilog();

            // Add services to the container.
            builder.Services.AddAuthorization();
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddSwaggerConfiguration();

            builder.Services.AddDbContext<OffersHubContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString(nameof(ConnectionStrings.DefaultConnection))));

            builder.Services.AddServices();
            builder.Services.Configure<JWTConfiguration>(builder.Configuration.GetSection(nameof(JWTConfiguration)));

            builder.Services.AddTokenAuthentication(builder.Configuration["JWTConfiguration:Secret"]!);

            builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            builder.Services.RegisterMaps();

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    await OffersHubSeed.InitializeAsync(services);
                }
                catch (Exception ex)
                {
                    Log.Error($"Error seeding database: {ex.Message}");
                }
            }

            //middlewares
            app.UseMiddleware<ExceptionHandler>();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
