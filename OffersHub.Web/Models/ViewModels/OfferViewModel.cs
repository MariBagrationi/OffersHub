using OffersHub.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace OffersHub.Web.Models.ViewModels
{
    public class OfferViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Title is required.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Title must be between 3 and 100 characters.")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Description is required.")]
        [StringLength(1000, ErrorMessage = "Description can't exceed 1000 characters.")]
        public string Description { get; set; } = string.Empty;

        public IFormFile? ImageFile { get; set; }

        [Required(ErrorMessage = "Quantity is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        [Range(0.01, 100000, ErrorMessage = "Price must be a positive value.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Offer due date is required.")]
        [DataType(DataType.Date)]
        public DateTime OfferDueDate { get; set; }

        public OfferStatus Status { get; set; }

        [Required]
        public string CategoryName { get; set; } = string.Empty;

        [Required]
        public string Company_UserName { get; set; } = string.Empty;
    }
}
