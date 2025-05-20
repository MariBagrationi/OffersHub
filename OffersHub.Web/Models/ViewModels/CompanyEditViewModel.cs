using System.ComponentModel.DataAnnotations;

namespace OffersHub.Web.Models.ViewModels
{
    public class CompanyEditViewModel
    {
        [Required(ErrorMessage = "Username is required.")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 30 characters.")]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Company name is required.")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Company name must be between 2 and 30 characters.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Company image is required.")]
        public IFormFile? ImageFile { get; set; }
    }
}
