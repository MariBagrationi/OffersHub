using Mapster;
using OffersHub.Application.Exceptions.Categories;
using OffersHub.Application.Models.Offers;
using OffersHub.Application.Models.Users;
using OffersHub.Application.Repositories;
using OffersHub.Domain.Models;

namespace OffersHub.API.Infrastructure.Mappings
{
    public static class MapsterConfiguration
    {
        public static void RegisterMaps(this IServiceCollection services)
        {
            TypeAdapterConfig<UserRegisterModel, User>.NewConfig()
                                                      .Map(dest => dest.Role, src => ParseRole(src.Role));

            TypeAdapterConfig<OfferRequestModel, Offer>.NewConfig()
                                                       .Ignore(dest => dest.Category)
                                                       .Ignore(dest => dest.Company);
            //.Map(dest => dest.CategoryId, src => GetCategoryId(src.Category)) 
            //.Map(dest => dest.CompanyId, src => GetCompanyId(src.Company_UserName)); 


            TypeAdapterConfig<Offer, OfferResponseModel>.NewConfig();
                                                     
        }

        private static int GetCategoryId(string category)
        {

            return category switch
            {
                "Electronics" => 1,
                "Clothing" => 2,
                // Add more mappings as needed
                _ => throw new CategoryDoesNotExist("such category does not exist")
            };
        }

        private static int GetCompanyId(string companyUserName)
        {
            // Implement logic to retrieve CompanyId based on the company username
            return 1; // Example static return value; replace with actual logic
        }

        private static Role ParseRole(string role)
        {
            return Enum.TryParse<Role>(role, true, out var parsedRole) ? parsedRole : Role.Guest;
        }
    }
}
