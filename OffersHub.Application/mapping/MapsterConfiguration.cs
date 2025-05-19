using Mapster;
using Microsoft.Extensions.DependencyInjection;
using OffersHub.Application.Models.Offers;
using OffersHub.Domain.Models;

namespace OffersHub.Application.mapping
{
    public static class MapsterConfiguration
    {
        public static void RegisterServiceMaps(this IServiceCollection services)
        {
            TypeAdapterConfig<OfferRequestModel, Offer>.NewConfig()
                                                        .Ignore(dest => dest.CategoryId)
                                                        //.Ignore(dest => dest.Category!)
                                                        .Ignore(dest => dest.CompanyId)
                                                        .Ignore(dest => dest.Company!);
        }
    }
}
