using OffersHub.Domain.Models;

namespace OffersHub.Application.Models.Offers
{
    public class OfferResponseModel
    {
        public int Id { get; set; } 
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? Image { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public DateTime OfferDueDate { get; set; }
        public OfferStatus Status { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public int CompanyId { get; set; }
        public string CompanyName { get; set;} = string.Empty;
    }
}
