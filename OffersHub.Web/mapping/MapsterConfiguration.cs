using Mapster;
using OffersHub.Application.Models.Companies;
using OffersHub.Application.Models.Users;
using OffersHub.Web.Models.ViewModels;

namespace OffersHub.Web.mapping
{
    public static class MapsterConfiguration
    {
        public static void RegisterMaps(this IServiceCollection services)
        {
            TypeAdapterConfig<RegisterViewModel, UserRegisterModel>.NewConfig();

            TypeAdapterConfig<CompanyCreateViewModel, CompanyRequestModel>
                .NewConfig()
                .Map(dest => dest.ImageData, src => src.ImageFile != null ? ConvertToBytes(src.ImageFile) : null!)
                .Map(dest => dest.Image, src => src.ImageFile != null ? Path.GetFileName(src.ImageFile.FileName) : null!);

            TypeAdapterConfig<CompanyEditViewModel, CompanyRequestModel>
               .NewConfig() 
               .Map(dest => dest.ImageData, src => src.ImageFile != null ? ConvertToBytes(src.ImageFile) : null!) 
               .Map(dest => dest.Image, src => src.ImageFile != null ? Path.GetFileName(src.ImageFile.FileName) : null!); 

        }

        private static byte[] ConvertToBytes(IFormFile file)
        {
            using var ms = new MemoryStream();
            file.CopyTo(ms);
            return ms.ToArray();
        }
    }
}
