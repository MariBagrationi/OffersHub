using System.ComponentModel.DataAnnotations;

namespace OffersHub.Web.Models.ViewModels
{
    public class CategoryViewModel
    {
        [Required(ErrorMessage = "Category name is required.")]
        [StringLength(30, MinimumLength = 4, ErrorMessage = "Company name must be between 4 and 30 characters.")]
        public string Name { get; set; } = string.Empty;
        public IFormFile? ImageFile { get; set; }

        [StringLength(1000, ErrorMessage = "Description can't exceed 1000 characters.")]
        public string? Description { get; set; }
        public IEnumerable<OfferViewModel> Offers { get; set; } = Enumerable.Empty<OfferViewModel>();
    }
}
