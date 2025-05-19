using OffersHub.Application.Repositories;
using OffersHub.Application.Services.Categories;
using OffersHub.Application.Services.Clients;
using OffersHub.Application.Services.Companies;
using OffersHub.Application.Services.Offers;
using OffersHub.Application.Services.Orders;
using OffersHub.Application.Services.Users;
using OffersHub.Domain.Contracts;
using OffersHub.Infrastructure;

namespace OffersHub.API.Infrastructure.Extensions
{
    public static class ServiceExtensions
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserRepository, UserRepository>();
            
            services.AddScoped<ICompanyService, CompanyService>();
            services.AddScoped<ICompanyRepository, CompanyRepository>();

            services.AddScoped<IClientService, ClientService>();
            services.AddScoped<IClientRepository, ClientRepository>();

            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();

            services.AddScoped<IOfferService, OfferService>();
            services.AddScoped<IOfferRepository, OfferRepository>();

            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IOrderRepository, OrderRepository>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            services.AddScoped<IClientOfferRepository, ClientOfferRepository>();
        }
    }
}
