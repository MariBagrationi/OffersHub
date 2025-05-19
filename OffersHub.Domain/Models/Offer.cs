
namespace OffersHub.Domain.Models
{
    public class Offer
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? Image {  get; set; } 
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public DateTime OfferDueDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public OfferStatus Status { get; set; }
        public bool IsDeleted { get; set; }

       
        public int CategoryId { get; set; }
        public Category? Category { get; set; } = new();
        
        public int CompanyId { get; set; }
        public Company? Company { get; set; } = new();

        public List<ClientOffer> Clients { get; set; } = new();
    }
}
