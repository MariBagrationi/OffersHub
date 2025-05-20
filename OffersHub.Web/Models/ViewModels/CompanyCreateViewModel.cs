using System.ComponentModel.DataAnnotations;

namespace OffersHub.Web.Models.ViewModels
{
    public class CompanyCreateViewModel
    {
        [Required(ErrorMessage = "Company name is required.")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Company name must be between 2 and 30 characters.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Username is required.")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 30 characters.")]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(50, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        public IFormFile? ImageFile { get; set; }
    }
}
